using Microsoft.AspNetCore.Identity;

namespace Clinic_Management_System.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}
