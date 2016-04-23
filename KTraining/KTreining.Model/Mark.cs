using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class Mark
    {
        public int Id { get; set; }

        [Required]
        public double MarkNum { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public bool Seen { get; set; }

        public string Description { get; set; }
    }
}
