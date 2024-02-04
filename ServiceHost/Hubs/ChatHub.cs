using _01_framework.Application;
using ChatRoomManagement.Application;
using ChatRoomManagement.Application.Contracts.Chat;
using ChatRoomManagement.Application.Contracts.Group;
using ChatRoomManagement.Application.Contracts.User;
using ChatRoomManagement.Domain.UserAgg;
using Microsoft.AspNetCore.SignalR;

namespace ServiceHost.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IGroupApplication _groupApplication;
        private readonly IChatApplication _chatApplication;
        private readonly IUserApplication _userApplication;
        private readonly IAuthHelper _authHelper;
        public ChatHub(IGroupApplication groupApplication, IAuthHelper authHelper, IChatApplication chatApplication, IUserApplication userApplication)
        {
            _groupApplication = groupApplication;
            _chatApplication = chatApplication;
            _userApplication = userApplication;
            _authHelper = authHelper;
        }

        public override Task OnConnectedAsync()
        {
            var userId = long.Parse(_authHelper.GetUserId(Context.User));
            Clients.Caller.SendAsync("SetUserId", userId);
            _userApplication.changeLastSeenStatus(userId, true);


            Clients.Others.SendAsync("ChangeStatus", userId, true);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = long.Parse(_authHelper.GetUserId(Context.User));
            _userApplication.changeLastSeenStatus(userId, false);
            _userApplication.EnterLastSeenDate(userId);

            string lastSeenDate=$"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            Clients.Others.SendAsync("ChangeStatus", userId, false,lastSeenDate);
            return base.OnDisconnectedAsync(exception);
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

            var reciver = long.Parse(reciverId);
            var userId = long.Parse(_authHelper.GetUserId(Context.User));


            var group = await _groupApplication.IsExistPrivateGroupWith(userId, reciver);

            if (group.Id == 0)
            {
                var newPrivateGroup = await _groupApplication.CreatePrivateGruop(userId, long.Parse(reciverId));
                await _groupApplication.JoinPrivateGroup(new JoinPrivateGroup
                {
                    GroupId = newPrivateGroup.Id,
                    OwnerId = newPrivateGroup.OwnerId,
                    ReciverId = newPrivateGroup.ReciverId
                });

                await Clients.Caller.SendAsync("NewGroup", newPrivateGroup.ReciverUserName, newPrivateGroup.ReciverPicture, newPrivateGroup.ReciverId, true);
                await Clients.User(newPrivateGroup.ReciverId.ToString()).SendAsync("NewGroup", newPrivateGroup.OwnerUserName, newPrivateGroup.OwnerPicture, newPrivateGroup.OwnerId, true);
                group = newPrivateGroup;
            }
            var chats = await _chatApplication.GetChats(group.Id, userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
            await Clients.Caller.SendAsync("JoinGroup", group, chats);
        }


        public async Task IsTyping(int currentGroupId, bool isTypingMood)
        {
            var userId = long.Parse(_authHelper.GetUserId(Context.User));
            var user = await _userApplication.GetUserBy(userId);
            var isGroupPrivate = await _groupApplication.IsGroupPrivate(currentGroupId);
            await Clients.OthersInGroup(currentGroupId.ToString()).SendAsync("TypingNotif", user, isTypingMood, isGroupPrivate);
        }
    }
}
