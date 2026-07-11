using Microsoft.EntityFrameworkCore;
using the_fitness_assistant.Models;

namespace the_fitness_assistant.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Food> Foods { get; set; }
    public DbSet<FoodLogEntry> FoodLogEntries { get; set; }

    public DbSet<CalorieGoal> CalorieGoals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User -> FoodLogEntries
        modelBuilder.Entity<FoodLogEntry>()
            .HasOne(f => f.User)
            .WithMany(u => u.FoodLogEntries)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Food -> FoodLogEntries
        modelBuilder.Entity<FoodLogEntry>()
            .HasOne(f => f.Food)
            .WithMany(food => food.FoodLogEntries)
            .HasForeignKey(f => f.FoodId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> Created Foods
        modelBuilder.Entity<Food>()
            .HasOne(f => f.CreatedByUser)
            .WithMany(u => u.Foods)
            .HasForeignKey(f => f.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // User -> CalorieGoals
        modelBuilder.Entity<CalorieGoal>()
            .HasOne(c => c.User)
            .WithMany(u => u.CalorieGoals)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
