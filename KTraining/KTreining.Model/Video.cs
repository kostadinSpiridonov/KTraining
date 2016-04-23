using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class Video
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SourceId { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
