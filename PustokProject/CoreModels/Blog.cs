using System.ComponentModel.DataAnnotations;

namespace PustokProject.CoreModels
{
    public class Blog : BaseModel
    {
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


        public int AuthorId { get; set; }
        public Author Author { get; set; }
        
        public ICollection<Tag> Tags { get; set; }
    }
}
