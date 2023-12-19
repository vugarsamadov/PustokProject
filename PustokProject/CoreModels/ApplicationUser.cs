using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokProject.CoreModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? ProfileImageUrl { get; set; }

        public override bool TwoFactorEnabled { get => false;}

        [NotMapped]
        public  override string PhoneNumber { get; set; }
        [NotMapped]
        public  override bool PhoneNumberConfirmed { get; set; }

    }
}
