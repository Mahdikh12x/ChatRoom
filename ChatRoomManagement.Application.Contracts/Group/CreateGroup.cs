using _01_framework.Application;
using Microsoft.AspNetCore.Http;

namespace ChatRoomManagement.Application.Contracts.Group
{
    public class CreateGroup
	{
		public string GroupTitle { get; set; }
		public IFormFile Picture { get; set; }
		public Guid OwnerId { get; set; }
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
		OperationResult CreateGroup(CreateGroup command);
		OperationResult EditGroup(EditGroup command);
		Task<List<GroupViewModel>> GetGroups();
		Task<EditGroup> GetDetails(long groupId);
	}
}
