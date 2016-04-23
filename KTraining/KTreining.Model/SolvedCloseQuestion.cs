using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class SolvedCloseQuestion
    {
        public SolvedCloseQuestion()
        {
            this.selectedAnswers = new HashSet<CloseAnswer>();
        }
        public int Id { get; set; }

        [Required]
        public int CloseQuestionId { get; set; }
        public virtual CloseQuestion CloseQuestion { get; set; }

        private ICollection<CloseAnswer> selectedAnswers { get; set; }
        public virtual ICollection<CloseAnswer> SelectedAnswers
        {
            get
            {
                return this.selectedAnswers;
            }
            set
            {
                this.selectedAnswers = value;
            }
        }

        public int? SolvedAutomaticTestId { get; set; }
        public virtual SolvedAutomaticTest SolvedAutomaticTest { get; set; }


        public int? SolvedManualTestId { get; set; }
        public virtual SolvedManualTest SolvedManualTest { get; set; }
    }
}
