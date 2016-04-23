using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;
using KTreining.Model;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class ManualTestController : BaseController
    {
        // GET: Show manual test
        [HttpGet]
        public ActionResult Show(int id)
        {
            if (this.BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                var test = this.manualTestService.GetById(id);
                var viewModel = new ShowManualTestViewModel
                {
                    Id = test.Id,
                    Rate = test.Rate,
                    Title = test.Title,
                    CloseQuestions = test.CloseQuestions.ToList(),
                    OpenQuestions = test.OpenQuestions.ToList(),
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
                foreach (var item in viewModel.OpenQuestions)
                {
                    item.Images = this.cloudinaryService.AddPathToQuestionImageName(item.Images);
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
            var tests = this.manualTestService.GetForTeacher(teacher.Id);
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
        public ActionResult AddRandomTest(AddRandomManualTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var testId = this.manualTestService.Add(
                new ManualTest
                {
                    Rate = model.Rate,
                    TeacherId = teacher.Id,
                    Title = model.Title,
                    Time = model.Time
                });
            List<int> closeQuestionsIds = new List<int>();
            List<int> openQuestionsIds = new List<int>();
            int index;
            Random rand = new Random();
            foreach (var item in model.SelectedTopics)
            {
                if (item.SelectedCloseQ == true)
                {
                    closeQuestionsIds = this.closeQuestionService.GetForTopic(item.Id).Select(x => x.Id).ToList<int>();
                    if (item.CloseQCount <= closeQuestionsIds.Count)
                    {
                        for (int i = 0; i < item.CloseQCount; i++)
                        {
                            index = rand.Next(closeQuestionsIds.Count);
                            this.manualTestService.AddCloseQuestion(closeQuestionsIds[index], testId);
                            closeQuestionsIds.RemoveAt(index);
                        }
                    }
                    else
                    {
                        var size = closeQuestionsIds.Count;
                        for (int i = 0; i < size; i++)
                        {
                            index = rand.Next(closeQuestionsIds.Count);
                            this.manualTestService.AddCloseQuestion(closeQuestionsIds[index], testId);
                            closeQuestionsIds.RemoveAt(index);
                        }
                    }
                }
                if (item.SelectedOpenQ == true)
                {
                    openQuestionsIds = this.openQuestionService.GetForTopic(item.Id).Select(x => x.Id).ToList<int>();
                    if (item.OpenQCount <= openQuestionsIds.Count)
                    {
                        for (int i = 0; i < item.OpenQCount; i++)
                        {
                            index = rand.Next(openQuestionsIds.Count);
                            this.manualTestService.AddOpenQuestion(openQuestionsIds[index], testId);
                            openQuestionsIds.RemoveAt(index);
                        }
                    }
                    else
                    {
                        var size = openQuestionsIds.Count;
                        for (int i = 0; i < size; i++)
                        {
                            index = rand.Next(openQuestionsIds.Count);
                            this.manualTestService.AddOpenQuestion(openQuestionsIds[index], testId);
                            openQuestionsIds.RemoveAt(index);
                        }
                    }
                }
            }
            return Redirect("/ManualTest/Tests");
        }

        //POST: Select topics for the random test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectTopicsRandom(AddRandomManualTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddRandomTest", model);
            }
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var topics = this.topicService.GetForTeacher(teacher.Id);
            var viewModel = topics.ToList().ConvertAll(x =>
                     new SelectTopicManualRandViewModel
                     {
                         Name = x.Name,
                         Id = x.Id
                     });
            model.SelectedTopics = viewModel;
            return View(model);
        }

        // GET: Manual Test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (this.BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                try
                {
                    this.manualTestService.Delete(id);
                }
                catch
                {
                    TempData["DelMError"] = Common.SomeoneUserThisTest;
                }
                return Redirect("/ManualTest/Tests");
            }
            return Redirect("/");
        }

        // GET: Add simple test
        [HttpGet]
        public ActionResult AddSimpleTest()
        {
            return View();
        }

        //POST: Add random test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSimpleTest(AddSimpleManualTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var testId = this.manualTestService.Add(new ManualTest
            {
                Rate = model.Rate,
                TeacherId = teacher.Id,
                Title = model.Title,
                Time = model.Time

            });
            if (model.TopicQuestions != null)
            {
                foreach (var item in model.TopicQuestions)
                {
                    if (item.CloseQuestions != null)
                    {
                        foreach (var q in item.CloseQuestions)
                        {
                            if (q.IsSelected)
                            {
                                this.manualTestService.AddCloseQuestion(q.Question.Id, testId);
                            }
                        }
                    }
                    if (item.OpenQuestions != null)
                    {
                        foreach (var q in item.OpenQuestions)
                        {
                            if (q.IsSelected)
                            {
                                this.manualTestService.AddOpenQuestion(q.Question.Id, testId);
                            }
                        }
                    }
                }
            }
            return Redirect("/ManualTest/Tests");
        }

        //POST: Select questions for test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectQuestions(AddSimpleManualTestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddSimpleTest", model);
            }

            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var topics = this.topicService.GetForTeacher(teacher.Id);
            var viewModel = new AddSimpleManualTestViewModel
            {
                Rate = model.Rate,
                Title = model.Title,
                Time = model.Time,
                TopicQuestions = topics.ToList()
                    .ConvertAll(x => new TopicQuestionsManualViewModel
                    {
                        Name = x.Name,
                        Id = x.Id,
                        OpenQuestions = x.OpenQuestions.ToList().ConvertAll(q => new SelectOpenQuestionViewModel { Question = q }),
                        CloseQuestions = x.CloseQuestions.ToList().ConvertAll(r => new SelectCloseQuestionViewModel { Question = r })
                    })
            };
            return View(viewModel);
        }

        //POST: Delete close question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCloseQuestion(int questionId, int testId)
        {
            if (this.BellongsToCurrentUser(testId, this.User.Identity.GetUserId()))
            {
                if (this.manualTestForSolvingService.CountTestWithId(testId) == 0)
                {
                    try
                    {
                        this.manualTestService.RemoveCloseQuestion(questionId, testId);
                        return Redirect("/ManualTest/Show/" + testId);
                    }
                    catch
                    {
                        TempData["DeQErrors"] = Common.SomeoneUserThisQ;
                        return Redirect("/ManualTest/Show/" + testId);
                    }
                }
                TempData["DeQErrors"] = Common.SomeoneUserThisQ;
                return Redirect("/ManualTest/Show/" + testId);
            }
            return Redirect("/");
        }

        //POST: Delete open question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOpenQuestion(int questionId, int testId)
        {
            if (this.BellongsToCurrentUser(testId, this.User.Identity.GetUserId()))
            {
                if (this.manualTestForSolvingService.CountTestWithId(testId) == 0)
                {
                    try
                    {
                        this.manualTestService.RemoveOpenQuestion(questionId, testId);
                        return Redirect("/ManualTest/Show/" + testId);
                    }
                    catch
                    {
                        TempData["DeQErrors"] = Common.SomeoneUserThisQ;
                        return Redirect("/ManualTest/Show/" + testId);
                    }
                }
                TempData["DeQErrors"] = Common.SomeoneUserThisQ;
                return Redirect("/ManualTest/Show/" + testId);
            }
            return Redirect("/");
        }

        //GET: Add question
        [HttpGet]
        public ActionResult AddQuestion(int id)
        {
            if (this.BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                if (this.manualTestForSolvingService.CountTestWithId(id) == 0)
                {
                    var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                    var topics = this.topicService.GetForTeacher(teacher.Id);
                    var test = this.manualTestService.GetById(id);
                    var viewModel = new AddQuestionToManualTest
                    {
                        Topics = topics.ToList()
                            .ConvertAll(x => new TopicQuestionsManualViewModel
                            {
                                Name = x.Name,
                                Id = x.Id,
                                // OpenQuestions = x.OpenQuestions.ToList().ConvertAll(q => new SelectOpenQuestionViewModel { Question = q }),
                                // CloseQuestions = x.CloseQuestions.ToList().ConvertAll(r => new SelectCloseQuestionViewModel { Question = r })
                            }),
                        TestId = id
                    };
                    foreach (var item in viewModel.Topics)
                    {
                        var cQuestions = topics.Where(x => x.Id == item.Id).First().CloseQuestions.Where(y => test.CloseQuestions.Select(q => q.Id).Contains(y.Id) == false);
                        var oQuestions = topics.Where(x => x.Id == item.Id).First().OpenQuestions.Where(y => test.OpenQuestions.Select(q => q.Id).Contains(y.Id) == false);
                        item.CloseQuestions = cQuestions.ToList().ConvertAll(r => new SelectCloseQuestionViewModel { Question = r });
                        item.OpenQuestions = oQuestions.ToList().ConvertAll(r => new SelectOpenQuestionViewModel { Question = r });
                    }
                    return View(viewModel);
                }
                TempData["DeQErrors"] = Common.SomeoneUserThisTestNoQ;
                return Redirect("/ManualTest/Show/" + id);
            }
            return Redirect("/");
        }

        //POST: Add question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(AddQuestionToManualTest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            foreach (var topic in model.Topics)
            {
                if (topic.CloseQuestions != null)
                {
                    foreach (var cQuestion in topic.CloseQuestions)
                    {
                        if (cQuestion.IsSelected)
                        {
                            manualTestService.AddCloseQuestion(cQuestion.Question.Id, model.TestId);
                        }
                    }
                }
                if (topic.OpenQuestions != null)
                {
                    foreach (var oQuestion in topic.OpenQuestions)
                    {
                        if (oQuestion.IsSelected)
                        {
                            manualTestService.AddOpenQuestion(oQuestion.Question.Id, model.TestId);
                        }
                    }

                }
            }
            return Redirect("/ManualTest/Show/" + model.TestId);
        }

        //Check if the manual test belongs to current user
        private bool BellongsToCurrentUser(int id, string userId)
        {
            return this.manualTestService.GetById(id).Teacher.ApplicationUserId == userId;
        }
    }
}