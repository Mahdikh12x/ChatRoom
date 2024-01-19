using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{

    [Authorize]

    public class IndexModel : PageModel
    {
        private readonly IGroupApplication _groupApplication;
        private readonly IAuthHelper _authHelper;
        public List<GroupViewModel> Groups { get; set; }
        public IndexModel(IGroupApplication groupApplication, IAuthHelper authHelper)
        {
            _groupApplication = groupApplication;
            _authHelper = authHelper;
        }

        public async Task OnGet()
        {
            var userId=_authHelper.GetUserId(User);
            Groups=await _groupApplication.GetGroupsBy(long.Parse(userId));
        }
    }
}