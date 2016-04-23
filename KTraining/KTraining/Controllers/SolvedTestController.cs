using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Student")]
    public class SolvedTestController : BaseController
    {
        // GET: SolvedTest
        [HttpGet]
        public ActionResult SolvedTests()
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var solvedTests = new SolvedTestViewModel
            {
                AutomaticSolvedTests = student.SolvedAutomaticTests
                .Where(y => y.Show == true)
                .Reverse()
                .ToList().ConvertAll(x =>
                new SolvedAutomaticTestShowViewModel
                {
                    CourseName = x.Course.Name,
                    Id = x.Id,
                    Name = x.Test.Title,
                    CourseId = x.CourseId.Value
                }),
                ManualSolvedTest = student.SolvedManualTests
                .Where(y => y.IsChecked == true)
                .Reverse()
                .ToList().ConvertAll(x =>
                new SolvedManualTestShowViewModel
                {
                    CourseName = x.Course.Name,
                    Id = x.Id,
                    Name = x.Test.Title,
                    CourseId = x.CourseId.Value
                })
            };
            return View(solvedTests);
        }

        //GET:Get Solved automatic test
        [HttpGet]
        public ActionResult SolvedAutomaticTest(int id)
        {
            var solvedTest = this.solvedAutomaticTestService.GetById(id);
            if (solvedTest.Student.ApplicationUserId == this.User.Identity.GetUserId())
            {
                double testPoints = this.solvedAutomaticTestService.GetPointForTest(id);
                double allPoints = this.automaticTestService.GetPointsForTest(solvedTest.TestId);
                var testRate = this.automaticTestService.GetById(solvedTest.TestId).Rate;
                double grade = this.testService.CalculateMark(allPoints, testPoints, testRate);
                var viewModel = new SolvedAutoTestFullViewModel
                {
                    CourseName = solvedTest.Course.Name,
                    Rate = solvedTest.Test.Rate,
                    TestTime = solvedTest.Test.Time,
                    Questions = solvedTest.SolvedQuestions.ToList(),
                    Mark = grade,
                    TestTitle = solvedTest.Test.Title,
                    CourseId = solvedTest.Course.Id
                };
                foreach (var item in viewModel.Questions)
                {
                    item.CloseQuestion.Images = this.cloudinaryService.AddPathToQuestionImageName(item.CloseQuestion.Images);
                    foreach (var q in item.CloseQuestion.Answers)
                    {
                        q.Images = this.cloudinaryService.AddPathToQuestionImageName(q.Images);
                    }
                }
                return View(viewModel);
            }
            return Redirect("/");
        }

        //GET:Get Solved automatic test
        [HttpGet]
        public ActionResult SolvedManualTest(int id)
        {
            var solvedTest = this.solvedManualTestService.GetById(id);
            if (solvedTest.Student.ApplicationUserId == this.User.Identity.GetUserId())
            {
                var viewModel = new SolvedManualTestFullViewModel
                {
                    CourseName = solvedTest.Course.Name,
                    Rate = solvedTest.Test.Rate,
                    TestTime = solvedTest.Test.Time,
                    CloseQuestions = solvedTest.SolvedCloseQuestions.ToList(),
                    OpenQuestions = solvedTest.SolvedOpenQuestions.ToList(),
                    Mark = solvedTest.Mark,
                    TestTitle = solvedTest.Test.Title,
                    CourseId = solvedTest.Course.Id
                };
                foreach (var item in viewModel.CloseQuestions)
                {
                    item.CloseQuestion.Images = this.cloudinaryService.AddPathToQuestionImageName(item.CloseQuestion.Images);
                    foreach (var q in item.CloseQuestion.Answers)
                    {
                        q.Images = this.cloudinaryService.AddPathToQuestionImageName(q.Images);
                    }
                }
                foreach (var item in viewModel.OpenQuestions)
                {
                    item.OpenQuestion.Images = this.cloudinaryService.AddPathToQuestionImageName(item.OpenQuestion.Images);
                }
                return View(viewModel);
            }
            return Redirect("/");
        }

    }
}