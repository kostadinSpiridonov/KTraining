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
    public class CloseQuestionViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }
    }

    public class CloseQuestionShowViewModel
    {
        public int TopicId { get; set; }

        public List<CloseQuestionViewModel> CloseQuestions { get; set; }
    }

    public class AddCloseQuestionViewModel
    {
        [Required(
            ErrorMessageResourceName = "FieldQuestionRequired",
            ErrorMessageResourceType = typeof(Question)
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
            ErrorMessageResourceType = typeof(Question)
            )]
        [Display(
            Name = "Points",
            ResourceType = typeof(Question)
            )]
        public double Points { get; set; }

        [Display(
            Name = "Pictures",
            ResourceType = typeof(Common)
            )]
        public HttpPostedFileBase[] Images { get; set; }

        [Display(
            Name = "Help",
            ResourceType = typeof(Common)
            )]
        [Url]
        public string HelpLink { get; set; }

        public List<AddCloseAnswerViewModel> Answers { get; set; }
    }

    public class CloseQuestionFullViewModel
    {
        public List<CloseAnswer> Answers { get; set; }

        [Display(
          Name = "Pictures",
          ResourceType = typeof(Common)
          )]
        public List<Image> Images { get; set; }

        public int Id { get; set; }

        [Required(
               ErrorMessageResourceName = "FieldQuestionRequired",
               ErrorMessageResourceType = typeof(Question)
               )]
        [AllowHtml]
        [Display(
            Name = "QuestionN",
            ResourceType = typeof(Question)
            )]
        public string Content { get; set; }

        public virtual Topic Topic { get; set; }

        [Required(
           ErrorMessageResourceName = "FieldPointsRequired",
           ErrorMessageResourceType = typeof(Question)
           )]
        [Display(
            Name = "Points",
            ResourceType = typeof(Question)
            )]
        public double Points { get; set; }

        [Display(
                   Name = "Help",
                   ResourceType = typeof(Common)
                   )]
        [Url]
        public string HelpLink { get; set; }
    }

    public class SelectCloseQuestionViewModel
    {
        public CloseQuestion Question { get; set; }

        public bool IsSelected { get; set; }
    }

    public class SolveCloseQuestionViewModel
    {
        public ICollection<CloseAnswer> Answers { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public string Content { get; set; }

        public int Index { get; set; }

        public int SelectedAnswer { get; set; }

        public string AnswerContent { get; set; }

        public ICollection<Image> Images { get; set; }

        [Required]
        public int SolvedTestId { get; set; }

        [Required]
        public int SolvedQuestionId { get; set; }

        public List<MultipleAnswer> MultipleSelected { get; set; }

        public bool IsMultiple { get; set; }
    }

    public class MultipleAnswer
    {
        public int AnswerId { get; set; }

        public bool IsSelected { get; set; }
    }
}