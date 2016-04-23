using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class PostAddViewModel
    {
        public int CourseId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}