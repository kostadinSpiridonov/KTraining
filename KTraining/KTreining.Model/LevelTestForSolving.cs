using System;
using System.Linq;

namespace KTreining.Model
{
    public class LevelTestForSolving
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int CourseLevelId { get; set; }
        public virtual CourseLevel CourseLevel { get; set; }
    }
}
