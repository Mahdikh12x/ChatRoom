
using _01_framework.Domain;
using ChatRoomManagement.Domain.GroupAgg;
using ChatRoomManagement.Domain.UserAgg;

namespace ChatRoomManagement.Domain.ChatAgg
{
    public class Chat:BaseEntity<long>
    {
        public string Body { get;private set; }
        public long UserId { get; private set; }
        public string File { get; private set; }
        public long GroupId { get; private set; }

        public User User { get; private set; }
        public Group Group{ get; private set; }

        public Chat(string body, long userId, long groupId,string file)
        {
            Body = body;
            UserId = userId;
            GroupId = groupId;
            File = file;
        }
    }
}
