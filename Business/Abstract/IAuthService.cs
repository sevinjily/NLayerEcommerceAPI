    using Core.Entities.Concrete;
    using Core.Utilities.Results.Abstract;
    using Entities.DTOs.AuthDTOs;

    namespace Business.Abstract
    {
        public interface IAuthService
        {
            Task<IResult> RegisterAsync(RegisterDTO model);
            Task<IDataResult<Token>> LoginAsync(LoginDTO loginDTO);
            Task<IDataResult<string>> UpdateRefreshToken(string refreshToken, AppUser appUser);
            Task<IDataResult<Token>> RefreshTokenLoginAsync(string refreshToken);
            Task<IResult> LogOut(string userId);
            Task<IResult> UserEmailConfirm(string email, string otp);
             Task<IResult> HardDeleteUser(string id);
             Task<IResult> SoftDeleteUser(string id);


    }
}
