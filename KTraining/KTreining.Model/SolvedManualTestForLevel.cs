using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class SolvedManualTestForLevel
    {

        private ICollection<SolvedCloseQuestion> solvedCloseQuestions;
        private ICollection<SolvedOpenQuestion> solvedOpenQuestions;

        public SolvedManualTestForLevel()
        {
            this.solvedCloseQuestions = new HashSet<SolvedCloseQuestion>();
            this.solvedOpenQuestions = new HashSet<SolvedOpenQuestion>();
        }

        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [Required]
        public virtual DateTime StartTime { get; set; }

        public virtual ICollection<SolvedCloseQuestion> SolvedCloseQuestions
        {
            get
            {
                return this.solvedCloseQuestions;
            }
            set
            {
                this.solvedCloseQuestions = value;
            }
        }

        public virtual ICollection<SolvedOpenQuestion> SolvedOpenQuestions
        {
            get
            {
                return this.solvedOpenQuestions;
            }
            set
            {
                this.solvedOpenQuestions = value;
            }
        }

        [Required]
        public int TestId { get; set; }
        public virtual ManualTest Test { get; set; }


        public int? CourseLevelId { get; set; }
        public virtual CourseLevel CourseLevel { get; set; }
    }
}
