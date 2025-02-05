using System.Security.Claims;
using ChronoLink.Authorization;
using ChronoLink.Data;
using ChronoLink.Dtos;

using ChronoLink.Models;
using ChronoLink.Services;
using ChronoLink.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            // get users as userdto
            var users = await _dbContext.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
                .ToListAsync();
            return Ok(users);
        }
    }
}
