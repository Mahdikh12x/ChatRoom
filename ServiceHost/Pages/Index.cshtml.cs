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
        public List<GroupViewModel> Groups { get; set; }
        public IndexModel(IGroupApplication groupApplication)
        {
            _groupApplication = groupApplication;
        }

        public async Task OnGet()
        {
            Groups=await _groupApplication.GetGroups();
        }
    }
}