using System.ComponentModel.DataAnnotations;
namespace Clinic_Management_System.Model
{
    public class LoginDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
