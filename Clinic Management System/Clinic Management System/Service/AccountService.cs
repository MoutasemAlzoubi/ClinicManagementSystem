using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clinic_Management_System.Data;
using Clinic_Management_System.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Clinic_Management_System.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> users;
        private readonly ClinicContext context;
        private readonly IConfiguration config;

        public AccountService(UserManager<ApplicationUser> _users, ClinicContext _context, IConfiguration _config)
        {
            users = _users;
            context = _context;
            config = _config;
        }

        public async Task<(bool Success, string Error, LoginResponseDTO Data)> Login(LoginDTO loginDTO)
        {
            ApplicationUser user = await users.FindByEmailAsync(loginDTO.Email);
            if (user == null || !await users.CheckPasswordAsync(user, loginDTO.Password)) return (false, "Invalid email or password.", null);

            Doctor doctor = context.doctors.FirstOrDefault(d => d.UserId == user.Id);
            if (doctor != null && !doctor.IsActive) return (false, "This doctor account is inactive.", null);

            IList<string> roles = await users.GetRolesAsync(user);
            string key = config["Jwt:Key"];
            DateTime expires = DateTime.UtcNow.AddHours(4);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            foreach (string role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

            JwtSecurityToken token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"], claims, DateTime.UtcNow, expires, new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
            LoginResponseDTO response = new LoginResponseDTO();
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.ExpiresAt = expires;
            response.Email = user.Email;
            response.FullName = user.FirstName + " " + user.LastName;
            response.Roles = roles;
            return (true, "", response);
        }

        public async Task<(bool Success, string Error)> RegisterPatient(RegisterPatientDTO patientDTO)
        {
            if (await users.FindByEmailAsync(patientDTO.Email) != null) return (false, "Email is already registered.");
            if (!context.Countries.Any(c => c.Id == patientDTO.CountryId)) return (false, "Selected country does not exist.");

            Patient patient = context.patients.FirstOrDefault(p => p.Email == patientDTO.Email);
            if (patient != null && patient.UserId != null) return (false, "A patient profile with this email is already linked to an account.");

            ApplicationUser user = new ApplicationUser();
            user.UserName = patientDTO.Email;
            user.Email = patientDTO.Email;
            user.FirstName = patientDTO.FirstName;
            user.LastName = patientDTO.LastName;
            user.PhoneNumber = patientDTO.Phone;
            user.EmailConfirmed = true;

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
            var result = await users.CreateAsync(user, patientDTO.Password);
            if (!result.Succeeded) { await transaction.RollbackAsync(); return (false, string.Join(" ", result.Errors.Select(e => e.Description))); }
            var roleResult = await users.AddToRoleAsync(user, "Patient");
            if (!roleResult.Succeeded) { await transaction.RollbackAsync(); return (false, "Could not assign Patient role."); }

            if (patient == null) { patient = new Patient(); context.patients.Add(patient); }
            patient.FirstName = patientDTO.FirstName;
            patient.LastName = patientDTO.LastName;
            patient.Email = patientDTO.Email;
            patient.DOB = patientDTO.DOB;
            patient.Gender = patientDTO.Gender;
            patient.Address = patientDTO.Address;
            patient.Phone = patientDTO.Phone;
            patient.CountryId = patientDTO.CountryId;
            patient.UserId = user.Id;
            context.SaveChanges();
            await transaction.CommitAsync();
            }
            return (true, "");
        }
    }
}
