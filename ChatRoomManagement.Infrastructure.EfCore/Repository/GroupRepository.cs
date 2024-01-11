
using _01_framework.Infrastructure;
using ChatRoomManagement.Application.Contracts.Group;
using ChatRoomManagement.Domain.GroupAgg;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomManagement.Infrastructure.EfCore.Repository
{
    public class GroupRepository : RepositoryBase<long, Group>, IGroupRepository
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

        public async Task<List<GroupViewModel>> GetGroupsBy(Guid userId)
        {

            var query = _context.Users.Include(p => p.Groups)
            .FirstOrDefault(p => p.Id == userId);

            var result = new List<GroupViewModel>();
            foreach (var item in query.Groups)
            {

                result.Add(new GroupViewModel
                {
                    Id = item.Id,
                    GroupTitle = item.GroupTitle,
                    Picture = item.Picture,
                    Token = item.Token.ToString(),
                    CreationDate = item.CreationDate,
                });

               
            }
             return result;
            


        }

        public async Task<List<SearchResultViewModel>> Search(string title, string uesrId)
        {
            var result = new List<SearchResultViewModel>();
            if (string.IsNullOrWhiteSpace(title))
                return result;

            var groups = _context.Groups.Where(p => p.GroupTitle.Contains(title) && !p.IsPrivate && p.OwnerId.ToString()!=uesrId)
                .Select(p => new SearchResultViewModel
                {
                    Title = p.GroupTitle,
                    IsUser = false,
                    Token = p.Token.ToString(),
                    Picture = p.Picture

                }).ToList();

            var users = _context.Users.Where(p => p.UserName.Contains(title) && Guid.Parse(uesrId) != p.Id)
                .Select(p => new SearchResultViewModel
                {
                    Title = p.UserName,
                    IsUser = true,
                    Token = p.Id.ToString(),
                    Picture = p.Picture
                });

            result.AddRange(groups);
            result.AddRange(users);

            return result;
        }
    }
}
