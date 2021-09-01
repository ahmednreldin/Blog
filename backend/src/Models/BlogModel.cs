using System;
using System.ComponentModel.DataAnnotations;

namespace OpenSchool.src.Models
{
    public class BlogModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Body is required")]
        public string Body { get; set; }

    }
}
