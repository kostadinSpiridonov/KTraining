using System;
using System.Linq;

namespace KTreining.Model
{
    public class AutomaticTestForSolving
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }


        public int? TestId { get; set; }
        public virtual AutomaticTest Test { get; set; }
    }
}
