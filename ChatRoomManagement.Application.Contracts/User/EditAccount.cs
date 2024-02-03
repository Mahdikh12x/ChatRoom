using Microsoft.AspNetCore.Http;

namespace ChatRoomManagement.Application.Contracts.User
{
    public class EditAccount 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
