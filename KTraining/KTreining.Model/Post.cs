using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class Post
    {
        public int Id { get; set; }

        public string Content { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }


        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime Date { get; set; }
    }
}
