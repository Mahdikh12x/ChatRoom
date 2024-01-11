using _01_framework.Application;
using ChatRoomManagement.Application.Contracts.User;
using ChatRoomManagement.Domain.UserAgg;

namespace ChatRoomManagement.Application
{
    public class UserApplication : IUserApplication
	{
		private readonly IUserRepository _userRepository;
		private readonly IAuthHelper _authHelper;
		private readonly ISecurity _security;
		public UserApplication(IUserRepository userRepository, IAuthHelper authHelper, ISecurity security)
		{
			_userRepository = userRepository;
			_authHelper = authHelper;
			_security = security;
		}

		public OperationResult CreateAccount(CreateAccount command)
		{
			var operation = new OperationResult();

			if (_userRepository.IsExist(p => p.UserName == command.UserName || p.Email == command.Email))
				return operation.Failed("The Account already is exist");

			string password = _security.GetSHA256Hash(command.Password);
			
			string defulatPicture = "\\Users\\Default\\avatar.jpg";
			var user = new User(command.UserName, command.Email, command.UserName, password, defulatPicture);

			_userRepository.Create(user);
			_userRepository.SaveChanges();

			return operation.IsSucssed();
		}

		public OperationResult EditAccount(EditAccount command)
		{
			throw new NotImplementedException();
		}

        public void LogOut()
        {
            _authHelper.Singout();
        }

        public bool SignIn(SignInViewModel signInViewModel)
		{
			if (signInViewModel.Email == null || signInViewModel.Password == null)
				return false;

			var user = _userRepository.GetByEmail(signInViewModel.Email);
			if (user is null)
				return false;

			var checkPassword = _security.GetSHA256Hash(signInViewModel.Password);

			if (checkPassword==user.Password)
			{
				var authViewModel = new AuthViewModel()
				{
					Id = user.Id.ToString(),
					Name = user.Name,
					Email = user.Email,
				};

				_authHelper.SignIn(authViewModel);

				return true;
			}

			return false;
		}
	}
}
