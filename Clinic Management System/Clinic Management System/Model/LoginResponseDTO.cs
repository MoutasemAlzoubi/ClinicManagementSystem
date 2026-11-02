namespace Clinic_Management_System.Model
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
