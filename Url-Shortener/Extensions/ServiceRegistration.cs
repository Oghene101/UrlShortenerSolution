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

        string postgresConnection = (env == "Production")
            ? Environment.GetEnvironmentVariable("DefaultConnection") ?? configuration.GetConnectionString("DefaultConnection")!
            : configuration.GetConnectionString("DefaultConnection")!;

        if (string.IsNullOrEmpty(postgresConnection))
        {
            throw new InvalidOperationException("DefaultConnection secret is not set.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(postgresConnection));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUrlService, UrlService>();

        // Check for pending migrations
        using var context = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
        var pendingMigrations = context.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            try
            {
                Console.WriteLine("Applying migrations...");
                context.Database.Migrate();
                Console.WriteLine("Migrations applied successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
                throw;
            }
        }
        else Console.WriteLine("No pending migrations.");
    }
}
