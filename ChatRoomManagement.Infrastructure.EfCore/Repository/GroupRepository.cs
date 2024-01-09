
using _01_framework.Infrastructure;
using ChatRoomManagement.Application.Contracts.Group;
using ChatRoomManagement.Domain.GroupAgg;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomManagement.Infrastructure.EfCore.Repository
{
    public class GroupRepository : RepositoryBase<long, Group>,IGroupRepository
    {
		private readonly ChatRoomContext _context;

		public GroupRepository(ChatRoomContext context) : base(context)
		{
			_context = context;
		}

		public async Task<EditGroup> GetDetails(long groupId)
		{
			var group = _context.Groups.Select(g => new EditGroup
			{
				Id = g.Id,
				GroupTitle = g.GroupTitle,
				Token = g.Token,
			}).FirstOrDefaultAsync(p => p.Id == groupId);


			return await group;


		}

		public async Task<List<GroupViewModel>> GetGroups()
		{
			var query = _context.Groups.Select(p => new GroupViewModel
			{
				Id = p.Id,
				GroupTitle = p.GroupTitle,
				Picture = p.Picture,
				Token = p.Token.ToString(),
				CreationDate = p.CreationDate,

			});

			return await query.OrderByDescending(p => p.CreationDate).ToListAsync();
		}
	}
}
