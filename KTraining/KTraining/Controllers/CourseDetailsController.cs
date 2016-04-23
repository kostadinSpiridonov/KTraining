using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;

namespace KTraining.Controllers
{
    public class CourseDetailsController : BaseController
    {
        // GET: Course details 
        [HttpGet]
        public ActionResult Details(int id)
        {
            var course = this.courseService.GetById(id);
            var viewModel = new CourseDetails
            {
                Id = course.Id,
                Teacher = course.Teacher,
                Description=course.Descrition,
                CourseName=course.Name,
                CountParticipants=course.Students.Count
                
            };
            if(course.IsComplete)
            {
                if(this.User.Identity.IsAuthenticated)
                {
                    return View("DetailsCompleted", viewModel);
                }
            }
            if (this.User.IsInRole("Student"))
            {
                var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
                var courses = student.Courses;
                if (courses.Any(x => x.Id == id))
                {
                    return View("DetailsStudent", viewModel);
                }
                return View(viewModel);
            }
            else if (this.User.IsInRole("Teacher"))
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var courses = this.courseService.GetCoursesForTeacher(teacher.Id);
                if (courses.Any(x => x.Id == id))
                {
                    return View("DetailsTeacher",viewModel);
                }
                return View(viewModel);
            }
            return View(viewModel);


        }

    }
}