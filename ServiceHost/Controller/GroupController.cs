using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.Group;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServiceHost.Hubs;

namespace ServiceHost.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly IHubContext<ChatHub> _chathub;
        private readonly IGroupApplication _groupApplication;
        private readonly IAuthHelper _authHelper;
        public GroupController(IHubContext<ChatHub> chathub, IGroupApplication groupApplication, IAuthHelper authHelper)
        {
            _chathub = chathub;
            _groupApplication = groupApplication;
            _authHelper = authHelper;
        }

        [HttpPost]
        [Route("CreateGroup")]
        public async Task CreateGroup([FromForm] string groupName,IFormFile imageFile)
        {
            var userId = _authHelper.GetUserId(User);

            try
            {
                var createGroup=new CreateGroup() 
                {
                    GroupTitle=groupName,
                    Picture=imageFile,
                    OwnerId = Guid.Parse(userId),
                    Token = Guid.NewGuid(),
                    
            
                };


                var result =await _groupApplication.CreateGroup(createGroup);
                
                await _chathub.Clients.User(userId.ToString()).SendAsync("NewGroup", result.GroupTitle, result.Picture, result.Token);


            }
            catch
            {
                await _chathub.Clients.User(userId.ToString()).SendAsync("NewGroup", null);
            }
        }
    }
}
