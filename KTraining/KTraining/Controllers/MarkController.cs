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
    public class MarkController : BaseController
    {
        // GET: Marks 
        [HttpGet]
        public ActionResult Marks()
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var marks = this.markService.MarksForStudent(student.Id)
                .OrderByDescending(x=>x.Date)
                .ToList()
                .ConvertAll(x => new ShowMarkViewModel
                {
                    CourseName = x.Course.Name,
                    Date = x.Date.ToShortDateString(),
                    MarkValue = x.MarkNum,
                    CourseId = x.CourseId,
                    Descrition=this.convertResource.ConvertContentCode(x.Description)
                }).ToList();
            this.markService.SetSeenStudentMarks(student.Id);
            return View(marks);
        }

        //GET: Get count of unseen marks
        [HttpGet]
        public JsonResult CountUnseenMarks()
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var count=this.markService.GetUnseenMarks(student.Id);
            return Json(count,JsonRequestBehavior.AllowGet);
        }
    
    }
}