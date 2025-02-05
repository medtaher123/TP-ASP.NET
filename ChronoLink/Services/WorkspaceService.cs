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

        public async Task<bool> IsMemberWorkspaceAsync(string userId, int workspaceId)
        {
            return await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                wu.UserId == userId && wu.WorkspaceId == workspaceId
            );
        }

        public async Task<bool> IsAdminWorkspaceAsync(string userId, int workspaceId)
        {
            return await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                wu.UserId == userId && wu.WorkspaceId == workspaceId && wu.IsAdmin == true
            );
        }

        public async Task<Workspace> CreateWorkspaceAsync(Workspace workspace)
        {
            _dbContext.Workspaces.Add(workspace);
            await _dbContext.SaveChangesAsync();
            return workspace;
        }

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
                        m.IsAdmin,
                    }),
                })
                .FirstOrDefaultAsync();
        }
    }
}