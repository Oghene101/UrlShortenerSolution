using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Entities;

namespace UrlShortener.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Link> Links { get; set; }
}
