using System.ComponentModel.DataAnnotations;

namespace PustokProject.ViewModels.Profile
{
    public class VM_UserProfile
    {
        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageUrl { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Email { get; set; }


        public string? CurrentPassword { get; set; }

        [Compare(nameof(ReNewPassword))]
        public string? NewPassword { get; set; }
        public string? ReNewPassword { get; set; }
    }
}
