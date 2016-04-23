using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class SolvedOpenQuestion
    {
        public int Id { get; set; }

        [Required]
        public int OpenQuestionId { get; set; }
        public virtual OpenQuestion OpenQuestion { get; set; }

        public string Answer { get; set; }

        public double Points { get; set; }

        public int? SolvedManualTestId { get; set; }
        public virtual SolvedManualTest SolvedManualTest { get; set; }
    }
}
