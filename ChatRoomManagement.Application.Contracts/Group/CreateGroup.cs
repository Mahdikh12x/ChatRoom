using _01_framework.Application;
using Microsoft.AspNetCore.Http;

namespace ChatRoomManagement.Application.Contracts.Group
{
    public class CreateGroup
    {
        public string GroupTitle { get; set; }
        public IFormFile Picture { get; set; }
        public long OwnerId { get; set; }
        public Guid Token { get; set; }
        public long ReciverId { get; set; }
    }

    public class EditGroup : CreateGroup
    {
        public long Id { get; set; }

    }

    public class GroupViewModel
    {

        public List<PublicGroupViewModel> PublicGroup { get; set; }
        public List<PrivateGroupViewModel> PrivateGroup { get; set; }

    }

    public class PublicGroupViewModel
    {
        public long Id { get; set; }
        public string GroupTitle { get; set; }
        public string Token { get; set; }
        public string Picture { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class JoinPrivateGroup
    {
        public long OwnerId { get; set; }
        public long ReciverId { get; set; }
        public long GroupId { get; set; }
    }
    public class PrivateGroupViewModel
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public long ReciverId { get; set; }
        public string OwnerUserName { get; set; }
        public string ReciverUserName { get; set; }
        public string OwnerPicture { get; set; }
        public string ReciverPicture { get; set; }



    }

    public interface IGroupApplication
    {
        Task<PublicGroupViewModel> CreateGroup(CreateGroup command);
        Task<PrivateGroupViewModel> IsExistPrivateGroupWith(long userId, long reciverId);
        Task<PrivateGroupViewModel> CreatePrivateGruop(long ownerId, long reciverId);
        Task<GroupViewModel> GetGroupsBy(long userId);
        Task<PublicGroupViewModel> GetGroupBy(long userId);
        Task<EditGroup> GetDetails(long groupId);
        Task<List<SearchResultViewModel>> Search(string title, long uesrId);
        Task<bool> IsUserInGroup(long userId, Guid token);
        Task JoinGroup(long userId, Guid groupToken);
        Task JoinPrivateGroup(JoinPrivateGroup command);
        Task<List<string>> usersIdInGroup(long groupId);

    }


    public class SearchResultViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Token { get; set; }
        public string Picture { get; set; }
        public bool IsUser { get; set; }
    }
}
