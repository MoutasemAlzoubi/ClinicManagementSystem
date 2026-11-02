using System.ComponentModel.DataAnnotations;
namespace Clinic_Management_System.Model
{
    public class CreateDoctorDTO
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required]
        public string TemporaryPassword { get; set; }
        [Required] public string Specialization { get; set; }
        public string? Bio { get; set; }
    }
}
