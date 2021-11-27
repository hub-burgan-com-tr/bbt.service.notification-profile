using Microsoft.EntityFrameworkCore;


public class DatabaseContext : DbContext
{
    public DbSet<Source> Sources { get; set; }
    public DbSet<Consumer> Consumers { get; set; }

    public string DbPath { get; private set; }
    public DatabaseContext()
    {
        DbPath = $"notification.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Consumer>().OwnsOne(p => p.Phone);
    }
}