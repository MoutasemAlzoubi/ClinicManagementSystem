using System.ComponentModel.DataAnnotations;

namespace Clinic_Management_System.Model
{
    public class DoctorDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; }

        public string? Bio { get; set; }
        public bool IsActive { get; set; }
    }
}
