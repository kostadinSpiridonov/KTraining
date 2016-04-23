using KTreining.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class SolvedManualTestViewModel
    {
        private List<SolvedCloseQuestion> SolvedCloseQuestions { get; set; }

        private List<SolvedOpenQuestion> SolvedOpenQuestions { get; set; }

        public int Id { get; set; }

        public virtual Student Student { get; set; }

        public virtual ManualTest Test { get; set; }
    }

    public class SolvedManualTestShowViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CourseName { get; set; }

        public string StudentName { get; set; }

        public string StudentId { get; set; }

        public int CourseId { get; set; }
    }

    public class SolvedManualTestCheckViewModel
    {
        public List<SolvedCloseQuestion> SolvedCloseQuestions { get; set; }

        public List<SolvedOpenQuestion> SolvedOpenQuestions { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public virtual Student Student { get; set; }

        public double MaxPoints { get; set; }

        public string TestTitle { get; set; }

        public int Rate { get; set; }

        public int CourseLevelId { get; set; }
    }
}