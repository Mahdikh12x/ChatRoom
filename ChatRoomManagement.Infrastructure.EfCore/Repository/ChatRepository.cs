

using _01_framework.Infrastructure;
using ChatRoomManagement.Application.Contracts.Chat;
using ChatRoomManagement.Domain.ChatAgg;
using Microsoft.EntityFrameworkCore;

namespace ChatRoomManagement.Infrastructure.EfCore.Repository
{
    public class ChatRepository : RepositoryBase<long, Chat>, IChatRepository
    {
        private readonly ChatRoomContext _context;

        public ChatRepository(ChatRoomContext context):base(context) 
        {
            _context = context;
        }

        public async Task<List<ChatViewModel>> GetChats(long groupId)
        {
           var query=_context.Chats.Include(p=>p.Group).Include(p=>p.User).Where(p=>p.GroupId == groupId)
                .Select(p=>new ChatViewModel
                {
                    Id = p.Id,
                    Body = p.Body,
                    AvatarSender=p.User.Picture,
                    GroupToken=p.Group.Token,
                    GroupId=p.GroupId,
                    UserId=p.UserId,
                    CreationDate=p.CreationDate.ToLongTimeString(),
                });

             return query.OrderBy(p=>p.CreationDate).ToList();

        }
    }
}
