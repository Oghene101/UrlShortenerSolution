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

        // Determine the database to use based on the environment
        //var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //if (env == "Production")
        //{
        //    // Use PostgreSQL in production
        //    services.AddDbContext<AppDbContext>(options =>
        //        options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
        //}
        //else
        //{
        //    // Use SQL Server in development
        //    services.AddDbContext<AppDbContext>(options =>
        //        options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));
        //}


        //services.AddDbContext<AppDbContext>(options =>
        //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUrlService, UrlService>();
    }
}
