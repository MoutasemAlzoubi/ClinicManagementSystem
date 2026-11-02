using System.ComponentModel.DataAnnotations;

namespace Clinic_Management_System.Model
{
    public class CountryDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
