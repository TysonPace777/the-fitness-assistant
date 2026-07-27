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

    public DbSet<DailyTask> DailyTasks { get; set; }

    public DbSet<DailyTaskCompletion> DailyTaskCompletions { get; set; }

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

        // User -> DailyTasks
        modelBuilder.Entity<DailyTask>()
            .HasOne(t => t.User)
            .WithMany(u => u.DailyTasks)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // DailyTask -> DailyTaskCompletions
        modelBuilder.Entity<DailyTaskCompletion>()
            .HasOne(c => c.DailyTask)
            .WithMany(t => t.Completions)
            .HasForeignKey(c => c.DailyTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent duplicate completion records for the same day
        modelBuilder.Entity<DailyTaskCompletion>()
            .HasIndex(c => new { c.DailyTaskId, c.Date })
            .IsUnique();
    }
}
