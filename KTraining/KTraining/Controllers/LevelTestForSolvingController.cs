using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;

namespace KTraining.Controllers
{
    [Authorize(Roles="Student")]
    public class LevelTestForSolvingController : BaseController
    {
        // GET: LevelTestForSolving
        public ActionResult Index()
        {
            var student=this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var levelTests = this.levelTestForSolvingService.GetTestsForStudent(student.Id);
            var viewModel = levelTests.ToList().ConvertAll(x => new LevelTestForSolvingViewModel
                {
                    CourseId=x.CourseLevel.CourseId,
                    CourseName=x.CourseLevel.Course.Name,
                    Id=x.Id,
                    LevelName=x.CourseLevel.Name,
                    TestName = x.CourseLevel.ManualTestId.HasValue ? x.CourseLevel.ManualTest.Title : x.CourseLevel.AutomaticTest.Title

                });
            foreach(var item in levelTests)
            {
                if(item.CourseLevel.AutomaticTestId.HasValue)
                {
                    var level=viewModel.Where(x => x.Id == item.Id).First();
                    level.IsManual = false;
                }
                else if (item.CourseLevel.ManualTestId.HasValue)
                {
                    var level = viewModel.Where(x => x.Id == item.Id).First();
                    level.IsManual = true;
                }

            }
            return View(viewModel);
        }
    }
}   