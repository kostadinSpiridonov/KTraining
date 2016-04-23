using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class SolvedTestViewModel
    {
        public List<SolvedAutomaticTestShowViewModel> AutomaticSolvedTests { get; set; }
        public List<SolvedManualTestShowViewModel> ManualSolvedTest { get; set; }
    }

    public class SolvedAutoTestFullViewModel
    {
        public string CourseName { get; set; }

        public int TestTime { get; set; }

        public int Rate { get; set; }

        public List<SolvedCloseQuestion> Questions { get; set; }

        public double Mark { get; set; }

        public string TestTitle { get; set; }

        public int CourseId { get; set; }
    }

    public class SolvedManualTestFullViewModel
    {
        public string CourseName { get; set; }

        public int TestTime { get; set; }

        public int Rate { get; set; }

        public List<SolvedCloseQuestion> CloseQuestions { get; set; }

        public List<SolvedOpenQuestion> OpenQuestions { get; set; }

        public double Mark { get; set; }

        public string TestTitle { get; set; }

        public int CourseId { get; set; }
    }

}