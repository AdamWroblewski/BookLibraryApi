using System.ComponentModel.DataAnnotations;

namespace BookLibraryApi.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public string Password { get; set; }
        
        [Compare("Password", ErrorMessage = "Passwords has to be the same")]

        public string ConfirmPassword { get; set; }
    }
}