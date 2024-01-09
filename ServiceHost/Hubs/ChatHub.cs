using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.Group;
using Microsoft.AspNetCore.SignalR;

namespace ServiceHost.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IGroupApplication _groupApplication;
        private readonly IAuthHelper _authHelper;

        public ChatHub(IGroupApplication groupApplication, IAuthHelper authHelper)
        {
            _groupApplication = groupApplication;
            _authHelper = authHelper;
        }

        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("SendClientMessage");
            return base.OnConnectedAsync();
        }

        public async Task CreateGroup(string groupName,IFormFile imageFile)
        {
            


        }
    }
}
