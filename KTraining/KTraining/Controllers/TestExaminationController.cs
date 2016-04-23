using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;
using KTreining.Model;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TestExaminationController : BaseController
    {
        // POST: Get teacher's courses
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCourses(int testId, string type)
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var courses = this.courseService.GetCoursesForTeacher(teacher.Id);
            var viewModel = new CoursesForExaminationViewModel
            {
                Courses = courses.ToList().ConvertAll(x => new CourseViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }),
                TestId = testId,
                Type = type
            };
            return View(viewModel);
        }

        //GET: Select test
        [HttpGet]
        public ActionResult SelectTest()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());

            var viewModel = new SelectTestViewModel
            {
                AutomaticTests = teacher.AutomaticTests.ToList(),
                ManualTests = teacher.ManualTests.ToList()
            };
            return View(viewModel);
        }

        //POST: Send test to students
        [HttpPost]
        public ActionResult SendTest(int[] coursesIds, int testId, string type)
        {
            if (coursesIds.Count() == 0 || testId == 0 || type == null)
            {
                return null;
            }
            foreach (var item in coursesIds)
            {
                var course = this.courseService.GetById(item);
                foreach (var st in course.Students)
                {
                    this.studentService.AddTestToSolve(st.Id, testId, type, item);
                    this.notificationService.Add(new Notification
                    {
                        UserId = st.ApplicationUserId,
                        Content = "^f797056e86c24aaa9d603a92366d2526^",
                        Link = "/Test/UnsolvedTests"

                    });
                }
            }
            return null;
        }
    }
}