using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.APIModel.Login
{
    public class LoginRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
