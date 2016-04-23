using KTraining.Resources.ViewModels;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class AddRandomManualTestViewModel
    {
        [Required(
            ErrorMessageResourceName = "FieldTitleRequired",
            ErrorMessageResourceType=typeof(Test)
            )]
        [Display(
            Name="Title",
            ResourceType=typeof(Test)
            )]
        public string Title { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldEvaulationRequired",
            ErrorMessageResourceType=typeof(Test)
            )]
        [Display(
            Name="Evaulation",
            ResourceType=typeof(Test)
            )]
        public int Rate { get; set; }

        public List<SelectTopicManualRandViewModel> SelectedTopics { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldTimeRequired",
            ErrorMessageResourceType=typeof(Test)
            )]
        [Display(
            Name = "Time",
            ResourceType=typeof(Test)
            )]
        public int Time { get; set; }
    }

    public class ShowManualTestViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Rate { get; set; }


        [Required]
        public int Time { get; set; }

        public List<CloseQuestion> CloseQuestions { get; set; }

        public List<OpenQuestion> OpenQuestions { get; set; }
    }

    public class AddSimpleManualTestViewModel
    {
        [Required(
           ErrorMessageResourceName = "FieldTitleRequired",
           ErrorMessageResourceType = typeof(Test)
           )]
        [Display(
            Name = "Title",
            ResourceType = typeof(Test)
            )]
        public string Title { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldEvaulationRequired",
            ErrorMessageResourceType = typeof(Test)
            )]
        [Display(
            Name = "Evaulation",
            ResourceType = typeof(Test)
            )]
        public int Rate { get; set; }

        public List<TopicQuestionsManualViewModel> TopicQuestions { get; set; }


        [Required(
           ErrorMessageResourceName = "FieldTimeRequired",
           ErrorMessageResourceType = typeof(Test)
           )]
        [Display(
            Name = "Time",
            ResourceType = typeof(Test)
            )]
        public int Time { get; set; }
    }

    public class StartManualTestViewModel
    {
        public string TeacherName { get; set; }

        public string Title { get; set; }

        public int Id { get; set; }

        public int Time { get; set; }

        public int SolvedTestId { get; set; }

        public int CountQuestion { get; set; }
    }

    public class AddQuestionToManualTest
    {
        [Required]
        public int TestId { get; set; }

        public List<TopicQuestionsManualViewModel> Topics { get; set; }
    }

    public class ManualTestShowBaseViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}