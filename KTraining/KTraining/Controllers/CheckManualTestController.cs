using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;
using KTreining.Model;

namespace KTraining.Controllers
{
    [Authorize(Roles="Teacher")]
    public class CheckManualTestController : BaseController
    {
        // GET: CheckManualTest
        [HttpGet]
        public ActionResult Tests()
        {
            var teacher=this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var tests = this.solvedManualTestService.GetForTeacher(teacher.Id)
                .Where(x=>x.IsChecked==false)
                .OrderBy(x=>x.Course.Name)
                .ToList().ConvertAll(x =>
                new SolvedManualTestShowViewModel
                {
                     CourseName=x.Course.Name,
                     CourseId=x.CourseId.Value,
                     StudentId=x.Student.ApplicationUserId,
                     Id=x.Id,
                     Name=x.Test.Title,
                     StudentName = x.Student.ApplicationUser.FirstName + " "+
                                    x.Student.ApplicationUser.SecondName +" "+
                                    x.Student.ApplicationUser.LastName
                });
            return View(tests);
        }

        [HttpGet]
        public JsonResult CountTests()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var tests = this.solvedManualTestService.GetForTeacher(teacher.Id)
                .Where(x => x.IsChecked == false)
                .OrderBy(x => x.Course.Name)
                .ToList();
            return Json(tests.Count,JsonRequestBehavior.AllowGet);
        }

        //GET: Check test
        [HttpGet]
        public ActionResult CheckTest(int id)
        {
            if (BelongToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                var test = this.solvedManualTestService.GetById(id);
                var viewModel = new SolvedManualTestCheckViewModel
                {
                    Id = test.Id,
                    Student = test.Student,
                    MaxPoints = this.manualTestService.MaxPoints(test.Test.Id),
                    SolvedCloseQuestions = test.SolvedCloseQuestions.ToList(),
                    SolvedOpenQuestions = test.SolvedOpenQuestions.ToList(),
                    TestTitle = test.Test.Title,
                    Rate = test.Test.Rate
                };

                foreach (var item in viewModel.SolvedOpenQuestions)
                {
                    item.OpenQuestion.Images = this.cloudinaryService.AddPathToQuestionImageName(item.OpenQuestion.Images);
                }

                foreach (var item in viewModel.SolvedCloseQuestions)
                {
                    item.CloseQuestion.Images = this.cloudinaryService.AddPathToQuestionImageName(item.CloseQuestion.Images);
                    foreach (var an in item.CloseQuestion.Answers)
                    {
                        an.Images = this.cloudinaryService.AddPathToQuestionImageName(an.Images);
                    }
                }
                return View(viewModel);
            }
            return Redirect("/CheckManualTest/Tests");
        }

        //POST:Check tests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckTest(SolvedManualTestCheckViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.SolvedOpenQuestions != null)
            {
                foreach (var item in model.SolvedOpenQuestions)
                {
                    this.solvedOpenQuestionService.SetPoints(item.Id, item.Points);
                }
            }
            var solvedTest = this.solvedManualTestService.GetById(model.Id);
            double yourPoints = this.solvedManualTestService.GetPointForTest(solvedTest.Id);
            double allPoints = this.manualTestService.MaxPoints(solvedTest.TestId);
            var testRate = solvedTest.Test.Rate;
            double mark = this.testService.CalculateMark(allPoints, yourPoints, testRate);
            this.solvedManualTestService.SetMark(solvedTest.Id, mark);
            this.markService.Add(new Mark
                {
                    CourseId = solvedTest.CourseId.Value,
                    Date = DateTime.Now,
                    MarkNum = mark,
                    StudentId = model.Student.Id
                });
            this.notificationService.Add(new Notification
            {
                UserId = model.Student.ApplicationUserId,
                Content = "^d482501cd64540118d525ea74747a692^(" + solvedTest.Course.Name + " - " + mark + ")",
                Link = "/Mark/Marks"

            });
            this.solvedManualTestService.SetChecked(solvedTest.Id);
            return Redirect("/CheckManualTest/Tests");
        }
        
        //Check if the solved manual test belongs to current user
        private bool BelongToCurrentUser(int id,string userId)
        {
           return this.solvedManualTestService.GetById(id).Course.Teacher.ApplicationUserId == userId;
        }
    }
}