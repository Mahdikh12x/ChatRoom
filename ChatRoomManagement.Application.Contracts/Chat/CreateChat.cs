
namespace ChatRoomManagement.Application.Contracts.Chat
{
    public class CreateChat
    {
        public string Body { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }

    }


    public class ChatViewModel
    {
        public long Id { get; set; }
        public string Body { get; set; }
        public  string AvatarSender { get; set; }
        public  string UserNameSender { get; set; }
        public Guid GroupToken { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public string CreationDate{ get; set; }
    }

    public interface IChatApplication
    {
        Task<ChatViewModel> CreateChat(CreateChat command);
        Task<List<ChatViewModel>> GetChats(long groupId,long userId);
    }
}
