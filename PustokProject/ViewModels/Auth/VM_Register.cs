using System.ComponentModel.DataAnnotations;

namespace PustokProject.ViewModels.Auth
{
    public class VM_Register
    {
        [Required,MinLength(3,ErrorMessage ="FullName should have at least 3 letters.")]
        public string FullName { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
		[Required, MinLength(3, ErrorMessage = "Username should have at least 3 letters.")]
		public string Username { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(RePassword),ErrorMessage ="Passwords do not match!")]
        public string Password { get; set; }
        [Required,DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
