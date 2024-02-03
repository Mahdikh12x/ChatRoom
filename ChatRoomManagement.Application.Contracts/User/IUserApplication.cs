using _01_framework.Application;

namespace ChatRoomManagement.Application.Contracts.User
{
    public interface IUserApplication
    {
        OperationResult CreateAccount(CreateAccount command);
        OperationResult EditAccount(EditAccount command);
        Task<UserViewModel> GetUserBy(long id);
        Task changeLastSeenStatus(long userId,bool status);
        Task EnterLastSeenDate(long userId);
        bool SignIn(SignInViewModel signInViewModel);
        void LogOut();

    }
}
