using ChatRoomManagement.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Auth.Register
{
	public class IndexModel : PageModel
	{
		private readonly IUserApplication _userApplication;
		public CreateAccount command { get; set; }
		public IndexModel(IUserApplication userApplication)
		{
			_userApplication = userApplication;
		}

		public void OnGet()
		{

		}

		public IActionResult OnPostCreateAccount(CreateAccount command)
		{
			//if (!ModelState.IsValid)
			//{
			//	return Page();
			//}

			_userApplication.CreateAccount(command);



			return RedirectToPage("/auth/login/Index");
		}
	}
}
