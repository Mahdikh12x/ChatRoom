using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.Chat;
using ChatRoomManagement.Application.Contracts.Group;
using Microsoft.AspNetCore.SignalR;

namespace ServiceHost.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IGroupApplication _groupApplication;
        private readonly IChatApplication _chatApplication;
        private readonly IAuthHelper _authHelper;

        public ChatHub(IGroupApplication groupApplication, IAuthHelper authHelper, IChatApplication chatApplication)
        {
            _groupApplication = groupApplication;
            _chatApplication = chatApplication;
            _authHelper = authHelper;
        }

        public override Task OnConnectedAsync()
        {
            var Id = _authHelper.GetUserId(Context.User);
            Clients.Caller.SendAsync("SetUserId", Id);
            return base.OnConnectedAsync();
        }

        public async Task JoinPublicGroup(string groupId, int currentGroupId)
        {
            var userId = long.Parse(_authHelper.GetUserId(Context.User));
            var group = await _groupApplication.GetGroupBy(long.Parse(groupId));
            var chats = await _chatApplication.GetChats(long.Parse(groupId), userId);
            if (group == null)
            {
                await Clients.Caller.SendAsync("Error", "Group not found");
            }
            else
            {

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
                await Clients.Caller.SendAsync("JoinGroup", group, chats);
            }

        }

        public async Task JoinPrivateGroup(string reciverId, int currentGroupId)
        {
            if (currentGroupId > 0)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, currentGroupId.ToString());

            var reciver=long.Parse(reciverId);
            var userId = long.Parse(_authHelper.GetUserId(Context.User));

            
            var group = await _groupApplication.IsExistPrivateGroupWith(userId, reciver);
            
            if (group.Id==0)
            {
                var newPrivateGroup = await _groupApplication.CreatePrivateGruop(userId, long.Parse(reciverId));
                await _groupApplication.JoinPrivateGroup(new JoinPrivateGroup
                {
                    GroupId = newPrivateGroup.Id,
                    OwnerId = newPrivateGroup.OwnerId,
                    ReciverId = newPrivateGroup.ReciverId
                });

                await Clients.Caller.SendAsync("NewGroup", newPrivateGroup.ReciverUserName, newPrivateGroup.ReciverPicture, newPrivateGroup.ReciverId,true);
                await Clients.User(newPrivateGroup.ReciverId.ToString()).SendAsync("NewGroup", newPrivateGroup.OwnerUserName, newPrivateGroup.OwnerPicture, newPrivateGroup.OwnerId,true);
                group=newPrivateGroup;
            }
            var chats=await _chatApplication.GetChats(group.Id,userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
            await Clients.Caller.SendAsync("JoinGroup",group,chats);
        }
    }
}
