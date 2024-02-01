

using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.Chat;
using ChatRoomManagement.Domain.ChatAgg;

namespace ChatRoomManagement.Application
{
    public class ChatApplication : IChatApplication
    {
        private readonly IChatRepository _chatRepository;
        private readonly IFileUploader _fileUploader;
        public ChatApplication(IChatRepository chatRepository, IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
            _chatRepository = chatRepository;
        }

        public async Task<ChatViewModel> CreateChat(CreateChat command)
        {
            if(string.IsNullOrWhiteSpace(command.Body))
                return new ChatViewModel();

            if (command.File != null)
            {
                var file=_fileUploader.Upload(command.File,"chatFiles");
                command.FilePath = file;
            }
            else
            {
                command.FilePath="No File";
            }
            return await _chatRepository.CreateChat(command);

        }

        public async Task<List<ChatViewModel>> GetChats(long groupId, long userId)
        {
            return await _chatRepository.GetChats(groupId, userId);
        }
    }
}
