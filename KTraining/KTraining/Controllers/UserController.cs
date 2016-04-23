using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Models;
using KTreining.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using KTraining.Data;
using KTraining.Service;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    public class UserController : BaseController
    {
        private RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> _roleManager;

        public RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> RoleManager
        {
            get
            {
                return _roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            }
            private set
            {
                _roleManager = value;
            }
        }
        
        // GET: User
        public ActionResult UserDetailsEdit()
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.userService.GetAppUser(userId);
            var viewModel = new UserProfileViewModel
            {
                AboutMe = user.AboutMe,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SecondName = user.SecondName,
                PhoneNumber = user.PhoneNumber,
                Skype = user.Skype,
                Id = user.Id,
                Role = RoleManager.FindById(user.Roles.First().RoleId).Name
            };
            return View(viewModel);
        }

        //POST: Edit user
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UserDetailsEdit(UserProfileViewModel model)
        {
            try
            {
                if (OtherFunctions.IsHasJS(model.AboutMe))
                {
                    ModelState.AddModelError("", Common.FieldAboutMeDanger);
                    return View(model);
                }
            }
            catch { }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser
            {
                Id = model.Id,
                AboutMe = model.AboutMe,
                City = model.City,
                Country = model.Country,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                SecondName = model.SecondName,
                Skype = model.Skype
            };
            this.userService.UpdateAppUser(user);
            return Redirect("/User/UserDetails/" + user.Id);
        }

        //GET: User details
        public ActionResult UserDetails(string id)
        {
            var user = this.userService.GetAppUser(id);
            var viewModel = new UserProfileViewModel
            {
                AboutMe = user.AboutMe,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SecondName = user.SecondName,
                PhoneNumber = user.PhoneNumber,
                Skype = user.Skype,
                Role = RoleManager.FindById(user.Roles.First().RoleId).Name,
                Id = user.Id
            };
            if (viewModel.Role == "Student")
            {
                var student = this.studentService.GetStudentByAppUserId(id);
                viewModel.CompleteCourses = student.CompletedCourses.ToList().ConvertAll(x =>
                    new CourseViewModel
                    {
                        Id = x.CourseId,
                        Name = x.Course.Name
                    });
                viewModel.Role = Common.Student;
                var levels = student.CourseLevels.OrderBy(x => x.Course.Name);
                var levelsVm = new List<CourseLevelsViewModel>();
                foreach (var item in levels)
                {
                    if (levelsVm.Where(x => x.CourseId == item.CourseId).Count() > 0)
                    {
                        levelsVm.Where(x => x.CourseId == item.CourseId).First().Levels.Add(
                            new CourseLevelViewModel
                            {
                                CourseId = item.CourseId,
                                Id = item.Id,
                                Name = item.Name
                            });
                    }
                    else
                    {
                        levelsVm.Add(new CourseLevelsViewModel
                        {
                            CourseId = item.CourseId,
                            CourseName = item.Course.Name,
                            Levels = new List<CourseLevelViewModel> 
                        {
                            new CourseLevelViewModel 
                            {
                               CourseId=item.CourseId,
                               Id=item.Id,
                               Name=item.Name
                            }
                        }
                        });
                    }
                }
                viewModel.CourseLevels = levelsVm;
            }
            else if (viewModel.Role == "Teacher")
            {
                var teacher = this.userService.GetTeacherByAppUserId(id);
                viewModel.CompleteCourses = teacher.Courses.Where(x => x.IsComplete == false).ToList().ConvertAll(x =>
                    new CourseViewModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    });
                viewModel.Role = Common.Leader;
            }
            return View(viewModel);
        }

        //GET: Get first user name
        [Authorize]
        public JsonResult FirstUserName()
        {
            var name = this.userService.GetAppUser(this.User.Identity.GetUserId()).FirstName;
            return Json(name, JsonRequestBehavior.AllowGet);
        }
    }
}