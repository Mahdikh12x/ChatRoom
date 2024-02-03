using _01_framework.Application;
using Microsoft.AspNetCore.Http;

namespace ChatRoomManagement.Application.Contracts.Group
{
    public class CreateGroup
    {
        public string GroupTitle { get; set; }
        public IFormFile Picture { get; set; }
        public long OwnerId { get; set; }
        public Guid Token { get; set; }
        public long ReciverId { get; set; }
    }
}
