using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTreining.Model;
using KTraining.Models;

namespace KTraining.Controllers
{
    [Authorize]
    public class RequestController : BaseController
    {
        //POST: Send request to join in course
        [HttpPost]
        [Authorize(Roles = "Student")]
        public ActionResult SendRequestToJoin(int id)
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            this.requestToJoinService.Add(new RequestToJoin
            {
                CourseId = id,
                SendById = student.Id
            });
            var course = courseService.GetById(id);
            this.notificationService.Add(new Notification
            {
                UserId = course.Teacher.ApplicationUserId,
                Content = "^b3f1a68fbceb498497a5c21b6170baf5^",
                Link = "/Request/RequestsToJoin"

            });
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //GET:Get teacher's requests to joining in course
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult RequestsToJoin()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var requests = this.requestToJoinService.GetRequestsForTeacher(teacher.Id)
                .OrderBy(x => x.CourseId)
                .ToList()
                .ConvertAll(
                x => new RequestToJoinViewModel
                {
                    Course = x.Course,
                    Id = x.Id,
                    SendBy = x.SendBy
                });

            return View(requests);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public JsonResult CountRequestsToJoin()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var requests = this.requestToJoinService.GetRequestsForTeacher(teacher.Id)
                .OrderBy(x => x.CourseId)
                .ToList();
            return Json(requests.Count, JsonRequestBehavior.AllowGet);
        }

        //POST: Accept request
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptRequest(int id)
        {
            var request = this.requestToJoinService.GetById(id);
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            this.courseService.AddStudentToCourse(request.CourseId, request.SendById);
            this.postService.Add(new Post
            {
                Content = teacher.ApplicationUser.FirstName + " " + teacher.ApplicationUser.SecondName + " " +
                teacher.ApplicationUser.LastName + " ^t4c66efaa35043beaaeb5f7e7c6a1ed1^ " + request.SendBy.ApplicationUser.FirstName + " " +
                request.SendBy.ApplicationUser.SecondName + " " + request.SendBy.ApplicationUser.LastName + " ^bb81ab8cfb1745708af8ab41b4b74368^",
                CourseId = request.CourseId,
                UserId = this.User.Identity.GetUserId(),
                Date = DateTime.Now

            });
            this.notificationService.Add(new Notification
            {
                UserId = request.SendBy.ApplicationUserId,
                Content = "^b74c4efaccfa44f3ba51d834c257ae16^ " + request.Course.Name,
                Link = "/CourseDetails/Details/" + request.CourseId

            });
            this.requestToJoinService.Remove(request.Id);
            return RedirectToAction("RequestsToJoin");
        }

        //POST: Decline request
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeclineRequest(int id)
        {
            this.requestToJoinService.Remove(id);
            return RedirectToAction("RequestsToJoin");
        }
    }
}