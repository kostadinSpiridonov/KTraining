using KTraining.Resources.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KTraining.Models
{
    public class AddCloseAnswerViewModel
    {
        [Required(
            ErrorMessageResourceName="FieldAnswerRequired",
            ErrorMessageResourceType=typeof(Question)
            )]
        [AllowHtml]
        [Display(
            Name = "Answer",
            ResourceType=typeof(Question)
            )]
        public string Content { get; set; }

        [Required]
        [Display(
            Name = "Correct",
            ResourceType=typeof(Common)
            )]        
        public bool Correct { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Display(
            Name = "Pictures",
            ResourceType = typeof(Common)
            )]   
        public HttpPostedFileBase[] Images { get; set; }
    }

    public class UpdateCloseAnswerViewModel
    {
        [Required]
        public int AnswerId { get; set; }

        [Required]
        [AllowHtml]
        [Display(
          Name = "Answer",
          ResourceType = typeof(Question)
          )]
        public string Content { get; set; }

        [Required]
        [Display(
           Name = "Correct",
           ResourceType = typeof(Common)
           )]  
        public bool Correct { get; set; }

        [Required]
        public int QuestionId { get; set; }
    }
}