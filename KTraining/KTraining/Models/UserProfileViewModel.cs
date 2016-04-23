using KTraining.Resources.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KTraining.Models
{
    public class UserProfileViewModel
    {
        [Required(
            ErrorMessageResourceName="FieldEmailRequired",
            ErrorMessageResourceType=typeof(Common)
            )]
        [EmailAddress]
        [Display(
            Name="Email",
            ResourceType=typeof(Common)
            )]
        public string Email { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldFirstNameRequired",
            ErrorMessageResourceType=typeof(Common)
            )]
        [Display(
            Name = "FirstName",
            ResourceType=typeof(Common)
            )]
        public string FirstName { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldSecondNameRequired",
            ErrorMessageResourceType=typeof(Common)
            )]
        [Display(
            Name = "SecondName",
            ResourceType=typeof(Common)
            )]
        public string SecondName { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldLastNameRequired",
            ErrorMessageResourceType=typeof(Common)
            )]
        [Display(
            Name = "LastName",
            ResourceType=typeof(Common)
            )]
        public string LastName { get; set; }

        [Display(
            Name = "Country",
            ResourceType=typeof(Common)
            )]
        public string Country { get; set; }

        [Display(
            Name = "City",
            ResourceType=typeof(Common)
            )]
        public string City { get; set; }

        [Display(
            Name = "AboutMe",
            ResourceType=typeof(Common)
            )]
        [AllowHtml]
        public string AboutMe { get; set; }

        [Display(
            Name = "GSM",
            ResourceType=typeof(Common)
            )]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(
            Name = "Skype",
            ResourceType=typeof(Common)
            )]
        public string Skype { get; set; }

        [Required]
        public string Id { get; set; }

        [Display(
            Name = "Role",
            ResourceType=typeof(Common)
            )]
        public string Role { get; set; }

        public List<CourseViewModel> CompleteCourses { get; set; }


        public List<CourseLevelsViewModel> CourseLevels { get; set; }
    }

    public class UserSearchViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }
    }

    public class StudentViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public bool IsChecked { get; set; }
    }
}