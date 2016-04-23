using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using KTraining.Models;
using KTreining.Model;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize]
    public class CourseParticipantsController : BaseController
    {
        // GET: Course Participants
        [HttpGet]
        public ActionResult Participants(int id)
        {
            var course = this.courseService.GetById(id);
            var viewModel = new CourseParticipants
            {
                Students = course.Students.ToList(),
                Id = course.Id,
                CourseName=course.Name
            };
            if (this.User.IsInRole("Student"))
            {
                var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
                var courses = student.Courses;
                if (courses.Any(x => x.Id == id))
                {
                    return View("ParticipantsStudent",viewModel);
                }
                 return View("Participants",viewModel);
            }
            else if (this.User.IsInRole("Teacher"))
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var courses = this.courseService.GetCoursesForTeacher(teacher.Id);
                if (courses.Any(x => x.Id == id))
                {

                    var usernames = this.studentService.GetStudentsWithoutCourse(id)
                        .ToList()
                         .Select(x => (x.ApplicationUser.FirstName + " " + x.ApplicationUser.SecondName + " " + x.ApplicationUser.LastName + " (" + x.ApplicationUser.Email + ")"));
                    var serializedUsernames = JsonConvert.SerializeObject(usernames);
                    viewModel.UsernamesJson = serializedUsernames;
                    return View("ParticipantsTeacher",viewModel);
                }
                return View("Participants",viewModel);

            }
            return View("Participants",viewModel);
        }

        // POST: Add Student to course 
        [Authorize(Roles="Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(AddStudentToCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AddUserError"] = Common.NoUser;
                return Redirect("/CourseParticipants/Participants/" + model.CourseId);
            }
            var email = model.Username.Substring(model.Username.IndexOf("(") + 1, model.Username.LastIndexOf(")") - model.Username.IndexOf("(") - 1);
            var student = this.studentService.GetStudentByEmail(email);
            var studentName = student.ApplicationUser.FirstName + " " + student.ApplicationUser.SecondName + " " + student.ApplicationUser.LastName;
            this.courseService.AddStudentToCourse(model.CourseId, student.Id);
            this.postService.Add(new Post
            {
                Content = "^t4c66efaa35043beaaeb5f7e7c6a1ed1^ " + studentName + " ^bb81ab8cfb1745708af8ab41b4b74368^",
                CourseId = model.CourseId,
                UserId = this.User.Identity.GetUserId(),
                Date = DateTime.Now

            });
            return Redirect("/CourseParticipants/Participants/" + model.CourseId);
        }

        //POST: Remove student from course
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveStudent(int courseId, int studentId)
        {
            if (BellongToCurrentUSer(courseId, this.User.Identity.GetUserId()))
            {
                if (courseId == 0 || studentId == 0)
                {
                    return Redirect("/CourseParticipants/Participants/" + courseId);
                }
                var student = this.studentService.GetById(studentId);
                var studentName = student.ApplicationUser.FirstName + " " + student.ApplicationUser.SecondName + " " + student.ApplicationUser.LastName;
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var teacherName = teacher.ApplicationUser.FirstName + " " + teacher.ApplicationUser.SecondName + " " + teacher.ApplicationUser.LastName;
                this.courseService.RemoveStudent(courseId, studentId);
                this.postService.Add(new Post
                {
                    Content = teacherName + " ^ab81e1b26160402e86c134cc4bc53f55^ " + studentName + " ^d1b83438729e43b886f48920a0478a0f^",
                    CourseId = courseId,
                    UserId = this.User.Identity.GetUserId(),
                    Date = DateTime.Now

                });
                return Redirect("/CourseParticipants/Participants/" + courseId);
            }
            return Redirect("/");

        }

        //Check if the course belongs to current user
        private bool BellongToCurrentUSer(int id,string userId)
        {
            return this.courseService.GetById(id).Teacher.ApplicationUserId == userId;
        }
    }
}