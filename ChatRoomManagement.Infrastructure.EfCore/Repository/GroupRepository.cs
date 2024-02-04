
using _01_framework.Infrastructure;
using ChatRoomManagement.Application.Contracts.Group;
using ChatRoomManagement.Domain.ChatAgg;
using ChatRoomManagement.Domain.GroupAgg;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;

namespace ChatRoomManagement.Infrastructure.EfCore.Repository
{
    public class GroupRepository : RepositoryBase<long, Group>, IGroupRepository
    {
        private readonly ChatRoomContext _context;

        public GroupRepository(ChatRoomContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PrivateGroupViewModel> CreatePrivateGruop(long ownerId, long reciverId)
        {
            var user = _context.Users.FirstOrDefault(p => p.Id == ownerId);
            var group = new Group("Private Group", "", ownerId, Guid.NewGuid(), true, reciverId, user);
            _context.Groups.Add(group);
            _context.SaveChanges();

            var owner = _context.Users.FirstOrDefault(p => p.Id == ownerId);
            var reciver = _context.Users.FirstOrDefault(p => p.Id == reciverId);

            var privateGroupViewModel = new PrivateGroupViewModel
            {
                Id = group.Id,
                OwnerId = owner.Id,
                OwnerPicture = owner.Picture,
                OwnerUserName = owner.UserName,
                ReciverId = reciver.Id,
                ReciverPicture = reciver.Picture,
                ReciverUserName = reciver.UserName
            };

            return privateGroupViewModel;
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

        public async Task<PublicGroupViewModel> GetGroupBy(long userId)
        {
            var query = _context.Groups.Select(p => new PublicGroupViewModel
            {
                Id = p.Id,
                GroupTitle = p.GroupTitle,
                Picture = p.Picture,
                Token = p.Token.ToString(),
                CreationDate = p.CreationDate,

            }).FirstOrDefaultAsync(p => p.Id == userId);

            return await query;
        }


        public async Task<List<PublicGroupViewModel>> GetGroups()
        {
            var query = _context.Groups.Select(p => new PublicGroupViewModel
            {
                Id = p.Id,
                GroupTitle = p.GroupTitle,
                Picture = p.Picture,
                Token = p.Token.ToString(),
                CreationDate = p.CreationDate,

            });

            return await query.OrderByDescending(p => p.CreationDate).ToListAsync();
        }

        public async Task<GroupViewModel> GetGroupsBy(long userId)
        {
            var groups = new GroupViewModel()
            {
                PublicGroup = new List<PublicGroupViewModel>(),
                PrivateGroup = new List<PrivateGroupViewModel>()

            };

            var AllGroups = _context.Users.Include(p => p.Groups)
           .FirstOrDefault(p => p.Id == userId);


            foreach (var item in AllGroups.Groups.Where(p => !p.IsPrivate))
            {

                groups.PublicGroup.Add(new PublicGroupViewModel
                {
                    Id = item.Id,
                    GroupTitle = item.GroupTitle,
                    Picture = item.Picture,
                    Token = item.Token.ToString(),
                    CreationDate = item.CreationDate,
                });

            }
            foreach (var item in AllGroups.Groups.Where(p => p.IsPrivate))
            {
                var owner = _context.Users.FirstOrDefault(p => p.Id == item.OwnerId);
                var reciver = _context.Users.FirstOrDefault(p => p.Id == item.ReciverId);
                groups.PrivateGroup.Add(new PrivateGroupViewModel
                {
                    Id = item.Id,
                    OwnerId = owner.Id,
                    OwnerPicture = owner.Picture,
                    OwnerUserName = owner.UserName,
                    OwnerIsOnline=owner.IsOnline,
                    ReciverIsOnline=reciver.IsOnline,
                    ReciverId = reciver.Id,
                    ReciverPicture = reciver.Picture,
                    ReciverUserName = reciver.UserName
                });
            }
            return groups;
        }



        public async Task<PrivateGroupViewModel> IsExistPrivateGroupWith(long userId, long reciverId)
        {
            var result = _context.Groups.FirstOrDefault(p => (p.OwnerId == userId && p.ReciverId == reciverId) || (p.OwnerId == reciverId && p.ReciverId == userId));

            if (result is null)
                return new PrivateGroupViewModel();

            var owner = _context.Users.FirstOrDefault(p => p.Id == result.OwnerId);
            var reciver = _context.Users.FirstOrDefault(p => p.Id == result.ReciverId);
            return new PrivateGroupViewModel
            {
                Id = result.Id,
                OwnerId = owner.Id,
                OwnerPicture = owner.Picture,
                OwnerUserName = owner.UserName,
                OwnerIsOnline = owner.IsOnline,
                ReciverIsOnline= reciver.IsOnline,
                ReciverId = reciver.Id,
                ReciverPicture = reciver.Picture,
                ReciverUserName = reciver.UserName,
                OwnerLastSeenDate=owner.LastSeen.ToShortDateString() + "  " + owner.LastSeen.ToShortTimeString(),
                ReciverLastSeenDate=reciver.LastSeen.ToShortDateString() + "  " + reciver.LastSeen.ToShortTimeString(),
            };
        }

        public async Task<bool> IsGroupPrivate(long groupId)
        {
            var group = _context.Groups.FirstOrDefault(p => p.Id == groupId).IsPrivate;
            return group;
        }

        public async Task<bool> IsUserInGroup(long userId, Guid token)
        {
            var user = _context.Users.Include(p => p.Groups).FirstOrDefault(p => p.Id == userId);

            return user.Groups.Any(p => p.Token == token);
        }

        public async Task JoinGroup(long userId, Guid groupToken)
        {
            if (groupToken != null || userId != null)
            {
                var user = _context.Users.Include(p => p.Groups).FirstOrDefault(p => p.Id == userId);
                var group = _context.Groups.FirstOrDefault(p => p.Token == groupToken);
                user.Groups.Add(group);
                _context.SaveChanges();
                await Task.CompletedTask;
            }


        }

        public async Task JoinPrivateGroup(JoinPrivateGroup command)
        {
            var users = _context.Users.Where(p => p.Id == command.OwnerId || p.Id == command.ReciverId);
            var group = _context.Groups.FirstOrDefault(p => p.Id == command.GroupId);
            foreach (var item in users)
            {
                item.Groups.Add(group);

            }
            _context.SaveChanges();
        }

        public async Task<List<SearchResultViewModel>> Search(string title, long uesrId)
        {
            var result = new List<SearchResultViewModel>();
            if (string.IsNullOrWhiteSpace(title))
                return result;

            var groups = _context.Groups.Where(p => p.GroupTitle.Contains(title) && !p.IsPrivate && p.OwnerId != uesrId)
                .Select(p => new SearchResultViewModel
                {
                    Id = p.Id,
                    Title = p.GroupTitle,
                    IsUser = false,
                    Token = p.Token.ToString(),
                    Picture = p.Picture

                }).ToList();

            var users = _context.Users.Where(p => p.UserName.Contains(title) && uesrId != p.Id)
                 .Select(p => new SearchResultViewModel
                 {
                     Id = p.Id,
                     Title = p.UserName,
                     IsUser = true,
                     Token = p.Id.ToString(),
                     Picture = p.Picture
                 });

            result.AddRange(groups);
            result.AddRange(users);

            return result;
        }

        public async Task<List<string>> usersIdInGroup(long groupId)
        {
            var query = _context.Groups.Include(p => p.Users).FirstOrDefault(p => p.Id == groupId);

            var users = new List<string>();

            foreach (var item in query.Users)
            {
                var user = item.Id.ToString();
                users.Add(user);
            }

            return users;
        }


    }
}
