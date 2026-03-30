using Microsoft.EntityFrameworkCore;


namespace SmartMealConsoleClient.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DomainModels.Dish> Dishes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DomainModels.Dish>().HasKey(x => x.Id);
    }
}