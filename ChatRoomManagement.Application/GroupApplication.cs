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

        public async Task<GroupViewModel> CreateGroup(CreateGroup command)
        {
            var operation = new OperationResult();

            if (!_groupRepository.IsExist(p => p.GroupTitle == command.GroupTitle))
            {
                string picture = _fileUploader.Upload(command.Picture, "Groups\\Pictures");
                var user=_userRepository.GetBy(command.OwnerId);
                var group = new Group(command.GroupTitle, picture, command.OwnerId, command.Token,user);
                _groupRepository.Create(group);
                _groupRepository.SaveChanges();

                var Group=new GroupViewModel
                {
                    Id=group.Id,
                    GroupTitle=group.GroupTitle,
                    Picture=group.Picture, 
                    Token=group.Token.ToString(),
                };

               return Group;
            }



            return new GroupViewModel();

           
        }

        public OperationResult EditGroup(EditGroup command)
        {
            var operation = new OperationResult();

            var group = _groupRepository.GetBy(command.Id);
            if (group is null)
                return operation.Failed("Error");

            if (_groupRepository.IsExist(p => p.Token == command.Token))
                return operation.Failed("Error");

            string picture = _fileUploader.Upload(command.Picture, "/Groups/Pictures/");
            group.Edit(command.GroupTitle, picture);
            _groupRepository.SaveChanges();

            return operation.IsSucssed();
        }

        public Task<EditGroup> GetDetails(long groupId)
        {
            return _groupRepository.GetDetails(groupId);
        }

        public Task<List<GroupViewModel>>GetGroupsBy(Guid userId)
        {
            return _groupRepository.GetGroupsBy(userId);
        }

        public async Task<List<SearchResultViewModel>> Search(string title, string uesrId)
        {
            return await _groupRepository.Search(title, uesrId);
        }
    }
}
