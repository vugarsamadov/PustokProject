﻿using PustokProject.CoreModels;
using System.ComponentModel.DataAnnotations;

namespace PustokProject.ViewModels.Blogs
{
    public class VM_BlogIndex
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

        public string AuthorName { get; set; }

        public string Tags{ get; set; }

        public bool IsDeleted { get; set; }


    }
}
