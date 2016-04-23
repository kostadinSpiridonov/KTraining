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
    public class LevelManualTestController : BaseController
    {
        //GET: Start solving
        [HttpGet]
        public ActionResult StartTest(int id)
        {
            var test = this.levelTestForSolvingService.GetById(id);
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            if (test.StudentId == student.Id)
            {
                var sId = this.solvedManualTestForLevelService.Add(
                    new SolvedManualTestForLevel
                    {
                        StudentId = student.Id,
                        StartTime = DateTime.Now,
                        CourseLevelId = test.CourseLevelId,
                        TestId = test.CourseLevel.ManualTestId.Value,
                        SolvedCloseQuestions = OtherFunctions
                            .Shuffle<CloseQuestion>(test.CourseLevel.ManualTest.CloseQuestions.ToList())
                            .ToList()
                            .ConvertAll(x => new SolvedCloseQuestion
                            {
                                CloseQuestionId = x.Id
                            }),
                        SolvedOpenQuestions = OtherFunctions
                       .Shuffle<OpenQuestion>(test.CourseLevel.ManualTest.OpenQuestions.ToList())
                       .ToList()
                       .ConvertAll(x => new SolvedOpenQuestion
                       {
                           OpenQuestionId = x.Id
                       })
                    });
                var newTest = new StartManualTestViewModel
                {
                    Id = test.Id,
                    TeacherName = test.CourseLevel.ManualTest.Teacher.ApplicationUser.FirstName + test.CourseLevel.ManualTest.Teacher.ApplicationUser.LastName,
                    Title = test.CourseLevel.ManualTest.Title,
                    Time = test.CourseLevel.ManualTest.Time,
                    SolvedTestId = sId,
                    CountQuestion = test.CourseLevel.ManualTest.CloseQuestions.Count + test.CourseLevel.ManualTest.OpenQuestions.Count

                };
                 this.studentService.RemoveManualTestForLevelToSolve(student.Id, id, test.CourseLevel.CourseId);
                return View(newTest);
            }

            return Redirect("/");
        }

        //GET: Get question
        [HttpGet]
        public ActionResult Question(int questionIndex, int solvedTestId)
        {
            var solvedTest = this.solvedManualTestForLevelService.GetById(solvedTestId);
            if (questionIndex >= solvedTest.SolvedCloseQuestions.Count)
            {
                if (questionIndex >= solvedTest.SolvedOpenQuestions.Count + solvedTest.SolvedCloseQuestions.Count)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                var solvedOpenQuestion = this.solvedOpenQuestionService.GetOpenQuestionByManualTestForLevelAndIndex(solvedTestId, questionIndex - solvedTest.SolvedCloseQuestions.Count);
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
            var solvedQuestion = this.solvedCloseQuestionService.GetCloseQuestionByManualTestForLevelAndIndex(solvedTestId, questionIndex);
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
            var test = this.solvedManualTestForLevelService.GetById(solvedTestId);
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
            if (id != 0)
            {
                var solvedTest = this.solvedManualTestForLevelService.GetById(id);
                this.notificationService.Add(new Notification
                {
                    UserId = solvedTest.CourseLevel.Course.Teacher.ApplicationUserId,
                    Content = "^q27e30c9e92b4b11bb5a420f7a98e29d^",
                    Link = "/CheckManualTestForLevel/Tests"

                });
                return View("ResultOfTest");
            }
            return Redirect("/");
        }
    }
}