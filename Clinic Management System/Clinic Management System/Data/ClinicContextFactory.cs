using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace Clinic_Management_System.Data;
public class ClinicContextFactory : IDesignTimeDbContextFactory<ClinicContext>
{
    public ClinicContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .AddEnvironmentVariables()
        .Build();
        var optionsBuilder = new DbContextOptionsBuilder<ClinicContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("ClinicConn"));
        return new ClinicContext(optionsBuilder.Options);
    }
}
