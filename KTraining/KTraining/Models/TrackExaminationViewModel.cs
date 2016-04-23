using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{

    public class CourseTrack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AutomaticTestTrack> AutoTests { get; set; }
        public List<ManualTestTrack> ManTests { get; set; }
        public CourseTrack()
        {
            AutoTests = new List<AutomaticTestTrack>();
            ManTests = new List<ManualTestTrack>();
        }
    }

    public class AutomaticTestTrack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StudentTrack> AutoTestsForSolving { get; set; }
        public List<StudentTrack> SolvedAutoTests { get; set; }
        public AutomaticTestTrack()
        {
            AutoTestsForSolving = new List<StudentTrack>();
            SolvedAutoTests = new List<StudentTrack>();
        }
    }

    public class ManualTestTrack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StudentTrack> ManTestsForSolving { get; set; }
        public List<StudentTrack> SolvedManTests { get; set; }
        public ManualTestTrack()
        {
            ManTestsForSolving = new List<StudentTrack>();
            SolvedManTests = new List<StudentTrack>();
        }
    }
    public class StudentTrack
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}