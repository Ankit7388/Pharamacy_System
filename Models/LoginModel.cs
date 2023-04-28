using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class LoginModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email format")]
        public string UserEmail { get; set; }
        [Required]
        [RegularExpression("^((?=.?[A-Z])(?=.?[a-z])(?=.?[0-9])|(?=.?[A-Z])(?=.?[a-z])(?=.?[^a-zA-Z0-9])|(?=.?[A-Z])(?=.?[0-9])(?=.?[^a-zA-Z0-9])|(?=.?[a-z])(?=.?[0-9])(?=.?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
