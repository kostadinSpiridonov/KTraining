using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class SolvedAutomaticTest
    {
        private ICollection<SolvedCloseQuestion> solvedQuestions;

        public SolvedAutomaticTest()
        {
            this.solvedQuestions = new HashSet<SolvedCloseQuestion>();
        }

        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }


        [Required]
        public virtual DateTime StartTime { get; set; }

        public virtual ICollection<SolvedCloseQuestion> SolvedQuestions
        {
            get
            {
                return this.solvedQuestions;
            }
            set
            {
                this.solvedQuestions = value;
            }
        }

        [Required]
        public int TestId { get; set; }
        public virtual AutomaticTest Test { get; set; }

        public bool Show { get; set; }

        public bool IsComplete { get; set; }


        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
