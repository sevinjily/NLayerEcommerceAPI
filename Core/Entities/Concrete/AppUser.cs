using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Concrete
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? OTP { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int FailedAttempts { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiredDate { get; set; }
        virtual public string? PhotoUrl { get; set; }   
      
    }
}
    