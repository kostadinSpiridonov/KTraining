using KTraining.Resources.ViewModels;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class TopicViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AddTopicViewModel
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
    }

    public class TypeQuestionsViewModel
    {
        public int TopicId { get; set; }

        public string TopicName { get; set; }
    }

    public class SelectTopicAutomaticRandViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }

        public int QuestionCount { get; set; }
    }

    public class SelectTopicManualRandViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool SelectedCloseQ { get; set; }

        public bool SelectedOpenQ { get; set; }

        public int OpenQCount { get; set; }

        public int CloseQCount { get; set; }
    }

    public class TopicQuestionsManualViewModel
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public List<SelectCloseQuestionViewModel> CloseQuestions { get; set; }

        public List<SelectOpenQuestionViewModel> OpenQuestions { get; set; }
    }

    public class TopicQuestionsAutomaticViewModel
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public List<SelectCloseQuestionViewModel> CloseQuestions { get; set; }
    }
}