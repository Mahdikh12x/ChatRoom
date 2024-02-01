using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.Group;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServiceHost.Hubs;
using ChatRoomManagement.Application.Contracts.Chat;

namespace ServiceHost.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly IHubContext<ChatHub> _chathub;
        private readonly IGroupApplication _groupApplication;
        private readonly IAuthHelper _authHelper;
        private readonly IChatApplication _chatApplication;
        public GroupController(IHubContext<ChatHub> chathub, IGroupApplication groupApplication, IAuthHelper authHelper, IChatApplication chatApplication)
        {
            _chathub = chathub;
            _groupApplication = groupApplication;
            _authHelper = authHelper;
            _chatApplication = chatApplication;
        }

        [HttpPost]
        [Route("CreateGroup")]
        public async Task CreateGroup([FromForm] string groupName, IFormFile imageFile)
        {
            var userId = long.Parse(_authHelper.GetUserId(User));

            try
            {
                var createGroup = new CreateGroup()
                {
                    GroupTitle = groupName,
                    Picture = imageFile,
                    OwnerId = userId,
                    Token = Guid.NewGuid(),


                };


                var result = await _groupApplication.CreateGroup(createGroup);

                await _chathub.Clients.User(userId.ToString()).SendAsync("NewGroup", result.GroupTitle, result.Picture, result.Id);


            }
            catch
            {
                await _chathub.Clients.User(userId.ToString()).SendAsync("NewGroup", null);
            }
        }


        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search([FromQuery] string title)
        {
            var userId = long.Parse(_authHelper.GetUserId(User));
            return new ObjectResult(await _groupApplication.Search(title, userId));
        }


        [HttpPost]
        [Route("SendMessage")]

        public async Task SendMessage([FromForm] string body, [FromForm] long currentGroupId, [FromForm] IFormFile? file)
        {
            if (!string.IsNullOrWhiteSpace(body))
            {
                var userId = long.Parse(_authHelper.GetUserId(User));
                var command = new CreateChat
                {
                    GroupId = currentGroupId,
                    Body = body,
                    UserId = userId,
                    File = file
                };

                var result = await _chatApplication.CreateChat(command);

                var usersIdInGroup = await _groupApplication.usersIdInGroup(result.GroupId);
                await _chathub.Clients.Users(usersIdInGroup).SendAsync("ReciveNotification", result);
                await _chathub.Clients.Group(result.GroupId.ToString()).SendAsync("RecieveMessage", result);
            }




        }
    }
}
