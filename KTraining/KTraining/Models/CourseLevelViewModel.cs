using KTraining.Resources.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KTraining.Models
{
    public class CourseLevelViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceName="FieldNameRequired",
            ErrorMessageResourceType=typeof(Common)
            )]
        [Display(
            Name="Name",
            ResourceType=typeof(Common)
            )]
        public string Name { get; set; }

        public int CourseId { get; set; }
    }

    public class CourseLevelsViewModel
    {
        public List<CourseLevelViewModel> Levels { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; }
    }
  
    public class AddCourseLevelViewModel
    {
        [Required]
        public int CourseId { get; set; }

        [Required(
             ErrorMessageResourceName = "FieldNameRequired",
             ErrorMessageResourceType = typeof(Common)
             )]
        [Display(
            Name = "Name",
            ResourceType = typeof(Common)
            )]
        public string Name { get; set; }

        public int ManualTestId { get; set; }

        public int AutomaticTestId { get; set; }

        [Display(
            Name = "Description",
            ResourceType=typeof(Common)
            )]
        [AllowHtml]
        public string Description { get; set; }

        public int Id { get; set; }
    }

    public class CourseLevelAddFullViewModel
    {
        public AddCourseLevelViewModel AddCourseLevelViewModel { get; set; }

        public List<ShowAutomaticTestBaseViewModel> AutomaticTests { get; set; }

        public List<ManualTestShowBaseViewModel> ManualTests { get; set; }
    }

    public class CourseLevelDetailsViewModel
    {
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldNameRequired",
            ErrorMessageResourceType = typeof(Common)
            )]
        [Display(
            Name = "Name",
            ResourceType = typeof(Common)
            )]
        public string Name { get; set; }

        public int TestId { get; set; }

        public bool IsManualTest { get; set; }

        [Display(
           Name = "TestN",
           ResourceType = typeof(Test)
           )]
        public string TestTitle { get; set; }

        [Display(
           Name = "Description",
           ResourceType = typeof(Common)
           )]
        [AllowHtml]
        public string Description { get; set; }
    }
}