using Microsoft.EntityFrameworkCore;
using the_fitness_assistant.Data;
using the_fitness_assistant.Models;

namespace the_fitness_assistant.Services;

/// <summary>
/// Everything the food library page needs.
///
/// Foods with a null CreatedByUserId are shared with everybody; foods with a
/// user id belong to that user and only they may edit or delete them.
/// </summary>
public class FoodService
{
    private readonly ApplicationDbContext _context;

    public FoodService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Foods this user is allowed to see: the shared list plus their own.
    /// An empty search term returns everything.
    /// </summary>
    public async Task<List<Food>> SearchAsync(int userId, string? searchTerm)
    {
        var query = _context.Foods
            .Where(f => f.CreatedByUserId == null || f.CreatedByUserId == userId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string term = searchTerm.Trim();

            query = query.Where(f => EF.Functions.ILike(f.Name, $"%{term}%"));
        }

        return await query
            .OrderBy(f => f.Name)
            .ToListAsync();
    }

    public async Task<Food?> GetByIdAsync(int foodId)
    {
        return await _context.Foods
            .FirstOrDefaultAsync(f => f.FoodId == foodId);
    }

    /// <summary>
    /// Adds a food owned by this user. Returns an error message when the user
    /// already has a food by that name, so the library does not fill up with
    /// near-duplicates.
    /// </summary>
    public async Task<(bool Success, string Message)> AddAsync(
        int userId,
        string name,
        int calories,
        string servingSize)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return (false, "Give the food a name.");
        }

        if (calories < 0)
        {
            return (false, "Calories cannot be negative.");
        }

        string cleanName = name.Trim();

        bool duplicate = await _context.Foods.AnyAsync(f =>
            f.CreatedByUserId == userId &&
            f.Name.ToLower() == cleanName.ToLower());

        if (duplicate)
        {
            return (false, $"You already have a food called \"{cleanName}\".");
        }

        var food = new Food
        {
            Name = cleanName,
            Calories = calories,
            ServingSize = string.IsNullOrWhiteSpace(servingSize)
                ? "1 serving"
                : servingSize.Trim(),
            CreatedByUserId = userId
        };

        _context.Foods.Add(food);

        await _context.SaveChangesAsync();

        return (true, $"Added \"{food.Name}\".");
    }

    /// <summary>
    /// Edits a food. Shared foods and other people's foods are refused.
    /// </summary>
    public async Task<(bool Success, string Message)> UpdateAsync(
        int userId,
        int foodId,
        string name,
        int calories,
        string servingSize)
    {
        var food = await GetByIdAsync(foodId);

        if (food == null)
        {
            return (false, "That food no longer exists.");
        }

        if (food.CreatedByUserId != userId)
        {
            return (false, "You can only edit foods you added yourself.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return (false, "Give the food a name.");
        }

        if (calories < 0)
        {
            return (false, "Calories cannot be negative.");
        }

        food.Name = name.Trim();
        food.Calories = calories;
        food.ServingSize = string.IsNullOrWhiteSpace(servingSize)
            ? "1 serving"
            : servingSize.Trim();

        await _context.SaveChangesAsync();

        return (true, $"Saved \"{food.Name}\".");
    }

    /// <summary>
    /// Deletes a food the user owns.
    ///
    /// The FoodLogEntry foreign key cascades, so deleting a food that is still
    /// logged would silently wipe those log entries and change the user's
    /// history. That is refused instead.
    /// </summary>
    public async Task<(bool Success, string Message)> DeleteAsync(
        int userId,
        int foodId)
    {
        var food = await GetByIdAsync(foodId);

        if (food == null)
        {
            return (false, "That food no longer exists.");
        }

        if (food.CreatedByUserId != userId)
        {
            return (false, "You can only delete foods you added yourself.");
        }

        int timesLogged = await _context.FoodLogEntries
            .CountAsync(f => f.FoodId == foodId);

        if (timesLogged > 0)
        {
            return (false,
                $"\"{food.Name}\" is used in {timesLogged} log " +
                $"{(timesLogged == 1 ? "entry" : "entries")}. " +
                "Remove those from the tracker first.");
        }

        _context.Foods.Remove(food);

        await _context.SaveChangesAsync();

        return (true, $"Deleted \"{food.Name}\".");
    }

    /// <summary>
    /// How many times the user has logged each food, so the library can show
    /// which entries are safe to delete.
    /// </summary>
    public async Task<Dictionary<int, int>> GetLogCountsAsync(int userId)
    {
        return await _context.FoodLogEntries
            .Where(f => f.UserId == userId)
            .GroupBy(f => f.FoodId)
            .Select(g => new { FoodId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.FoodId, x => x.Count);
    }
}
