using KTraining.Resources.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class LevelTestForSolvingViewModel
    {
        public int Id { get; set; }

        [Display(
            Name="Levels",
            ResourceType=typeof(Common)
            )]
        public string LevelName { get; set; }

        [Display(
            Name = "Course",
            ResourceType=typeof(Common)
            )]
        public string CourseName { get; set; }

        public int CourseId { get; set; }

        [Display(
            Name = "TestN",
            ResourceType=typeof(Test)
            )]
        public string TestName { get; set; }

        public bool IsManual { get; set; }
    }
}