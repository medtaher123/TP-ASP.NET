using System.Security.Claims;
using ChronoLink.Data;
using ChronoLink.Models;
using Microsoft.EntityFrameworkCore;
using Task = ChronoLink.Models.Task;
using ChronoLink.Controllers;
namespace ChronoLink.Services
{
    public interface ITaskService
    {
        Task<List<Task>> GetAllTasksAsync(string userId, int? workspace);
        Task<List<Task>> GetMyTasksAsync(string userId, int? workspace);
        Task<Task?> GetTaskAsync(int taskId, string userId);
        Task<Task> CreateTaskAsync(CreateTaskRequest request, int? workspace);
        Task<Task?> UpdateTaskAsync(int taskId, UpdateTaskRequest request);
        Task<bool> DeleteTaskAsync(int taskId);
    }

    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;

        public TaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Task>> GetAllTasksAsync(string userId, int? workspace)
        {
            IQueryable<Task> query = _dbContext.Tasks;

            if (workspace.HasValue)
            {
                var isMember = await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                    wu.UserId == userId && wu.WorkspaceId == workspace.Value
                );

                if (!isMember)
                {
                    throw new UnauthorizedAccessException("User is not a member of the workspace.");
                }

                query = query.Where(t => t.WorkspaceUser.WorkspaceId == workspace.Value);
            }

            return await query
                .Select(t => new Task
                {
                    Id = t.Id,
                    Description = t.Description,
                    StartDateTime = t.StartDateTime,
                    EndDateTime = t.EndDateTime,
                    WorkspaceUser = t.WorkspaceUser
                })
                .ToListAsync();
        }

        public async Task<List<Task>> GetMyTasksAsync(string userId, int? workspace)
        {
            IQueryable<Task> query = _dbContext.Tasks.Where(t => t.WorkspaceUser.UserId == userId);

            if (workspace.HasValue)
            {
                var isMember = await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                    wu.UserId == userId && wu.WorkspaceId == workspace.Value
                );

                if (!isMember)
                {
                    throw new UnauthorizedAccessException("User is not a member of the workspace.");
                }

                query = query.Where(t => t.WorkspaceUser.WorkspaceId == workspace.Value);
            }

            return await query
                .Select(t => new Task
                {
                    Id = t.Id,
                    Description = t.Description,
                    StartDateTime = t.StartDateTime,
                    EndDateTime = t.EndDateTime,
                    WorkspaceUser = t.WorkspaceUser
                })
                .ToListAsync();
        }

        public async Task<Task?> GetTaskAsync(int taskId, string userId)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.WorkspaceUser)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return null;
            }

            var isMember = await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                wu.UserId == userId && wu.WorkspaceId == task.WorkspaceUser.WorkspaceId
            );

            if (!isMember)
            {
                throw new UnauthorizedAccessException("User is not a member of the workspace.");
            }

            return task;
        }

        public async Task<Task> CreateTaskAsync(CreateTaskRequest request, int? workspace)
        {
            var workspaceUser = await _dbContext.WorkspaceUsers.FirstOrDefaultAsync(wu =>
                wu.UserId == request.UserId && wu.WorkspaceId == workspace
            );

            if (workspaceUser == null)
            {
                throw new UnauthorizedAccessException("The user is not a member of the workspace.");
            }

            var task = new Task
            {
                Description = request.Description,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                WorkspaceUserId = workspaceUser.Id,
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return task;
        }

        public async Task<Task?> UpdateTaskAsync(int taskId, UpdateTaskRequest request)
        {
            var task = await _dbContext.Tasks.Include(t => t.WorkspaceUser).FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return null;
            }

            task.Description = request.Description;
            task.StartDateTime = request.StartDateTime;
            task.EndDateTime = request.EndDateTime;

            await _dbContext.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return false;
            }

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}