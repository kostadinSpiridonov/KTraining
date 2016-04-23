using KTraining.Resources.ViewModels;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KTraining.Models
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class CourseDetails
    {
        public int Id { get; set; }

        public Teacher Teacher { get; set; }

        public string Description { get; set; }

        public string CourseName { get; set; }

        public int CountParticipants { get; set; }
    }

    public class CourseParticipants
    {
        public List<Student> Students { get; set; }

        public int Id { get; set; }

        public string UsernamesJson { get; set; }

        public string CourseName { get; set; }
    }

    public class CreateCourseViewModel
    {
        [Required(
            ErrorMessageResourceName="FieldNameRequired",
            ErrorMessageResourceType=typeof(Common)
            )]
        [Display(
            Name="Name",
            ResourceType=typeof(Common)
            )]
        public string Name { get; set; }

        [Display(
            Name = "Description",
            ResourceType=typeof(Common)
            )]
        [AllowHtml]
        public string Description { get; set; }
    }

    public class AddStudentToCourseViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public int CourseId { get; set; }

        public string UsernamesJson { get; set; }
    }

    public class CoursePostsViewModel
    {
        public List<Post> Posts { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

    }

    public class CourseSearchViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TeacherName { get; set; }

        public string Relation { get; set; }

        public string TeacherAppId { get; set; }
    }

    public class CoursesForExaminationViewModel
    {
        public List<CourseViewModel> Courses { get; set; }

        public int TestId { get; set; }

        public string Type { get; set; }
    }

    public class CompletedCourseViewModel
    {
        public string Name { get; set; }

        public double CourseMark { get; set; }

        public int CourseId { get; set; }
    }

    public class CourseEditViewModel
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

        [Display(
            Name = "Description",
            ResourceType = typeof(Common)
            )]
        [AllowHtml]
        public string Description { get; set; }
    }
}