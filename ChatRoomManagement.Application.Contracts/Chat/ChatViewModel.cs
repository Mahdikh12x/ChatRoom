namespace ChatRoomManagement.Application.Contracts.Chat
{
    public class ChatViewModel
    {
        public long Id { get; set; }
        public string Body { get; set; }
        public  string AvatarSender { get; set; }
        public  string UserNameSender { get; set; }
        public string GroupName { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public string CreationDate{ get; set; }
        public string FileAttach { get; set; }
    }
}
