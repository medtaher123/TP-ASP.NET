using System;
using System.Collections.Generic;
using System.Linq;
using ChronoLink.Models;
using ChronoLink.Data;
using Microsoft.EntityFrameworkCore;
using ChronoLink.Dtos;


namespace ChronoLink.Repositories
{
    public class TaskRepository : BaseRepository<Models.Task, TaskDto>
    {
        public TaskRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        // Implementing the abstract method to project Task to TaskDto
        protected override TaskDto ProjectToDto(Models.Task entity)
        {
            return new TaskDto
            {
                Id = entity.Id,
                Description = entity.Description,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
                WorkspaceId = entity.WorkspaceUser.WorkspaceId,
                AssignedUserId = entity.WorkspaceUser.UserId,
                AssignedUserName = entity.WorkspaceUser.User.Name
            };
        }

        protected override IQueryable<TaskDto> ProjectToDto(IQueryable<Models.Task> query)
        {
            return
         query.Select(entity => new TaskDto
         {
             Id = entity.Id,
             Description = entity.Description,
             StartDateTime = entity.StartDateTime,
             EndDateTime = entity.EndDateTime,
             WorkspaceId = entity.WorkspaceUser.WorkspaceId,
             AssignedUserId = entity.WorkspaceUser.UserId,
             AssignedUserName = entity.WorkspaceUser.User.Name
         });
        }
    }


}