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

        public async Task JoinPublicGroup(string groupId, int currentGroupId)
        {
            var group = await _groupApplication.GetGroupBy(long.Parse(groupId));

            if (group == null)
            {
                await Clients.Caller.SendAsync("Error", "Group not found");
            }
            else
            {
                var userId = long.Parse(_authHelper.GetUserId(Context.User));
                var token = Guid.Parse(group.Token);

                if (!await _groupApplication.IsUserInGroup(userId, token))
                {

                    await _groupApplication.JoinGroup(userId, token);
                    await Clients.Caller.SendAsync("NewGroup", group.GroupTitle, group.Picture, group.Id);
                }
                if (currentGroupId > 0)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group.Id.ToString());
                }
                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Caller.SendAsync("JoinGroup",group);
            }

        }
    }
}
