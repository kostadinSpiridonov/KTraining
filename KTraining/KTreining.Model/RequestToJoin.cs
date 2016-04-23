using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class RequestToJoin
    {
        public int Id { get; set; }

        [Required]
        public int SendById { get; set; }
        public virtual Student SendBy { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
