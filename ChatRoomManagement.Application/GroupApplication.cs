using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.Group;
using ChatRoomManagement.Application.Contracts.User;
using ChatRoomManagement.Domain.GroupAgg;
using ChatRoomManagement.Domain.UserAgg;

namespace ChatRoomManagement.Application
{
    public class GroupApplication : IGroupApplication
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileUploader _fileUploader;

        public GroupApplication(IGroupRepository groupRepository, IFileUploader fileUploader, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _fileUploader = fileUploader;
            _userRepository = userRepository;
        }

        public async Task<PublicGroupViewModel> CreateGroup(CreateGroup command)
        {
            var operation = new OperationResult();

            if (!_groupRepository.IsExist(p => p.GroupTitle == command.GroupTitle))
            {
                string picture = _fileUploader.Upload(command.Picture, "Groups\\Pictures");
                var user=_userRepository.GetBy(command.OwnerId);
                var group = new Group(command.GroupTitle, picture, command.OwnerId, command.Token,false,0,user);
                _groupRepository.Create(group);
                _groupRepository.SaveChanges();

                var Group=new PublicGroupViewModel
                {
                    Id=group.Id,
                    GroupTitle=group.GroupTitle,
                    Picture=group.Picture, 
                    Token=group.Token.ToString(),
                };

               return Group;
            }



            return new PublicGroupViewModel();

           
        }

        public async Task<PrivateGroupViewModel> CreatePrivateGruop(long ownerId, long reciverId)
        {
            if(ownerId==0 || reciverId==0)
                return new PrivateGroupViewModel();

           var result=await _groupRepository.CreatePrivateGruop(ownerId,reciverId);

            return result;
        }

        public Task<EditGroup> GetDetails(long groupId)
        {
            return _groupRepository.GetDetails(groupId);
        }

        public async Task<PublicGroupViewModel> GetGroupBy(long userId)
        {
            return await _groupRepository.GetGroupBy(userId);   
        }

        public Task<GroupViewModel>GetGroupsBy(long userId)
        {
            return _groupRepository.GetGroupsBy(userId);
        }

        public async Task<PrivateGroupViewModel> IsExistPrivateGroupWith(long userId, long reciverId)
        {
            return await _groupRepository.IsExistPrivateGroupWith(userId, reciverId);   
        }

        public async Task<bool> IsGroupPrivate(long groupId)
        {
            return await _groupRepository.IsGroupPrivate(groupId);
        }

        public async Task<bool> IsUserInGroup(long userId, Guid token)
        {
            return await _groupRepository.IsUserInGroup(userId, token);
        }

        public async Task JoinGroup(long userId, Guid groupToken)
        {
            await _groupRepository.JoinGroup(userId, groupToken);
        }

        public async Task JoinPrivateGroup(JoinPrivateGroup command)
        {
            if(command == null) throw new ArgumentNullException();
           await _groupRepository.JoinPrivateGroup(command); 
        }

        public async Task<List<SearchResultViewModel>> Search(string title, long uesrId)
        {
            return await _groupRepository.Search(title, uesrId);
        }

        public async Task<List<string>> usersIdInGroup(long groupId)
        {
           return await _groupRepository.usersIdInGroup(groupId);
        }
    }
}
