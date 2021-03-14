using System.ComponentModel.DataAnnotations;

namespace BookLibraryApi.Models
{
    public class RegisterModel
    {
        [MinLength(6)] [Required] public string UserName { get; set; }
        [Required] public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords has to be the same")]
        [DataType(DataType.Password)]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}