

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

        public async Task CreateChat(CreateChat command)
        {
            if(command  != null)
            {
                var chat=new Chat(command.Body,command.UserId,command.GroupId);
                _chatRepository.Create(chat);
                _chatRepository.SaveChanges();
                await Task.CompletedTask;
            }
                
        }

        public Task<List<ChatViewModel>> GetChats(long groupId)
        {
            return _chatRepository.GetChats(groupId);
        }
    }
}
