using ChatRoomManagement.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Auth.Login
{
	public class IndexModel : PageModel
	{
		private readonly IUserApplication _userApplication;
        public SignInViewModel signInViewModel { get; set; }
        public IndexModel(IUserApplication userApplication)
		{
			_userApplication = userApplication;
		}

		public void OnGet()
		{
		}


		public IActionResult OnPostSignIn(SignInViewModel signInViewModel)
		{
			if(!ModelState.IsValid)
				return Page();


			var result=_userApplication.SignIn(signInViewModel);

			if(result)
				return RedirectToPage("/Index");

			return Page();
		}
		

		public IActionResult OnGetLogOut()
		{
			_userApplication.LogOut();
			return RedirectToPage("/auth/login/Index");
		}


	}
}
