

using ChatRoomManagement.Application.Contracts.Chat;
using ChatRoomManagement.Domain.ChatAgg;

namespace ChatRoomManagement.Application
{
    public class ChatApplication : IChatApplication
    {
        private readonly IChatRepository _chatRepository;

        public ChatApplication(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ChatViewModel> CreateChat(CreateChat command)
        {
            return await _chatRepository.CreateChat(command); 
                
        }

        public async Task<List<ChatViewModel>> GetChats(long groupId, long userId)
        {
            return await _chatRepository.GetChats(groupId,userId);
        }
    }
}
