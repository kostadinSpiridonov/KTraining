using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;
using KTreining.Model;
using KTraining.Service;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    public class CourseController : BaseController
    {
        // GET: Get All Courses
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult GetAllCourses()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var courses = teacher.Courses.ToList()
                .Where(x => x.IsComplete == false)
                .ToList()
                .ConvertAll(x => new CourseViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return View(courses);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult CompletedTeacherCourses()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var courses = teacher.Courses.ToList()
                .Where(x => x.IsComplete == true)
                .ToList()
                .ConvertAll(x => new CourseViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return View(courses);
        }

        // GET: CourseTeacher/Create
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseTeacher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(CreateCourseViewModel model)
        {
            if (model.Description!=null&&OtherFunctions.IsHasJS(model.Description))
            {
                ModelState.AddModelError("", Common.FieldDescriptionDanger);
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                this.courseService.Add(new Course
                    {
                        Name = model.Name,
                        TeacherId = teacher.Id,
                        Descrition = model.Description,
                        CreationDate = DateTime.Now
                    });

                return Redirect("/Course/GetAllCourses");
            }
            catch
            {
                return View();
            }
        }

        // GET: CourseTeacher/Delete/5
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int id)
        {
            if (BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                try
                {
                    this.courseService.Remove(id);
                    return Redirect("/Course/GetAllCourses");
                }
                catch
                {
                    TempData["DeleteCourseError"] = Common.SomeoneUseThisCourse;
                    return Redirect("/CourseDetails/Details/" + id);
                }
            }
            return Redirect("/");
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult Complete(int id)
        {
            if (BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                this.courseService.Complete(id);
                return Redirect("/Course/GetAllCourses");
            }
            return Redirect("/");
        }

        // GET: Get All Courses
        [Authorize(Roles = "Student")]
        public ActionResult StudentCourses()
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var courses = student.Courses
                .ToList()
                .ConvertAll(x => new CourseDetails
                {
                    Id = x.Id,
                    CourseName = x.Name,
                    Teacher = x.Teacher
                });

            return View(courses);
        }

        //GET: Get latest courses
        [HttpGet]
        public ActionResult LatestCourses()
        {
            var courses = this.courseService.LatestTenCourses();
            var viewModel = courses.ToList().ConvertAll(x =>
                new CourseViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            return PartialView(viewModel);
        }

        //GET: Get most famous courses
        [HttpGet]
        public ActionResult MostFamous()
        {
            var courses = this.courseService.MostFamous();
            var viewModel = courses.ToList().ConvertAll(x =>
                new CourseViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            return PartialView(viewModel);
        }

        //GET: Get completed courses for student
        [HttpGet]
        [Authorize(Roles = "Student")]
        public ActionResult CompletedStudentCourses()
        {
            var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
            var courses = student.CompletedCourses.ToList()
                .ToList()
                .ConvertAll(x => new CompletedCourseViewModel
                {
                    Name = x.Course.Name,
                    CourseMark = x.Mark,
                    CourseId = x.CourseId
                });

            return View(courses);
        }

        //GET: Get all courses
        [HttpGet]
        [Authorize(Roles = "Teacher,Student")]
        public ActionResult Courses()
        {
            List<Course> courses = new List<Course>();
            courses = this.courseService.GetAll()
                .Where(x => x.IsComplete == false)
                .ToList();

            var coursesPageCount = courses.Count % 10 == 0 ? courses.Count / 10 : courses.Count / 10 + 1;
            if (coursesPageCount == 0)
            {
                coursesPageCount = 1;
            }

            return View(coursesPageCount);
        }

        //GET: Edit course
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int id)
        {
            if (BellongsToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                var model = this.courseService.GetById(id);
                var viewmodel = new CourseEditViewModel
                {
                    Name = model.Name,
                    Id = model.Id,
                    Description = model.Descrition

                };
                return View(viewmodel);
            }
            return Redirect("/");
        }

        //POST: Edit course
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(CourseEditViewModel model)
        {
            if (model.Description != null && model.Description.Contains("lt;script&gt;"))
            {
                ModelState.AddModelError("", Common.FieldDescriptionDanger);
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            this.courseService.Update(
            new Course
            {
                Name = model.Name,
                Descrition = model.Description,
                Id = model.Id
            });
            return Redirect("/CourseDetails/Details/" + model.Id);
        }
        
        //Check if the course bellongs to current user
        private bool BellongsToCurrentUser(int id,string userId)
        {
            return this.courseService.GetById(id).Teacher.ApplicationUserId == userId;
        }
    }
}
