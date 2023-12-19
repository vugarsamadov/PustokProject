using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace PustokProject.ViewModels.Auth
{
    public class VM_Register
    {
        [Required,MinLength(3,ErrorMessage ="FullName should have at least 3 letters.")]
        [RegularExpression("^[a-zA-Z]* [a-zA-Z]*$",ErrorMessage ="Invalid format: write your fullname by seperating your first and last name with space")]
        public string FullName { get; set; }


        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }

		[Required, MaxLength(20,ErrorMessage ="Username too long!"),MinLength(3,ErrorMessage = "Username should have at least 3 letters.")]
        [RegularExpression("[a-z]*",ErrorMessage ="Only lowercase letters are allowed!")]
		public string Username { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(RePassword),ErrorMessage ="Passwords do not match!")]
        public string Password { get; set; }
        [Required,DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
