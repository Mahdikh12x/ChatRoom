
using _01_framework.Infrastructure;
using ChatRoomManagement.Application.Contracts.User;
using ChatRoomManagement.Domain.UserAgg;
using Microsoft.EntityFrameworkCore;

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

        public async Task<UserViewModel> GetUserBy(long id)
        {
            var user=await _context.Users.Select(p=> new UserViewModel
            {
                Name=p.Name,
                Email=p.Email,
                Picture=p.Picture,
                Id=p.Id,
            }).FirstOrDefaultAsync(p=>p.Id==id);
            
            return user;
        }
    }
}
