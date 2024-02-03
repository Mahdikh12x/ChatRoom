using _01_framework.Domain;
using ChatRoomManagement.Application.Contracts.Group;

namespace ChatRoomManagement.Domain.GroupAgg
{
    public interface IGroupRepository : IRepository<long, Group>
    {
        Task<List<PublicGroupViewModel>> GetGroups();
        Task<PublicGroupViewModel> GetGroupBy(long userId);
        Task<GroupViewModel> GetGroupsBy(long userId);
        Task<PrivateGroupViewModel> CreatePrivateGruop(long ownerId, long reciverId);
        Task<PrivateGroupViewModel> IsExistPrivateGroupWith(long userId, long reciverId);
        Task JoinPrivateGroup(JoinPrivateGroup command);
        Task<EditGroup> GetDetails(long groupId);
        Task<List<SearchResultViewModel>> Search(string title, long uesrId);
        Task<bool> IsUserInGroup(long userId, Guid token);
        Task JoinGroup(long userId, Guid groupToken);
        Task<List<string>> usersIdInGroup(long groupId);
        Task<bool> IsGroupPrivate(long groupId);



    }


}
