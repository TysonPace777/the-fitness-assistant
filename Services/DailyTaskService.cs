using Microsoft.EntityFrameworkCore;
using the_fitness_assistant.Data;
using the_fitness_assistant.Models;
using the_fitness_assistant.Models.Enums;

namespace the_fitness_assistant.Services;

public class DailyTaskService
{
    private readonly ApplicationDbContext _context;

    public DailyTaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task EnsureTodaysTasksExistAsync(int userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var activeTasks = await _context.DailyTasks
            .Where(t => t.UserId == userId && t.IsActive)
            .ToListAsync();


        var existingCompletions = await _context.DailyTaskCompletions
            .Where(c =>
                c.Date == today &&
                c.DailyTask.UserId == userId)
            .Select(c => c.DailyTaskId)
            .ToListAsync();


        var missingCompletions = activeTasks
            .Where(t => !existingCompletions.Contains(t.DailyTaskId))
            .Select(t => new DailyTaskCompletion
            {
                DailyTaskId = t.DailyTaskId,
                Date = today,
                Completed = false
            })
            .ToList();


        if (missingCompletions.Count > 0)
        {
            await _context.DailyTaskCompletions
                .AddRangeAsync(missingCompletions);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<DailyTaskCompletion>> GetTodaysTasksAsync(int userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return await _context.DailyTaskCompletions
            .Include(c => c.DailyTask)
            .Where(c =>
                c.Date == today &&
                c.DailyTask.UserId == userId)
            .OrderBy(c => c.DailyTask.DisplayOrder)
            .ToListAsync();
    }

    public async Task ToggleCompletionAsync(int completionId)
    {
        var completion = await _context.DailyTaskCompletions
            .FirstOrDefaultAsync(c => c.DailyTaskCompletionId == completionId);

        if (completion == null)
            return;


        completion.Completed = !completion.Completed;


        if (completion.Completed)
        {
            completion.CompletedAt = DateTime.UtcNow;
        }
        else
        {
            completion.CompletedAt = null;
        }


        await _context.SaveChangesAsync();
    }

    public async Task<DailyProgressStatus> GetProgressStatusAsync(int userId)
    {
        var tasks = await GetTodaysTasksAsync(userId);

        if (!tasks.Any())
        {
            return DailyProgressStatus.NoProgress;
        }

        var completedCount = tasks.Count(t => t.Completed);

        return completedCount switch
        {
            0 => DailyProgressStatus.NoProgress,

            _ when completedCount == tasks.Count
                => DailyProgressStatus.Completed,

            _ => DailyProgressStatus.InProgress
        };
    }

    public async Task CreateTaskAsync(DailyTask task)
    {
        task.IsActive = true;

        _context.DailyTasks.Add(task);

        await _context.SaveChangesAsync();


        // Create today's completion record
        var completion = new DailyTaskCompletion
        {
            DailyTaskId = task.DailyTaskId,
            Date = DateOnly.FromDateTime(DateTime.Today),
            Completed = false
        };

        _context.DailyTaskCompletions.Add(completion);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        var task = await _context.DailyTasks
            .FirstOrDefaultAsync(t => t.DailyTaskId == taskId);

        if (task == null)
            return;

        task.IsActive = false;

        await _context.SaveChangesAsync();
    }

    public async Task<List<DailyTask>> GetUserTasksAsync(int userId)
    {
        return await _context.DailyTasks
            .Where(t =>
                t.UserId == userId &&
                t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
    }

    public async Task UpdateTaskAsync(DailyTask task)
    {
        var existingTask = await _context.DailyTasks
            .FirstOrDefaultAsync(t =>
                t.DailyTaskId == task.DailyTaskId);

        if (existingTask == null)
            return;


        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.DisplayOrder = task.DisplayOrder;

        await _context.SaveChangesAsync();
    }
}