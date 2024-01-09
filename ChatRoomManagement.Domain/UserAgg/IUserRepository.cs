using _01_framework.Domain;

namespace ChatRoomManagement.Domain.UserAgg
{
    public interface IUserRepository:IRepository<Guid,User>
    {
        User GetByEmail(string email);
    }
}
