using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;
using KTreining.Model;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class CheckManualTestForLevelController : BaseController
    {
        // GET: CheckManualTest
        [HttpGet]
        public ActionResult Tests()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var tests = this.solvedManualTestForLevelService.GetForTeacher(teacher.Id)
                .OrderBy(x => x.CourseLevel.Course.Name)
                .ToList().ConvertAll(x =>
                new SolvedManualTestShowViewModel
                {
                    CourseName = x.CourseLevel.Course.Name,
                    CourseId = x.CourseLevel.CourseId,
                    StudentId = x.Student.ApplicationUserId,
                    Id = x.Id,
                    Name = x.Test.Title,
                    StudentName = x.Student.ApplicationUser.FirstName + " " +
                                   x.Student.ApplicationUser.SecondName + " " +
                                   x.Student.ApplicationUser.LastName
                });
            return View(tests);
        }

        [HttpGet]
        public JsonResult CountTests()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var tests = this.solvedManualTestForLevelService.GetForTeacher(teacher.Id)
                .OrderBy(x => x.CourseLevel.Course.Name)
                .ToList();
            return Json(tests.Count, JsonRequestBehavior.AllowGet);
        }

        //GET: Check test
        [HttpGet]
        public ActionResult CheckTest(int id)
        {
            if (BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                var test = this.solvedManualTestForLevelService.GetById(id);
                var viewModel = new SolvedManualTestCheckViewModel
                {
                    Id = test.Id,
                    Student = test.Student,
                    MaxPoints = this.manualTestService.MaxPoints(test.Test.Id),
                    SolvedCloseQuestions = test.SolvedCloseQuestions.ToList(),
                    SolvedOpenQuestions = test.SolvedOpenQuestions.ToList(),
                    TestTitle = test.Test.Title,
                    Rate = test.Test.Rate,
                    CourseLevelId = test.CourseLevelId.Value
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
            return Redirect("/CheckManualTestForLevel/Tests");
        }

        //POST:Check tests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckTest(SolvedManualTestCheckViewModel model)
        {
            if (!ModelState.IsValid)
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
            var solvedTest = this.solvedManualTestForLevelService.GetById(model.Id);
            double yourPoints = this.solvedManualTestForLevelService.GetPointForTest(solvedTest.Id);
            double allPoints = this.manualTestService.MaxPoints(solvedTest.TestId);
            var testRate = solvedTest.Test.Rate;
            double mark = this.testService.CalculateMark(allPoints, yourPoints, testRate);
            var level = this.courseLevelService.GetById(model.CourseLevelId);
            this.markService.Add(new Mark
            {
                CourseId = solvedTest.CourseLevel.CourseId,
                Date = DateTime.Now,
                MarkNum = mark,
                StudentId = model.Student.Id,
                Description = "^c999d7d108874d94abbc95ffb2118a99^ " + level.Name
            });
            if (mark >= 3.00)
            {
                this.notificationService.Add(new Notification
                {
                    UserId = model.Student.ApplicationUserId,
                    Content = "^ca779cae33834c0fbd4491b45e066343^" + level.Name + "(" + solvedTest.CourseLevel.Course.Name + "-" + mark + ")",
                    Link = "/Mark/Marks"

                });
                this.courseLevelService.AddCourseLevelToStudent(level.Id, model.Student.Id);
            }
            else
            {
                this.notificationService.Add(new Notification
                {
                    UserId = model.Student.ApplicationUserId,
                    Content = "^d482501cd64540118d525ea74747a692^(" + solvedTest.CourseLevel.Course.Name + " - " + mark + ")",
                    Link = "/Mark/Marks"

                });
            }
            Response.Cookies.Clear();
            this.solvedManualTestForLevelService.Remove(solvedTest.Id);
            return Redirect("/CheckManualTestForLevel/Tests");
        }

        //Check if the solved manual test belongs to current user
        private bool BellongsToCurrentUser(int id, string userId)
        {
            return this.solvedManualTestForLevelService.GetById(id).CourseLevel.Course.Teacher.ApplicationUserId == userId;
        }
    }
}