using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;
using KTreining.Model;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize(Roles="Teacher")]
    public class ExaminationForLevelController : BaseController
    {
        // GET: ExaminationForLevel
        [HttpGet]
        public ActionResult SelectCourse()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var courses = this.courseService.GetCoursesForTeacher(teacher.Id);
            var viewModel = courses.ToList().ConvertAll(x => new CourseViewModel
                {
                    Id=x.Id,
                    Name=x.Name
                });
            return View(viewModel);
        }

        //GET: Select level for examination
        [HttpGet]
        public ActionResult SelectLevel(int id)
        {

            if (this.BellongToCurrentUserCourse(id))
            {
                var levels = this.courseLevelService.GetForCourse(id);
                var viewModel = levels.ToList().ConvertAll(x => new CourseLevelViewModel
                {
                    CourseId = x.CourseId,
                    Id = x.Id,
                    Name = x.Name
                });
                return View(viewModel);
            }
            return Redirect("/");
        }

        //GET: Select students for examination
        [HttpGet]
        public ActionResult SelectStudents(int id)
        {
            if (this.BellongToCurrentUser(id))
            {
                var courseId = this.courseLevelService.GetById(id).CourseId;
                var students = this.studentService.GetStudentsWithoutCourseLevelForCourse(id, courseId);

                var convertedStudents = students.ToList().ConvertAll(x =>
                    new StudentViewModel
                    {
                        FullName = x.ApplicationUser.FirstName + " " +
                                 x.ApplicationUser.SecondName + " " +
                                 x.ApplicationUser.LastName,
                        Id = x.Id,
                        IsChecked = false
                    }).ToList();
                var viewModel = new ExaminationForLevelViewModel
                {
                    LevelId = id,
                    Students = convertedStudents
                };
                return View(viewModel);
            }
            return Redirect("/");
        }

        //POST: Send test
        [HttpPost]
        public ActionResult SendTest(ExaminationForLevelViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("SelectStudents",model);
            }
            if(model.Students.Where(x=>x.IsChecked==false).Count()==model.Students.Count)
            {
                this.ModelState.AddModelError("", Common.ChooseOneStudent);
                return View("SelectStudents", model);
            }
            foreach (var item in model.Students)
            {
                if(item.IsChecked)
                {
                    if (!this.levelTestForSolvingService.HasTest(item.Id, model.LevelId))
                    {
                        this.levelTestForSolvingService.Add(new LevelTestForSolving
                            {
                                CourseLevelId = model.LevelId,
                                StudentId = item.Id
                            });
                    }
                    var student=this.studentService.GetById(item.Id);
                    this.notificationService.Add(new Notification
                    {
                        UserId = student.ApplicationUserId,
                        Content = "^d3357bfa357649c2859fd8041a445ef4^",
                        Link = "/LevelTestForSolving"

                    });
                }
            }
            return Redirect("/");
        }

        //Check if the level belongs to current user
        private bool BellongToCurrentUser(int id)
        {
            return this.courseLevelService.GetById(id).Course.Teacher.ApplicationUserId == this.User.Identity.GetUserId();
        }

        //Check if the course belongs to current user
        private bool BellongToCurrentUserCourse(int id)
        {
            return this.courseService.GetById(id).Teacher.ApplicationUserId == this.User.Identity.GetUserId();
        }
    }
}