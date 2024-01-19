using _01_framework.Domain;
using ChatRoomManagement.Application.Contracts.Group;

namespace ChatRoomManagement.Domain.GroupAgg
{
    public interface IGroupRepository : IRepository<long, Group>
    {
        Task<List<GroupViewModel>> GetGroups();
        Task<EditGroup> GetDetails(long groupId);
        Task<List<SearchResultViewModel>> Search(string title, long uesrId);
        Task<List<GroupViewModel>> GetGroupsBy(long userId);
        Task<bool> IsUserInGroup(long userId, Guid token);
        Task JoinGroup(long userId, Guid groupToken);
        Task<GroupViewModel> GetGroupBy(long userId);


    }


}
