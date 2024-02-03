using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.Group;
using ChatRoomManagement.Application.Contracts.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ServiceHost.Pages
{

    [Authorize]

    public class IndexModel : PageModel
    {
        private readonly IGroupApplication _groupApplication;
        private readonly IUserApplication _userApplication;
        private readonly IAuthHelper _authHelper;

        [BindProperty]
        public EditAccount EditAccount { get; set; }
        public GroupViewModel Groups { get; set; }
        public UserViewModel UserModel { get; set; }
        public IndexModel(IGroupApplication groupApplication, IAuthHelper authHelper,IUserApplication userApplication)
        {
            _groupApplication = groupApplication;
            _userApplication=userApplication;
            _authHelper = authHelper;
        }

        public async Task OnGet()
        {
            var userId=long.Parse(_authHelper.GetUserId(User));
            UserModel=await _userApplication.GetUserBy(userId);
            Groups=await _groupApplication.GetGroupsBy(userId);
        }

        public IActionResult OnPostEditAccount()
        {
            var result= _userApplication.EditAccount(EditAccount);
            return RedirectToPage("Index");
        }
    }
}