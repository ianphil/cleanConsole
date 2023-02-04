using Microsoft.EntityFrameworkCore;

namespace CleanConsole.Entities;

public class SimplifiedContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    public SimplifiedContext(DbContextOptions<SimplifiedContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}