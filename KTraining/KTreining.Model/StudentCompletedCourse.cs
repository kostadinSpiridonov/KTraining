using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class StudentCompletedCourse
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public double Mark { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
