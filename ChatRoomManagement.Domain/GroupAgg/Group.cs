
using _01_framework.Domain;
using ChatRoomManagement.Domain.UserAgg;

namespace ChatRoomManagement.Domain.GroupAgg
{
	public class Group:BaseEntity<long>
	{
        public string GroupTitle { get;private set; }
        public string Picture { get;private set; }
        public Guid OwnerId { get;private set; }
        public Guid Token { get; private set; }
        public bool IsPrivate { get;private set; }



        public List<User> Users { get;private set; }

		public Group(string groupTitle, string picture,Guid ownerId,Guid token)
		{
			GroupTitle = groupTitle;
			Picture = picture;
			OwnerId=ownerId;
			IsPrivate=false;
			Token=token;
			Users=new List<User>();
		}

		public void Edit(string groupTitle, string picture)
		{
			GroupTitle = groupTitle;
			Picture = picture;
		}


	}
}
