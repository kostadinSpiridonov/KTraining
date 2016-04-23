using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class CourseLevel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int? AutomaticTestId { get; set; }
        public virtual AutomaticTest AutomaticTest { get; set; }

        public int? ManualTestId { get; set; }
        public virtual ManualTest ManualTest { get; set; }

        public string Description { get; set; }
    }
}
