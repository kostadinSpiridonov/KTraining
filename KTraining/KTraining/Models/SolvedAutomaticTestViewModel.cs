using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class SolvedAutomaticTestViewModel
    {
        private List<SolvedCloseQuestion> SolvedQuestions { get; set; }

        public int Id { get; set; }

        public virtual Student Student { get; set; }

        public virtual AutomaticTest Test { get; set; }
    }

    public class SolvedAutomaticTestShowViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CourseName { get; set; }

        public int CourseId { get; set; }
    }
}