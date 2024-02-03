using System.ComponentModel.DataAnnotations;

namespace ChatRoomManagement.Application.Contracts.User
{
    public class CreateAccount
    {


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string RePassword { get; set; }

    }
}
