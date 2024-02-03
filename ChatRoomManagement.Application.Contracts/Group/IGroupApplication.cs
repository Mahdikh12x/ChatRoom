namespace ChatRoomManagement.Application.Contracts.Group
{
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
        Task<bool> IsGroupPrivate(long groupId);

    }
}
