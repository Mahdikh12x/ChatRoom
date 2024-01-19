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
    }

	public class EditGroup : CreateGroup
	{
		public long Id { get; set; }

	}

	public class GroupViewModel
	{
        public long Id { get; set; }
        public string GroupTitle { get; set; }
        public string Token { get; set; }
        public string Picture { get; set; }
        public DateTime CreationDate { get; set; }

    }

	public interface IGroupApplication
	{
		Task<GroupViewModel> CreateGroup(CreateGroup command);
		Task<List<GroupViewModel>> GetGroupsBy(long userId);
		Task<GroupViewModel> GetGroupBy(long userId);
		Task<EditGroup> GetDetails(long groupId);
		Task<List<SearchResultViewModel>> Search(string title,long uesrId);
		Task<bool> IsUserInGroup(long userId,Guid token);
		Task JoinGroup(long userId,Guid groupToken);
	}


	public class SearchResultViewModel
	{
        public long Id{ get; set; }
        public string Title { get; set; }
        public string Token{ get; set; }
        public string Picture { get; set; }
        public bool IsUser { get; set; }
    }
}
