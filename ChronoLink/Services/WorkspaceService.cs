using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChronoLink.Models;
using ChronoLink.Data;

namespace ChronoLink.Services
{
    public class WorkspaceService
    {
        private readonly AppDbContext _dbContext;

        public WorkspaceService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Workspace> GetByIdAsync(int id)
        {
            var workspace = await _dbContext.Workspaces.FindAsync(id);
            if (workspace == null)
            {
                throw new InvalidOperationException($"Workspace with id {id} not found.");
            }
            return workspace;
        }

        public async Task<bool> IsUserInWorkspaceAsync(string userId, int workspaceId)
        {
            return await _dbContext.WorkspaceUsers.AnyAsync(wu =>
            wu.UserId == userId && wu.WorkspaceId == workspaceId);
        }
        public async System.Threading.Tasks.Task RequireUserIsMemberAsync(string userId, int workspaceId)
        {
            var isMember = await IsUserInWorkspaceAsync(userId, workspaceId);
            if (!isMember)
            {
                throw new UnauthorizedAccessException("User is not a member of the workspace.");
            }
        }
        public async Task<bool> IsUserAdminInWorkspaceAsync(string userId, int workspaceId)
        {
            return true;
            return await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                wu.UserId == userId
                && wu.WorkspaceId == workspaceId
                && wu.Role == WorkspaceRole.Admin
            );
        }
        public async System.Threading.Tasks.Task RequireUserIsAdminAsync(string userId, int workspaceId)
        {
            var isAdmin = await IsUserAdminInWorkspaceAsync(userId, workspaceId);
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("User is not an admin of the workspace.");
            }
        }

        public async Task<Workspace> CreateWorkspaceAsync(Workspace workspace)
        {
            _dbContext.Workspaces.Add(workspace);
            await _dbContext.SaveChangesAsync();
            return workspace;
        }

        /* public async Task<Workspace> UpdateWorkspaceAsync(Workspace workspace)
         {
             var existingWorkspace = await _dbContext.Workspaces.FindAsync(workspace.Id);
             if (existingWorkspace == null)
             {
                 throw new InvalidOperationException($"Works²pace with id {workspace.Id} not found.");
             }

             existingWorkspace.Name = workspace.Name;
             existingWorkspace.Description = workspace.Description;
             // Update other properties as needed

             _dbContext.Workspaces.Update(existingWorkspace);
             await _dbContext.SaveChangesAsync();
             return existingWorkspace;
         }
     */
        public async Task<int> DeleteWorkspaceAsync(Guid id)
        {
            var workspace = await _dbContext.Workspaces.FindAsync(id);
            if (workspace == null)
            {
                throw new InvalidOperationException($"Workspace with id {id} not found.");
            }

            _dbContext.Workspaces.Remove(workspace);
            return await _dbContext.SaveChangesAsync();
        }

        internal async Task<object> GetWorkspaceAsync(int workspaceId)
        {
            return await _dbContext
                .Workspaces.Where(w => w.Id == workspaceId)
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    Members = w.WorkspaceUsers.Select(m => new
                    {
                        m.Id,
                        m.User.Name,
                        m.Role,
                    }),
                })
                .FirstOrDefaultAsync();
        }
    }
}