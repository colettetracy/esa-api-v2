using ESA.Core.Interfaces;
using ESA.Infrastructure.Data;
using ESA.Infrastructure.Services;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;

namespace ESA.Infrastructure
{
    public static class Startup
    {
        public static void AddInfrastructureContext(this IServiceCollection services, string connectionString, bool useEnvironmentVariable = false, string environmentVariableName = "")
        {
            if (useEnvironmentVariable)
            {
                var connStringBuilder = new SqlConnectionStringBuilder
                {
                    ConnectionString = connectionString
                };
                if (string.IsNullOrEmpty(connStringBuilder.Password))
                {
                    connStringBuilder.Password = EnvironmentManagement.GetVariable(environmentVariableName);
                    connectionString = connStringBuilder.ConnectionString;
                }
            }
            services.AddDbContext<AcademyDbContext>(options => options.UseNpgsql(connectionString));
        }

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            var ssl = new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, sslErrors) => true };
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerService<>));
            services.AddScoped(typeof(IRepository<>), typeof(AcademyRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(AcademyRepository<>));
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
