using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class TestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class SelectTestViewModel
    {
        public List<ManualTest> ManualTests { get; set; }

        public List<AutomaticTest> AutomaticTests { get; set; }
    }

    public class UnsolvedTestsViewModel
    {
        public List<ManualTestForSolving> ManualTests { get; set; }

        public List<AutomaticTestForSolving> AutomaticTests { get; set; }
    }
}