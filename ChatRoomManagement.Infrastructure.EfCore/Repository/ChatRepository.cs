

using _01_framework.Infrastructure;
using ChatRoomManagement.Application.Contracts.Chat;
using ChatRoomManagement.Domain.ChatAgg;
using ChatRoomManagement.Domain.UserAgg;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomManagement.Infrastructure.EfCore.Repository
{
    public class ChatRepository : RepositoryBase<long, Chat>, IChatRepository
    {
        private readonly ChatRoomContext _context;

        public ChatRepository(ChatRoomContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ChatViewModel> CreateChat(CreateChat command)
        {
            if (command == null)
                return new ChatViewModel();

            var chat = new Chat(command.Body, command.UserId, command.GroupId);
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();

            var user=_context.Users.FirstOrDefault(p=>p.Id==command.UserId);

            var chatViewModel = new ChatViewModel()
            {
                Id = chat.Id,
                Body = chat.Body,
                UserId = chat.UserId,
                GroupId = chat.GroupId,
                CreationDate = chat.CreationDate.ToShortDateString() + "  " + chat.CreationDate.ToShortTimeString(),
                UserNameSender = user.Name,
                AvatarSender = user.Picture,


            };

            return chatViewModel;

        }

        public async Task<List<ChatViewModel>> GetChats(long groupId, long userId)
        {

            var chatsViewModel = new List<ChatViewModel>();

            var chats = _context.Chats.Where(p => p.GroupId == groupId).Include(p=>p.User).OrderBy(p => p.CreationDate).ToList();

            foreach (var item in chats)
            {
                var chat = new ChatViewModel
                {
                    Id = item.Id,
                    Body = item.Body,
                    AvatarSender = item.User.Picture,
                    GroupId = item.GroupId,
                    UserId = item.UserId,
                    CreationDate = item.CreationDate.ToShortDateString() + "  " + item.CreationDate.ToShortTimeString(),
                    UserNameSender =item.User.Name
                };

                chatsViewModel.Add(chat);
            }



            return chatsViewModel;


        }
    }
}
