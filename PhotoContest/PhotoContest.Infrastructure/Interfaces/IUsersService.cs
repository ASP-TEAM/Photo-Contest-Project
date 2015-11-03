using System.Linq;
using PhotoContest.Models;

namespace PhotoContest.Infrastructure.Interfaces
{
    public interface IUsersService
    {
        IQueryable<Invitation> GetNotifications(string userId);
    }
}