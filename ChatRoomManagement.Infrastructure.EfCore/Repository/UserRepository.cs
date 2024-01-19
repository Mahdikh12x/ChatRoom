
using _01_framework.Infrastructure;
using ChatRoomManagement.Domain.UserAgg;

namespace ChatRoomManagement.Infrastructure.EfCore.Repository
{
    public class UserRepository : RepositoryBase<long, User>, IUserRepository
    {
        private readonly ChatRoomContext _context;
        public UserRepository(ChatRoomContext context) : base(context)
        {
            _context=context;
        }

		public User GetByEmail(string email)
		{
			return _context.Users.FirstOrDefault(p=>p.Email==email);

		}
	}
}
