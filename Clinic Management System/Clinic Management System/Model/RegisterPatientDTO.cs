using System.ComponentModel.DataAnnotations;
namespace Clinic_Management_System.Model
{
    public class RegisterPatientDTO
    {
        [Required, StringLength(50)] public string FirstName { get; set; }
        [Required, StringLength(50)] public string LastName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, MinLength(3)] public string Password { get; set; }
        [Required] public DateTime DOB { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string Phone { get; set; }
        [Required] public int CountryId { get; set; }
    }
}
