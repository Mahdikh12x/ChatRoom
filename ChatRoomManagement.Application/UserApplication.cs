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
        private readonly IFileUploader _fileUploader;
        public UserApplication(IUserRepository userRepository, IAuthHelper authHelper, ISecurity security, IFileUploader fileUploader)
        {
            _userRepository = userRepository;
            _authHelper = authHelper;
            _fileUploader = fileUploader;
            _security = security;
        }

        public async Task changeLastSeenStatus(long userId, bool status)
        {
            var user = _userRepository.GetBy(userId);
            if (user != null)
            {
                user.changeLastSeenStatus(status);
                _userRepository.SaveChanges();

            }
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
            var operation = new OperationResult();
            var user = _userRepository.GetBy(command.Id);
            if (user == null)
                return operation.Failed("User Not Find");

            if (_userRepository.IsExist(p => p.Email == command.Email && p.Id != command.Id))
                return operation.Failed("The Email is exist");

            var picture = _fileUploader.Upload(command.Avatar, "/User");
            user.Edit(command.Name, command.Email, picture);
            _userRepository.SaveChanges();

            if (command.Password != null)
            {
                var hashPassword = _security.GetSHA256Hash(command.Password);
                user.ChangePassword(hashPassword);
                _userRepository.SaveChanges();
            }

            return operation.IsSucssed();
        }

        public async Task EnterLastSeenDate(long userId)
        {
            var user = _userRepository.GetBy(userId);
            if (user != null)
            {
                user.EnterLastSeenDate();
                _userRepository.SaveChanges();
            }
        }

        public async Task<UserViewModel> GetUserBy(long id)
        {
            return await _userRepository.GetUserBy(id);
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

            if (checkPassword == user.Password)
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
