using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;
using KTreining.Model;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class AutomaticTestController : BaseController
    {
        // GET: Show Automatic Test
        [HttpGet]
        public ActionResult Show(int id)
        {
            if (this.BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                var test = this.automaticTestService.GetById(id);
                var viewModel = new ShowAutomaticTestViewModel
                    {
                        Id = test.Id,
                        Rate = test.Rate,
                        Title = test.Title,
                        CloseQuestions = test.CloseQuestions.ToList(),
                        Time = test.Time
                    };
                foreach (var item in viewModel.CloseQuestions)
                {
                    item.Images = this.cloudinaryService.AddPathToQuestionImageName(item.Images);
                    foreach (var an in item.Answers)
                    {
                        an.Images = this.cloudinaryService.AddPathToQuestionImageName(an.Images);
                    }
                }
                return View(viewModel);
            }
            return Redirect("/");
        }

        // GET: Automatic Tests
        [HttpGet]
        public ActionResult Tests()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var tests = this.automaticTestService.GetForTeacher(teacher.Id);
            var viewModel = tests.Reverse().ToList()
                .ConvertAll(x => new TestViewModel
                {
                    Id = x.Id,
                    Title = x.Title
                });
            return View(viewModel);
        }

        //GET: Add random test
        [HttpGet]
        public ActionResult AddRandomTest()
        {
            return View();
        }

        //POST: Add random test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRandomTest(AddRandomAutomaticTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var testId = this.automaticTestService.Add(
                new AutomaticTest
                {
                    Rate = model.Rate,
                    TeacherId = teacher.Id,
                    Title = model.Title,
                    Time = model.Time
                });
            List<int> closeQuestionsIds = new List<int>();
            int index;
            Random rand = new Random();
            foreach (var item in model.SelectedTopics)
            {
                if (item.Selected == true)
                {
                    closeQuestionsIds = this.closeQuestionService.GetForTopic(item.Id).Select(x => x.Id).ToList<int>();
                    if (item.QuestionCount <= closeQuestionsIds.Count)
                    {
                        for (int i = 0; i < item.QuestionCount; i++)
                        {
                            index = rand.Next(closeQuestionsIds.Count);
                            this.automaticTestService.AddCloseQuestion(closeQuestionsIds[index], testId);
                            closeQuestionsIds.RemoveAt(index);
                        }
                    }
                    else
                    {
                        var size = closeQuestionsIds.Count;
                        for (int i = 0; i < size; i++)
                        {
                            index = rand.Next(closeQuestionsIds.Count);
                            this.automaticTestService.AddCloseQuestion(closeQuestionsIds[index], testId);
                            closeQuestionsIds.RemoveAt(index);
                        }
                    }
                }
            }
            return Redirect("/AutomaticTest/Tests");
        }

        //POST: Select topics for the random test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectTopicsRandom(AddRandomAutomaticTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddRandomTest", model);
            }
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var topics = this.topicService.GetForTeacher(teacher.Id);
            var viewModel = topics.ToList().ConvertAll(x =>
                     new SelectTopicAutomaticRandViewModel
                     {
                         Name = x.Name,
                         Id = x.Id
                     });
            model.SelectedTopics = viewModel;
            return View(model);
        }

        // POST: Delete Automatic Test
        [HttpPost]
        public ActionResult Delete(int id)
        {

            if (this.BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                try
                {
                    this.automaticTestService.Delete(id);
                }
                catch
                {
                    TempData["DelError"] = Common.SomeoneUserThisTest;
                }
                return Redirect("/AutomaticTest/Tests");
            }
            return Redirect("/");
        }

        //GET: Add Simple test
        [HttpGet]
        public ActionResult AddSimpleTest()
        {
            return View();
        }

        //POST: Add simple test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSimpleTest(AddSimpleAutomaticTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var testId = this.automaticTestService.Add(new AutomaticTest
                {
                    Rate = model.Rate,
                    TeacherId = teacher.Id,
                    Title = model.Title,
                    Time = model.Time

                });
            foreach (var item in model.TopicQuestions)
            {
                foreach (var q in item.CloseQuestions)
                {
                    if (q.IsSelected)
                    {
                        this.automaticTestService.AddCloseQuestion(q.Question.Id, testId);
                    }
                }
            }
            return Redirect("/AutomaticTest/Tests");
        }

        //POST: Select questions for simple test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectQuestions(AddSimpleAutomaticTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddSimpleTest", model);
            }
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var topics = this.topicService.GetForTeacher(teacher.Id);
            var viewModel = new AddSimpleAutomaticTestViewModel
                     {
                         Rate = model.Rate,
                         Title = model.Title,
                         TopicQuestions = topics.ToList()
                             .ConvertAll(x => new TopicQuestionsAutomaticViewModel
                             {
                                 Name = x.Name,
                                 Id = x.Id,
                                 CloseQuestions = x.CloseQuestions.ToList().ConvertAll(r => new SelectCloseQuestionViewModel { Question = r })
                             }),
                         Time = model.Time
                     };
            return View(viewModel);
        }
        
        //POST: Select questions for simple test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuestion(int questionId, int testId)
        {

            if (this.BellongsToCurrentUser(testId, this.User.Identity.GetUserId()))
            {
                if (this.autoTestForSolvingService.CountTestWithId(testId) == 0)
                {
                    try
                    {
                        this.automaticTestService.RemoveQuestion(questionId, testId);
                        return Redirect("/AutomaticTest/Show/" + testId);
                    }
                    catch
                    {
                        TempData["DeQErrors"] = Common.SomeoneUserThisQ;
                        return Redirect("/AutomaticTest/Show/" + testId);
                    }
                }
                TempData["DeQErrors"] = Common.SomeoneUserThisQ;
                return Redirect("/AutomaticTest/Show/" + testId);
            }
            return Redirect("/");
        }

        //GET: Get add qustion view
        [HttpGet]
        public ActionResult AddQuestion(int id)
        {
            if (this.autoTestForSolvingService.CountTestWithId(id) == 0)
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var topics = this.topicService.GetForTeacher(teacher.Id);
                var test = this.automaticTestService.GetById(id);
                var viewModel = new AddQuestionToAutoTest
                {
                    Topics = topics.ToList()
                        .ConvertAll(x => new TopicQuestionsAutomaticViewModel
                        {
                            Name = x.Name,
                            Id = x.Id
                        }),
                    TestId = id
                };
               foreach(var item in viewModel.Topics)
               {
                   var questions = topics.Where(x => x.Id == item.Id).First().CloseQuestions.Where(y => test.CloseQuestions.Select(q=>q.Id).Contains(y.Id) == false);
                   item.CloseQuestions = questions.ToList().ConvertAll(r => new SelectCloseQuestionViewModel { Question = r });
               }
                return View(viewModel);
            }
            TempData["DeQErrors"] = Common.SomeoneUserThisTestNoQ;
            return Redirect("/AutomaticTest/Show/" + id);
        }

        //POST: Add question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(AddQuestionToAutoTest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            foreach (var topic in model.Topics)
            {
                foreach(var question in topic.CloseQuestions)
                {
                    if(question.IsSelected)
                    {
                        automaticTestService.AddCloseQuestion(question.Question.Id, model.TestId);
                    }
                }
            }
            return Redirect("/AutomaticTest/Show/" + model.TestId);
        }
        
        //Check if the item automatic test to current user
        private bool BellongsToCurrentUser(int id,string userId)
        {
           return this.automaticTestService.GetById(id).Teacher.ApplicationUserId == userId;
        }
    }
}