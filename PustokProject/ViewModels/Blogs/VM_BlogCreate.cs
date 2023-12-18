using System.ComponentModel.DataAnnotations;

namespace PustokProject.ViewModels.Blogs
{
    public class VM_BlogCreate
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        [MaxLength(120)]
        [MinLength(3)]
        public string Description { get; set; }

        [Required]
        [MinLength(3)]
        public string Content { get; set; }

        public int AuthorId{ get; set; }

        public ICollection<int> TagIds { get; set; }
    }
}
