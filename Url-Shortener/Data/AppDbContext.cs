using Microsoft.EntityFrameworkCore;
using Url_Shortener.Models.Entities;

namespace Url_Shortener.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Link> Links { get; set; }
}
