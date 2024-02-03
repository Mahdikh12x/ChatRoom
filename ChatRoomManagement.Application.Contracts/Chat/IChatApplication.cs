namespace ChatRoomManagement.Application.Contracts.Chat
{
    public interface IChatApplication
    {
        Task<ChatViewModel> CreateChat(CreateChat command);
        Task<List<ChatViewModel>> GetChats(long groupId,long userId);
        
    }
}
