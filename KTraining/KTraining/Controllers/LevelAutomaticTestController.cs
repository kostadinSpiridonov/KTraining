using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Service;
using KTraining.Models;
using Newtonsoft.Json;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Student")]
    public class LevelAutomaticTestController : BaseController
    {
        //GET: Start solving
        [HttpGet]
        public ActionResult StartTest(int id)
        {
            var test = this.levelTestForSolvingService.GetById(id);
            this.ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie("levelId") { Value = test.CourseLevelId.ToString() });
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            if (test.StudentId == student.Id)
            {
                var sTest =
                    new SolvedAutomaticTest
                    {
                        StudentId = student.Id,
                        StartTime = DateTime.Now,
                        CourseId = test.CourseLevel.CourseId,
                        TestId = test.CourseLevel.AutomaticTestId.Value,
                        SolvedQuestions = OtherFunctions
                        .Shuffle<CloseQuestion>(test.CourseLevel.AutomaticTest.CloseQuestions.ToList())
                        .ToList()
                        .ConvertAll(x => new SolvedCloseQuestion
                        {
                            CloseQuestionId = x.Id
                        }),
                        Course = new Course { Name = test.CourseLevel.Course.Name }
                    };

                HttpCookie cookie = new HttpCookie("testForLevel");
                cookie.Value = JsonConvert.SerializeObject(sTest);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                var newTest = new StartAutoTestViewModel
                {
                    Id = test.Id,
                    TeacherName = test.CourseLevel.AutomaticTest.Teacher.ApplicationUser.FirstName + " " + test.CourseLevel.AutomaticTest.Teacher.ApplicationUser.LastName,
                    Title = test.CourseLevel.AutomaticTest.Title,
                    Time = test.CourseLevel.AutomaticTest.Time,
                    QuestionCount = test.CourseLevel.AutomaticTest.CloseQuestions.Count

                };
                this.levelTestForSolvingService.Remove(id);
                return View(newTest);
            }

            return Redirect("/");
        }

        //GET: Get question\
        public ActionResult Question(int questionIndex)
        {
            var test = this.ControllerContext.HttpContext.Request.Cookies["testForLevel"].Value;
            var solvedTest = JsonConvert.DeserializeObject<SolvedAutomaticTest>(test);
            if (questionIndex >= solvedTest.SolvedQuestions.Count)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            var clQId = solvedTest.SolvedQuestions.ElementAt(questionIndex).CloseQuestionId;
            var solvedQuestion = this.closeQuestionService.GetById(clQId);
            var newQuestion = new SolveCloseQuestionViewModel
            {
                Answers = solvedQuestion.Answers.ToList().ConvertAll(x => new CloseAnswer
                {
                    Content = x.Content,
                    Correct = x.Correct,
                    Id = x.Id,
                    Images = this.cloudinaryService.AddPathToQuestionImageName(x.Images),
                    Question = x.Question,
                    QuestionId = x.QuestionId
                }),
                Content = solvedQuestion.Content,
                QuestionId = solvedQuestion.Id,
                Index = questionIndex,
                Images = this.cloudinaryService.AddPathToQuestionImageName(solvedQuestion.Images).ToList(),
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
            if (question.SolvedQuestionId == 0 || question.QuestionId == 0)
            {
                return RedirectToAction("Question", new { questionIndex = question.Index, solvedTestId = question.SolvedTestId });
            }
            var test = this.ControllerContext.HttpContext.Request.Cookies["testForLevel"].Value;
            var solvedTest = JsonConvert.DeserializeObject<SolvedAutomaticTest>(test);
            if (question.SelectedAnswer != 0)
            {
                var q = solvedTest.SolvedQuestions.Where(x => x.CloseQuestionId == question.SolvedQuestionId).First();
                var answer = this.closeAnswerService.GetById(question.SelectedAnswer);
                q.SelectedAnswers = new List<CloseAnswer> { new CloseAnswer{ Content="",Correct=answer.Correct,Id=answer.Id,
                Images=answer.Images,QuestionId=answer.QuestionId} };
            }
            else if (question.MultipleSelected != null)
            {
                var q = solvedTest.SolvedQuestions.Where(x => x.CloseQuestionId == question.SolvedQuestionId).First();
                var answers = new List<CloseAnswer>();
                foreach (var item in question.MultipleSelected.Where(x => x.IsSelected == true).Select(x => x.AnswerId))
                {
                    var newAnswer = this.closeAnswerService.GetById(item);
                    answers.Add(new CloseAnswer
                    {
                        Content = "",
                        Correct = newAnswer.Correct,
                        Id = newAnswer.Id,
                        Images = newAnswer.Images,
                        QuestionId = newAnswer.QuestionId
                    });
                }
                q.SelectedAnswers = answers;

            }
            HttpCookie cookie = new HttpCookie("testForLevel");
            cookie.Value = JsonConvert.SerializeObject(solvedTest);
            this.ControllerContext.HttpContext.Response.Cookies.Remove("testForLevel");
            this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

            return RedirectToAction("Question", new { questionIndex = question.Index + 1 });
        }

        //POST: Check time for test
        [HttpPost]
        public JsonResult CheckTime()
        {
            var test = this.ControllerContext.HttpContext.Request.Cookies["testForLevel"].Value;
            var solvedTest = JsonConvert.DeserializeObject<SolvedAutomaticTest>(test);
            var startTime = solvedTest.StartTime;
            var time = this.automaticTestService.GetById(solvedTest.TestId).Time;
            var endTime = startTime.AddMinutes(time);
            if (DateTime.Now < endTime)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        //POST: End test
        [HttpGet]
        public ActionResult EndTest()
        {
            var test = this.ControllerContext.HttpContext.Request.Cookies["testForLevel"].Value;
            var solvedTest = JsonConvert.DeserializeObject<SolvedAutomaticTest>(test);
            double testPoints = this.solvedAutomaticTestService.GetPointForTest(solvedTest);
            double allPoints = this.automaticTestService.GetPointsForTest(solvedTest.TestId);
            var testRate = this.automaticTestService.GetById(solvedTest.TestId).Rate;
            double grade = this.testService.CalculateMark(allPoints, testPoints, testRate);
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var level = this.courseLevelService.GetById(int.Parse(this.ControllerContext.HttpContext.Request.Cookies["levelId"].Value));
            this.markService.Add(
                new Mark
                {
                    CourseId = solvedTest.CourseId.Value,
                    MarkNum = grade,
                    StudentId = student.Id,
                    Date = DateTime.Now,
                    Description = "^c999d7d108874d94abbc95ffb2118a99^ " + level.Name
                });
            if (grade >= 3.00)
            {
                this.notificationService.Add(new Notification
                {
                    UserId = student.ApplicationUserId,
                    Content = "^ca779cae33834c0fbd4491b45e066343^ " + level.Name + "(" + solvedTest.Course.Name + "-" + grade + ")",
                    Link = "/Mark/Marks"

                });
                this.courseLevelService.AddCourseLevelToStudent(level.Id, student.Id);
                Response.Cookies.Clear();
                return View("ResultOfTest", null, new ResultViewModel { CorrectAnswers = testPoints, Grade = grade, Description = level.Name + " в курс " + level.Course.Name });
            }
            else
            {
                this.notificationService.Add(new Notification
                {
                    UserId = student.ApplicationUserId,
                    Content = "^d482501cd64540118d525ea74747a692^.(" + solvedTest.Course.Name + " - " + grade + ")",
                    Link = "/Mark/Marks"

                });
                Response.Cookies.Clear();
                return View("NotAccessResult", null, new ResultViewModel { CorrectAnswers = testPoints, Grade = grade, Description = level.Name + " в курс " + level.Course.Name });
            }
        }
    }
}