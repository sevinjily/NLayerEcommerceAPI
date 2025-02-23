using Business.Abstract;
using Business.Messages;
using Business.Validations.FluentValidation;
using Core.Entities.Concrete;
using Core.Utilities.Message.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccesResults;
using Core.Utilities.Security.Abstract;
using Entities.Common;
using Entities.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Net;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;     
        private readonly ITokenService _tokenService;
        private readonly IMessageService _messageService;   
        public AuthManager(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMessageService messageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _messageService = messageService;
        }



        public async Task<IDataResult<Token>> LoginAsync(LoginDTO loginDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(loginDTO.UsernameOrEmail);
            if (findUser == null)
                findUser = await _userManager.FindByNameAsync(loginDTO.UsernameOrEmail);

            if (findUser == null)
                return new ErrorDataResult<Token>(message: "User does not exist!", HttpStatusCode.NotFound);

            if (findUser.EmailConfirmed == false)
            {
                return new ErrorDataResult<Token>(message: "User not confirmed",HttpStatusCode.BadRequest);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(findUser, loginDTO.Password, false);
            var userRoles = await _userManager.GetRolesAsync(findUser);
            if (result.Succeeded)
            {
                Token token = await _tokenService.CreateAccessToken(findUser, roles: userRoles.ToList());
               var response = await UpdateRefreshToken(token.RefreshToken,findUser);
                return new SuccessDataResult<Token>(data: token, statusCode: HttpStatusCode.OK, message: response.Message);
            }
            else
            {
                Log.Error("Username or Password is not valid");
                return new ErrorDataResult<Token>(message: "Username or Password is not valid", HttpStatusCode.BadRequest);
            }

        }

        public async Task<IResult> LogOut(string userId)
        {
           var user=await _userManager.FindByIdAsync(userId);
            if(user is not null)
            {
                    user.RefreshToken = null;
                user.RefreshTokenExpiredDate=null;
                var result=await _userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return new SuccessResult(HttpStatusCode.OK);
                }
                else
                {
                    string response = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        response += error.Description + ".";
                    }
                    return new ErrorResult(response, HttpStatusCode.BadRequest);
                }   
            }
            return new ErrorResult(HttpStatusCode.NotFound);
        }

        public async Task<IDataResult<Token>> RefreshTokenLoginAsync(string refreshToken)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.RefreshToken == refreshToken);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (user is not null && user.RefreshTokenExpiredDate > DateTime.Now)
            {
                Token token = await _tokenService.CreateAccessToken(user, userRoles.ToList());
                token.RefreshToken = refreshToken;
                return new SuccessDataResult<Token>(data: token, statusCode: HttpStatusCode.OK);

            }
            else
            {
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message:AuthMessage.UserNotFound);
            }
        }
        private string GenerateOtp()
        {
            byte[] data = new byte[4];

            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(data);
            int value = BitConverter.ToInt32(data, 0);
            return Math.Abs(value % 900000).ToString("D6");
        }

        //[ValidationAspect(typeof(RegisterValidator))]
        public async Task<IResult> RegisterAsync(RegisterDTO model)
        {

            var validator = new RegisterValidation();
            var validationResult=validator.Validate(model);
            if (!validationResult.IsValid)
            {
                Log.Error(validationResult.ToString());
                return new ErrorResult(message: validationResult.ToString(),HttpStatusCode.BadRequest);
            }

            AppUser newUser = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                OTP = GenerateOtp(),
                ExpiredDate = DateTime.Now.AddMinutes(3),
                EmailConfirmed=false

            };
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
               await _messageService.SendMessage(newUser.Email, "Welcome", newUser.OTP);
                return new SuccessResult(System.Net.HttpStatusCode.Created);
            }
            else
            {
                string response = string.Empty;
                foreach (var error in result.Errors)
                {
                    response += error.Description+".";

                }
                return new ErrorResult(response, System.Net.HttpStatusCode.BadRequest);
            }
        }
        private string GenerateNewOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // 6 rəqəmli OTP
        }

        public async Task<IResult> UserEmailConfirm(string email, string otp)
        {
            var findUser = _userManager.Users.FirstOrDefault(x=>x.Email==email);

            if (findUser == null)
                return new ErrorResult("User not found.", HttpStatusCode.NotFound);

            if (findUser.EmailConfirmed)
                return new ErrorResult("Email is already confirmed.", HttpStatusCode.BadRequest);


            if (findUser.LockoutEnd.HasValue && findUser.LockoutEnd > DateTime.Now)
            {
                return new ErrorResult("Too many failed attempts. Try again later.", HttpStatusCode.Forbidden);
            }

            if (string.IsNullOrEmpty(findUser.OTP) || findUser.ExpiredDate <= DateTime.Now)
            {
                string newOtp = GenerateNewOTP();
                findUser.OTP = newOtp;
                findUser.ExpiredDate = DateTime.Now.AddMinutes(5); // Yeni OTP 5 dəqiqə keçərli olacaq
                findUser.FailedAttempts = 0; // Yanlış cəhdləri sıfırlayırıq
                await _userManager.UpdateAsync(findUser);


                await _messageService.SendMessage(findUser.Email,"Welcome", findUser.OTP);
                return new ErrorResult("OTP expired. A new OTP has been sent.", HttpStatusCode.BadRequest);
            }

            if (findUser.FailedAttempts > 3) // 3 səhv + 3 yeni kodla səhv
            {
                findUser.LockoutEnd = DateTime.Now.AddMinutes(15); // 15 dəqiqəlik bloklama
                await _userManager.UpdateAsync(findUser);
                return new ErrorResult("Too many failed attempts. Your account is temporarily locked.", HttpStatusCode.Forbidden);
            }

            if (findUser.FailedAttempts == 3)
            {

                string newOtp = GenerateNewOTP();
            findUser.OTP = newOtp;
            findUser.ExpiredDate = DateTime.Now.AddMinutes(5);
                findUser.FailedAttempts++; // Yanlış OTP daxil edilibsə, say artırılır
                await _userManager.UpdateAsync(findUser);

                await  _messageService.SendMessage(findUser.Email, "Welcome", newOtp);
                return new ErrorResult("Too many failed attempts. A new OTP has been sent.", HttpStatusCode.Forbidden);
            }

           

            if (findUser.OTP.Length < 6)
                return new ErrorResult("Invalid OTP format.", HttpStatusCode.BadRequest);


            if (findUser.OTP == otp && findUser.ExpiredDate > DateTime.Now)
            {
                findUser.EmailConfirmed = true;
                findUser.OTP = null; // OTP-ni ləğv edirik
                findUser.FailedAttempts = 0; // Səhv cəhd sayını sıfırlayırıq
                await _userManager.UpdateAsync(findUser);
                return new SuccessResult(HttpStatusCode.OK);
            }

                findUser.FailedAttempts++; // Yanlış OTP daxil edilibsə, say artırılır

            await _userManager.UpdateAsync(findUser);

            return new ErrorResult("Invalid OTP or expired.", HttpStatusCode.BadRequest);

        }
    

        public async Task<IDataResult<string>> UpdateRefreshToken(string refreshToken, AppUser appUser)
        {
            if (appUser is not null)
            {
                appUser.RefreshToken = refreshToken;
                appUser.RefreshTokenExpiredDate = DateTime.UtcNow.AddMonths(1);
                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    return new SuccessDataResult<string>(data: refreshToken, HttpStatusCode.OK);

                }
                else
                {
                    string response = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        response += error.Description + ".";
                    }
                    return new ErrorDataResult<string>(message: response, HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return new ErrorDataResult<string>(HttpStatusCode.NotFound);
            }
        }
       
    }
}
