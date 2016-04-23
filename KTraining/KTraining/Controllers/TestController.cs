using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;

namespace KTraining.Controllers
{
    public class TestController : BaseController
    {
        // GET: Type test
        [Authorize(Roles = "Teacher")]
        public ActionResult Type()
        {
            return View();
        }

        //GET: Unsolved tests for student
        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult UnsolvedTests()
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var viewModel = new UnsolvedTestsViewModel
            {
                AutomaticTests = student.AutomaticTestsToSolve.ToList().OrderBy(x => x.Course.Name).ToList(),
                ManualTests = student.ManualTestsToSolve.ToList().OrderBy(x => x.Course.Name).ToList()
            };
            return View(viewModel);
        }

        //GET: Get count of unsolved tests
        [Authorize(Roles = "Student")]
        [HttpGet]
        public JsonResult CountUnsolvedTests()
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var count = student.AutomaticTestsToSolve.Count + student.ManualTestsToSolve.Count;
            return Json(count, JsonRequestBehavior.AllowGet);
        }

    }
}