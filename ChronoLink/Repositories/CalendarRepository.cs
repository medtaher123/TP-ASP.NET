using System.Linq;
using ChronoLink.Data;
using ChronoLink.Models;
using Microsoft.EntityFrameworkCore;

namespace ChronoLink.Repositories
{
    public class CalendarRepository : Repository<Calendar>, ICalendarRepository
    {
        private readonly AppDbContext _context;

        public CalendarRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public Calendar GetCalendar(string userId, int? workspaceId)
        {
            return _context
                .Calendars.Include(c => c.Events)
                .FirstOrDefault(c =>
                    workspaceId == null ? c.UserId == userId : c.WorkspaceId == workspaceId
                );
        }
    }
}
