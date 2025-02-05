using System.Security.Claims;
using ChronoLink.Controllers;
using ChronoLink.Data;
using ChronoLink.Dtos;
using ChronoLink.Models;
using ChronoLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = ChronoLink.Models.Task;

namespace ChronoLink.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllTasksAsync(string userId, int? workspaceId);
        Task<List<TaskDto>> GetMyTasksAsync(string userId, int? workspaceId);
        Task<TaskDto?> GetTaskAsync(int taskId, string userId);
        Task<TaskDto> CreateTaskAsync(CreateTaskRequest request, int workspaceId);
        Task<Task?> UpdateTaskAsync(int taskId, UpdateTaskRequest request);
        Task<bool> DeleteTaskAsync(int taskId);
    }

    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly WorkspaceService _workspaceService;

        public TaskService(AppDbContext dbContext, WorkspaceService workspaceService)
        {
            _dbContext = dbContext;
            _workspaceService = workspaceService;
        }

        public async Task<List<TaskDto>> GetAllTasksAsync(string userId, int? workspaceId)
        {
            IQueryable<Task> query = _dbContext.Tasks;

            if (!workspaceId.HasValue)
            {
                query = query.Where(t => t.WorkspaceUser.UserId == userId);

            }
            return await query
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    StartDateTime = t.StartDateTime,
                    EndDateTime = t.EndDateTime,
                    WorkspaceId = t.WorkspaceUser.WorkspaceId,
                    AssignedUserId = t.WorkspaceUser.UserId,
                    AssignedUserName = t.WorkspaceUser.User.UserName,
                })
                .ToListAsync();
        }

        public async Task<List<TaskDto>> GetMyTasksAsync(string userId, int? workspaceId)
        {
            IQueryable<Task> query = _dbContext.Tasks;

            if (workspaceId.HasValue)
            {
                query = query.Where(t => t.WorkspaceUser.UserId == userId);
            }

            return await query
                .Where(t => t.WorkspaceUser.UserId == userId)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    StartDateTime = t.StartDateTime,
                    EndDateTime = t.EndDateTime,
                    WorkspaceId = t.WorkspaceUser.WorkspaceId,
                    AssignedUserId = t.WorkspaceUser.UserId,
                    AssignedUserName = t.WorkspaceUser.User.UserName,
                })
                .ToListAsync();
        }

        public async Task<TaskDto?> GetTaskAsync(int taskId, string userId)
        {
            var task = await _dbContext
                .Tasks.Include(t => t.WorkspaceUser)
                .ThenInclude(wu => wu.User)
                .FirstOrDefaultAsync(t => t.Id == taskId);
            if (task.WorkspaceUser.UserId != userId)
            {
                throw new UnauthorizedAccessException("User is not a member of the workspace.");
            }
            if (task == null)
            {
                return null;
            }

            return new TaskDto
            {
                Id = task.Id,
                Description = task.Description,
                StartDateTime = task.StartDateTime,
                EndDateTime = task.EndDateTime,
                WorkspaceId = task.WorkspaceUser.WorkspaceId,
                AssignedUserId = task.WorkspaceUser.UserId,
                AssignedUserName = task.WorkspaceUser.User.UserName,
            };
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskRequest request, int workspaceId)
        {
            var WorkspaceUser = await _dbContext
                .WorkspaceUsers.Include(wu => wu.User)
                .Include(wu => wu.Workspace)
                .FirstOrDefaultAsync(wu =>
                    wu.UserId == request.UserId && wu.WorkspaceId == workspaceId
                );

            if (WorkspaceUser == null)
            {
                throw new UnauthorizedAccessException("User is not a member of the workspace.");
            }
            var task = new Task
            {
                Description = request.Description,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                WorkspaceUserId = WorkspaceUser.Id,
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return new TaskDto
            {
                Id = task.Id,
                Description = task.Description,
                StartDateTime = task.StartDateTime,
                EndDateTime = task.EndDateTime,
                WorkspaceId = WorkspaceUser.WorkspaceId,
                AssignedUserId = WorkspaceUser.UserId,
                AssignedUserName = WorkspaceUser.User.UserName,
            };
        }

        public async Task<Task?> UpdateTaskAsync(int taskId, UpdateTaskRequest request)
        {
            var task = await _dbContext
                .Tasks.Include(t => t.WorkspaceUser)
                .FirstOrDefaultAsync(t => t.Id == taskId);

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
        private bool isWorkspaceMember(int workspaceId, string userId)
        {
            return _dbContext.WorkspaceUsers.Any(wu => wu.WorkspaceId == workspaceId && wu.UserId == userId);
        }
    }
}
