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
    public class OpenQuestionFullViewModel
    {
        public List<Image> Images { get; set; }

        public int Id { get; set; }

        [Required]
        [Display(
            Name = "QuestionN",
            ResourceType = typeof(Question)
            )]
        [AllowHtml]
        public string Content { get; set; }

        public virtual Topic Topic { get; set; }

        [Display(
            Name = "Points",
            ResourceType = typeof(Question)
            )]
        public double Points { get; set; }

        [Url]
        [Display(
            Name = "Help",
            ResourceType = typeof(Common)
            )]
        public string HelpLink { get; set; }
    }

    public class OpenQuestionsViewModel
    {
        public int TopicId { get; set; }
        public List<OpenQuestionViewModel> Questions { get; set; }
    }

    public class OpenQuestionViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }
    }

    public class AddOpenQuestionViewModel
    {
        [Required(
            ErrorMessageResourceName = "FieldQuestionRequired",
            ErrorMessageResourceType=typeof(Question)
            )]
        [AllowHtml]
        [Display(
         Name = "QuestionN",
         ResourceType = typeof(Question)
         )]
        public string Content { get; set; }

        public int TopicId { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldPointsRequired",
            ErrorMessageResourceType=typeof(Question)
            )]
        [Display(
            Name = "Points",
            ResourceType = typeof(Question)
            )]
        public double Points { get; set; }

        [Display(
            Name = "Pictures",
            ResourceType=typeof(Common)
            )]
        public HttpPostedFileBase[] Images { get; set; }

        [Url]
        [Display(
           Name = "Help",
           ResourceType = typeof(Common)
           )]
        public string HelpLink { get; set; }
    }

    public class SelectOpenQuestionViewModel
    {
        public OpenQuestion Question { get; set; }

        public bool IsSelected { get; set; }
    }

    public class SolveOpenQuestionViewModel
    {
        [Required]
        public int QuestionId { get; set; }

        public string Content { get; set; }

        public int Index { get; set; }

        public ICollection<Image> Images { get; set; }

        [Required]
        public int SolvedTestId { get; set; }

        [Required]
        public int SolvedQuestionId { get; set; }

        [AllowHtml]
        public string Answer { get; set; }
    }
}