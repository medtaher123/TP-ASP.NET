using ChronoLink.Models;

namespace ChronoLink.Repositories
{
    public interface ICalendarRepository : IRepository<Calendar>
    {
        Calendar GetCalendar(string userId, int? workspaceId);
    }
}
