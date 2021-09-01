using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenSchool.src.Data;

namespace OpenSchool.src.Installers
{
    public class DbContextInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connection = @"host=db;port=5432;database=blogdb;username=bloguser;password=bloguser";
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));

        }
    }
}
