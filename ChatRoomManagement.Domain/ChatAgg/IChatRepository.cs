

using _01_framework.Domain;
using ChatRoomManagement.Application.Contracts.Chat;

namespace ChatRoomManagement.Domain.ChatAgg
{
    public interface IChatRepository : IRepository<long, Chat>
    {
        Task<List<ChatViewModel>> GetChats(long groupId, long userId);
        Task<ChatViewModel> CreateChat(CreateChat command);

    }
}
