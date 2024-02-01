
using _01_framework.Domain;
using ChatRoomManagement.Domain.ChatAgg;
using ChatRoomManagement.Domain.UserAgg;

namespace ChatRoomManagement.Domain.GroupAgg
{
	public class Group:BaseEntity<long>
	{
        public string GroupTitle { get;private set; }
        public string Picture { get;private set; }
        public long OwnerId { get;private set; }
        public Guid Token { get; private set; }
        public bool IsPrivate { get;private set; }
        public long ReciverId { get; private set; }

        public List<Chat> Chats{ get; private set; }
        public List<User> Users { get;private set; }


		protected Group(){}

		public Group(string groupTitle, string picture,long ownerId,Guid token,bool isPrivate,long reciverId,User user)
		{
			GroupTitle = groupTitle;
			Picture = picture;
			OwnerId=ownerId;
			IsPrivate=isPrivate;
			Token=token;
			ReciverId=reciverId;
			Users=new List<User>(){user};
		}

		public void Edit(string groupTitle, string picture)
		{
			GroupTitle = groupTitle;
			Picture = picture;
		}


		

	}
}
