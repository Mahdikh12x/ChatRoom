
using Microsoft.AspNetCore.Http;

namespace ChatRoomManagement.Application.Contracts.Chat
{
    public class CreateChat
    {
        public string Body { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public IFormFile?  File { get; set; }
        public string?  FilePath { get; set; }

    }
}
