using KTraining.Resources.ViewModels;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class ShowAutomaticTestBaseViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class ShowAutomaticTestViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Rate { get; set; }

        [Required]
        public int Time { get; set; }

        public List<CloseQuestion> CloseQuestions { get; set; }
    }

    public class AddRandomAutomaticTestViewModel
    {
        [Required(
            ErrorMessageResourceName="FieldTitleRequired",
            ErrorMessageResourceType=typeof(Test)
            )]
        [Display(
            Name="Title",
            ResourceType=typeof(Test)
            )]
        public string Title { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldTimeRequired",
            ErrorMessageResourceType = typeof(Test)
            )]
        [Display(
            Name = "Time",
            ResourceType = typeof(Test)
            )]
        public int Time { get; set; }

        [Required(
            ErrorMessageResourceName = "FieldEvaulationRequired",
            ErrorMessageResourceType = typeof(Test)
            )]
        [Display(
            Name = "Evaulation",
            ResourceType = typeof(Test)
            )]
        public int Rate { get; set; }

        public List<SelectTopicAutomaticRandViewModel> SelectedTopics { get; set; }
    }

    public class AddSimpleAutomaticTestViewModel
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

        [Required(
          ErrorMessageResourceName = "FieldTimeRequired",
          ErrorMessageResourceType = typeof(Test)
          )]
        [Display(
            Name = "Time",
            ResourceType = typeof(Test)
            )]
        public int Time { get; set; }

        public List<TopicQuestionsAutomaticViewModel> TopicQuestions { get; set; }
    }

    public class StartAutoTestViewModel
    {
        public string TeacherName { get; set; }

        public string Title { get; set; }

        public int Id { get; set; }

        public int Time { get; set; }

        public int SolvedTestId { get; set; }

        public int QuestionCount { get; set; }
    }

    public class AddQuestionToAutoTest
    {
        [Required]
        public int TestId { get; set; }

        public List<TopicQuestionsAutomaticViewModel> Topics { get; set; }
    }
}