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
                ExpiredDate = DateTime.Now.AddMinutes(3)
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
