using _01_framework.Domain;
using ChatRoomManagement.Domain.ChatAgg;
using ChatRoomManagement.Domain.GroupAgg;

namespace ChatRoomManagement.Domain.UserAgg
{
    public class User:BaseEntity<long>
    {
        public string Name { get; private set; }
        public string Email { get;private set; }
        public string UserName { get;private set; }
        public bool IsActive { get;private set; }
        public bool IsOnline { get;private set; }
        public string Password { get;private set; }
        public string Picture { get;private set; }


        public List<Chat> Chats { get; private set; }
        public List<Group> Groups { get;private set; }

        public User(string name, string email, string userName, string password, string picture)
        {
            Name = name;
            Email = email;
            UserName = userName;
            Password = password;
            Picture = picture;
            IsActive = false;
            IsOnline = true;
            Groups=new List<Group>();
        }

        public void Edit(string name, string userName, string picture)
        {
            Name = name;
            UserName = userName;
            Picture = picture;
        }

        public void ChangePassword(string password)
        {
            Password=password;
        }

        
    }
}
