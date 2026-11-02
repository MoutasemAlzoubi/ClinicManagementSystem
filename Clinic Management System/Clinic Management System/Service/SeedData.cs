using Clinic_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Management_System.Service
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services, IConfiguration config)
        {
            using (IServiceScope scope = services.CreateScope())
            {
                ClinicContext context = scope.ServiceProvider.GetRequiredService<ClinicContext>();
                context.Database.Migrate();

                if (!context.Countries.Any())
                {
                    context.Countries.AddRange(
                        new Country { Name = "Jordan" },
                        new Country { Name = "United States" },
                        new Country { Name = "United Kingdom" },
                        new Country { Name = "Canada" },
                        new Country { Name = "United Arab Emirates" },
                        new Country { Name = "Saudi Arabia" });
                    context.SaveChanges();
                }

                RoleManager<IdentityRole> roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                foreach (string role in new[] { "Admin", "Doctor", "Patient" })
                {
                    if (!roles.RoleExistsAsync(role).GetAwaiter().GetResult()) roles.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }

                string email = config["AdminSeed:Email"];
                string password = config["AdminSeed:Password"];
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;
                UserManager<ApplicationUser> users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (users.FindByEmailAsync(email).GetAwaiter().GetResult() != null) return;
                ApplicationUser admin = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true, FirstName = "System", LastName = "Administrator" };
                var result = users.CreateAsync(admin, password).GetAwaiter().GetResult();
                if (!result.Succeeded) throw new InvalidOperationException("Admin seed failed: " + string.Join("; ", result.Errors.Select(e => e.Description)));
                users.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
            }
        }
    }
}
