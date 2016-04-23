using KTraining.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTreining.Model;
using KTraining.Service;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Student")]
    public class AutomaticTestStudentController : BaseController
    {
        //GET: Start solving
        [HttpPost]
        public ActionResult StartTest(int id)
        {

            var test = this.autoTestForSolvingService.GetById(id);
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            if (test.StudentId == student.Id)
            {
                var sId = this.solvedAutomaticTestService.Add(
                    new SolvedAutomaticTest
                    {
                        StudentId = student.Id,
                        StartTime = DateTime.Now,
                        CourseId = test.CourseId,
                        TestId = test.TestId.Value,
                        SolvedQuestions = OtherFunctions
                        .Shuffle<CloseQuestion>(test.Test.CloseQuestions.ToList())
                        .ToList()
                        .ConvertAll(x => new SolvedCloseQuestion
                        {
                            CloseQuestionId = x.Id
                        })
                    });
                var newTest = new StartAutoTestViewModel
                {
                    Id = test.Id,
                    TeacherName = test.Test.Teacher.ApplicationUser.FirstName + " " + test.Test.Teacher.ApplicationUser.LastName,
                    Title = test.Test.Title,
                    Time = test.Test.Time,
                    SolvedTestId = sId,
                    QuestionCount = test.Test.CloseQuestions.Count

                };
                this.studentService.RemoveAutoTestToSolve(student.Id, id, test.CourseId);
                return View(newTest);
            }
            return Redirect("/");
        }

        //GET: Get question
        [HttpGet]
        public ActionResult Question(int questionIndex, int solvedTestId)
        {
            var solvedTest = this.solvedAutomaticTestService.GetById(solvedTestId);
            if (questionIndex >= solvedTest.SolvedQuestions.Count)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            var solvedQuestion = this.solvedCloseQuestionService.GetQuestionByAutoTestAndIndex(solvedTestId, questionIndex);
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
                SolvedQuestionId = solvedQuestion.Id,
            };

            newQuestion.Answers = OtherFunctions.Shuffle<CloseAnswer>(newQuestion.Answers.ToList()).ToList();
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

        //POST: Send answer
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
            else if(question.MultipleSelected!=null)
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
            var test = this.solvedAutomaticTestService.GetById(solvedTestId);
            var startTime = test.StartTime;
            var time = test.Test.Time;
            var endTime = startTime.AddMinutes(time);
            if (DateTime.Now < endTime)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        //POST: End test
        [HttpGet]
        public ActionResult EndTest(int id)
        {
            if (id != 0)
            {
                var solvedTest = this.solvedAutomaticTestService.GetById(id);
                double testPoints = this.solvedAutomaticTestService.GetPointForTest(id);
                double allPoints = this.automaticTestService.GetPointsForTest(solvedTest.TestId);
                var testRate = this.automaticTestService.GetById(solvedTest.TestId).Rate;
                double grade = this.testService.CalculateMark(allPoints, testPoints, testRate);
                var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
                this.solvedAutomaticTestService.SetComplete(id);
                if (this.solvedAutomaticTestService.IsWholeTestComplete(solvedTest.TestId, solvedTest.CourseId.Value))
                {
                    var students = this.courseService.GetById(solvedTest.CourseId.Value).Students;
                    List<int> testIds = new List<int>();
                    foreach (var item in students)
                    {
                        foreach (var test in item.SolvedAutomaticTests)
                        {
                            testIds.Add(test.Id);
                        }
                    }
                    this.solvedAutomaticTestService.SetShow(testIds);
                }
                this.markService.Add(
                    new Mark
                    {
                        CourseId = solvedTest.CourseId.Value,
                        MarkNum = grade,
                        StudentId = student.Id,
                        Date = DateTime.Now
                    });
                this.notificationService.Add(new Notification
                {
                    UserId = student.ApplicationUserId,
                    Content = "^d482501cd64540118d525ea74747a692^(" + solvedTest.Course.Name + " - " + grade + ")",
                    Link = "/Mark/Marks"

                });
                return View("ResultOfTest", null, new ResultViewModel { CorrectAnswers = testPoints, Grade = grade });
            }
            return Redirect("/");
        }
    }
}