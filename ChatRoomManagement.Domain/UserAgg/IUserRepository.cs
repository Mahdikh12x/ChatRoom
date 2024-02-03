using _01_framework.Domain;
using ChatRoomManagement.Application.Contracts.User;

namespace ChatRoomManagement.Domain.UserAgg
{
    public interface IUserRepository : IRepository<long, User>
    {
        User GetByEmail(string email);
        Task<UserViewModel> GetUserBy(long id);

    }
}
