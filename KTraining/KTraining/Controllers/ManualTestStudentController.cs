using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTreining.Model;
using KTraining.Service;
using KTraining.Models;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Student")]
    public class ManualTestStudentController : BaseController
    {
        //GET: Start solving
        [HttpPost]
        public ActionResult StartTest(int id)
        {
            var test = this.manualTestForSolvingService.GetById(id);
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            if (test.StudentId == student.Id)
            {
                var sId = this.solvedManualTestService.Add(
                    new SolvedManualTest
                    {
                        StudentId = student.Id,
                        StartTime = DateTime.Now,
                        CourseId = test.CourseId,
                        TestId = test.TestId.Value,
                        SolvedCloseQuestions = OtherFunctions
                            .Shuffle<CloseQuestion>(test.Test.CloseQuestions.ToList())
                            .ToList()
                            .ConvertAll(x => new SolvedCloseQuestion
                            {
                                CloseQuestionId = x.Id
                            }),
                        SolvedOpenQuestions = OtherFunctions
                       .Shuffle<OpenQuestion>(test.Test.OpenQuestions.ToList())
                       .ToList()
                       .ConvertAll(x => new SolvedOpenQuestion
                       {
                           OpenQuestionId = x.Id
                       })
                    });
                var newTest = new StartManualTestViewModel
                {
                    Id = test.Id,
                    TeacherName = test.Test.Teacher.ApplicationUser.FirstName + test.Test.Teacher.ApplicationUser.LastName,
                    Title = test.Test.Title,
                    Time = test.Test.Time,
                    SolvedTestId = sId,
                    CountQuestion = test.Test.CloseQuestions.Count + test.Test.OpenQuestions.Count

                };
                this.studentService.RemoveManualTestToSolve(student.Id, id, test.CourseId);
                return View(newTest);
            }
            return Redirect("/");
        }

        //GET: Get question
        [HttpGet]
        public ActionResult Question(int questionIndex, int solvedTestId)
        {
            var solvedTest = this.solvedManualTestService.GetById(solvedTestId);
            if (questionIndex >= solvedTest.SolvedCloseQuestions.Count)
            {
                if (questionIndex >= solvedTest.SolvedOpenQuestions.Count + solvedTest.SolvedCloseQuestions.Count)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                var solvedOpenQuestion = this.solvedOpenQuestionService.GetOpenQuestionByManualTestAndIndex(solvedTestId, questionIndex - solvedTest.SolvedCloseQuestions.Count);
                var newOpenQuestion = new SolveOpenQuestionViewModel
                {
                    Content = solvedOpenQuestion.OpenQuestion.Content,
                    QuestionId = solvedOpenQuestion.OpenQuestion.Id,
                    Index = questionIndex,
                    Images = this.cloudinaryService.AddPathToQuestionImageName(solvedOpenQuestion.OpenQuestion.Images).ToList(),
                    SolvedTestId = solvedTestId,
                    SolvedQuestionId = solvedOpenQuestion.Id
                };
                return PartialView("OpenQuestion", newOpenQuestion);

            }
            var solvedQuestion = this.solvedCloseQuestionService.GetCloseQuestionByManualTestAndIndex(solvedTestId, questionIndex);
            var newQuestion = new SolveCloseQuestionViewModel
            {
                Answers = solvedQuestion.CloseQuestion.Answers.ToList().ConvertAll(x => new CloseAnswer
                {
                    Content = x.Content,
                    Correct = x.Correct,
                    Id = x.Id,
                    Images = this.cloudinaryService.AddPathToQuestionImageName(x.Images),
                    Question = x.Question,
                    QuestionId = x.QuestionId
                }),
                Content = solvedQuestion.CloseQuestion.Content,
                QuestionId = solvedQuestion.CloseQuestion.Id,
                Index = questionIndex,
                Images = this.cloudinaryService.AddPathToQuestionImageName(solvedQuestion.CloseQuestion.Images).ToList(),
                SolvedTestId = solvedTestId,
                SolvedQuestionId = solvedQuestion.Id
            };
            newQuestion.Answers = OtherFunctions.Shuffle<CloseAnswer>(newQuestion.Answers.ToList());
            var correctAnswers = newQuestion.Answers.Where(x => x.Correct == true).Count();
            if (correctAnswers > 1)
            {
                newQuestion.IsMultiple = true;
                newQuestion.MultipleSelected = new List<MultipleAnswer>();
                foreach (var item in newQuestion.Answers)
                {
                    newQuestion.MultipleSelected.Add(new MultipleAnswer
                    {
                        AnswerId = item.Id,
                        IsSelected = false
                    });
                }
                newQuestion.MultipleSelected.ToList();
            }
            return PartialView(newQuestion);
        }

        //POST: Send answer for close question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendAnswer(SolveCloseQuestionViewModel question)
        {
            if (question.SolvedTestId == 0 || question.SolvedQuestionId == 0 || question.QuestionId == 0)
            {
                return RedirectToAction("Question", new { questionIndex = question.Index, solvedTestId = question.SolvedTestId });
            }
            if (question.SelectedAnswer != 0)
            {
                this.solvedCloseQuestionService.AddSelectedAnswer(new List<int> { question.SelectedAnswer }, question.SolvedQuestionId);
            }
            else if (question.MultipleSelected != null)
            {
                this.solvedCloseQuestionService.AddSelectedAnswer(question.MultipleSelected.Where(x => x.IsSelected == true)
                    .Select(x => x.AnswerId).ToList()
                    , question.SolvedQuestionId);
            }
            return RedirectToAction("Question", new { questionIndex = question.Index + 1, solvedTestId = question.SolvedTestId });
        }

        //POST: Check time for test
        [HttpPost]
        public JsonResult CheckTime(int solvedTestId)
        {
            var test = this.solvedManualTestService.GetById(solvedTestId);
            var startTime = test.StartTime;
            var time = test.Test.Time;
            var endTime = startTime.AddMinutes(time);
            if (DateTime.Now < endTime)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        //POST: Send answer for open question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendAnswerOpen(SolveOpenQuestionViewModel question)
        {
            if (question.SolvedTestId == 0 || question.SolvedQuestionId == 0 || question.QuestionId == 0)
            {
                return RedirectToAction("Question", new { questionIndex = question.Index, solvedTestId = question.SolvedTestId });
            }
            if (question.Answer != null)
            {
                this.solvedOpenQuestionService.AddAnswer(question.SolvedQuestionId, question.Answer);
            }
            return RedirectToAction("Question", new { questionIndex = question.Index + 1, solvedTestId = question.SolvedTestId });
        }

        //POST: End test
        [HttpGet]
        public ActionResult EndTest(int id)
        {
            if (id != 0 && BellongToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                var solvedTest = this.solvedManualTestService.GetById(id);
                this.solvedManualTestService.SetComplete(id);
                this.notificationService.Add(new Notification
                {
                    UserId = solvedTest.Course.Teacher.ApplicationUserId,
                    Content = "^q27e30c9e92b4b11bb5a420f7a98e29d^",
                    Link = "/CheckManualTest/Tests"

                });
                return View("ResultOfTest");
            }
            return Redirect("/");
        }

        //Check if the solved manual test belongs to current user
        private bool BellongToCurrentUser(int id, string userId)
        {
            return this.solvedManualTestService.GetById(id).Student.ApplicationUserId == userId;
        }
    }
}