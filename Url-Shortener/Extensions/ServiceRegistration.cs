using Microsoft.EntityFrameworkCore;
using Url_Shortener.Data;
using Url_Shortener.Data.Abstractions;
using Url_Shortener.Data.Repositories;
using Url_Shortener.Services;
using Url_Shortener.Services.Abstractions;

namespace Url_Shortener.Extensions;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine(env);
        if (env == "Production")
        {
            var postgresConnection = Environment.GetEnvironmentVariable("PostgresConnection");
            if (string.IsNullOrEmpty(postgresConnection))
            {
                throw new InvalidOperationException("PostgresConnection secret is not set.");
            }
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(postgresConnection));
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("SqlServerConnection"));
        }

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUrlService, UrlService>();
    }
}
