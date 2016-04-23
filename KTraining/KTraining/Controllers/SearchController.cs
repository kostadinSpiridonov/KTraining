using KTraining.Models;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace KTraining.Controllers
{
    public class SearchController : BaseController
    {
        //GET: Search course
        [HttpGet]
        public ActionResult Search(string q)
        {
            if (String.IsNullOrEmpty(q))
            {
                q = "";
            }
            q = q.ToLower();
            var latin = this.alphabetFunctions.ConvertCyrilicToLatin(q);
            var cyrilic = this.alphabetFunctions.ConvertLatinToCyrylic(q);
            List<Course> courses = new List<Course>();
            courses = this.courseService.GetAll()
                .Where(x => x.IsComplete == false)
                .Where(x => x.Name.ToLower().Contains(cyrilic) ||
                    x.Name.ToLower().Contains(latin))
                .ToList();

            List<ApplicationUser> appUsers = new List<ApplicationUser>();
            appUsers = this.userService.GetAllAppUsers()
                .Where(x => x.FirstName.ToLower().Contains(cyrilic) ||
                       x.SecondName.ToLower().Contains(cyrilic) ||
                       x.LastName.ToLower().Contains(cyrilic) ||
                       x.FirstName.ToLower().Contains(latin) ||
                       x.SecondName.ToLower().Contains(latin) ||
                       x.LastName.ToLower().Contains(latin)).ToList();
            var currentUser = appUsers.Find(x => x.Id == this.User.Identity.GetUserId());
            appUsers.Remove(currentUser);

            var coursesPageCount = courses.Count % 10 == 0 ? courses.Count / 10 : courses.Count / 10 + 1;
            var usersPageCount = appUsers.Count % 15 == 0 ? appUsers.Count / 15 : appUsers.Count / 15 + 1;
            if (coursesPageCount == 0)
            {
                coursesPageCount = 1;
            }
            if (usersPageCount == 0)
            {
                usersPageCount = 1;
            }
            var viewModel = new SearchViewModel
            {
                SearchWord = q,
                CoursesPageCount = coursesPageCount,
                UsersPageCount = usersPageCount
            };
            return View(viewModel);
        }

        //GET: Search user
        [HttpGet]
        public ActionResult SearchUser(string q, int page)
        {
            var viewModel = new List<UserSearchViewModel>();
            if (String.IsNullOrEmpty(q))
            {
                q = "";
            }
            q = q.ToLower();
            var latin = this.alphabetFunctions.ConvertCyrilicToLatin(q);
            var cyrilic = this.alphabetFunctions.ConvertLatinToCyrylic(q);
            List<ApplicationUser> appUsers = new List<ApplicationUser>();
            appUsers = this.userService.GetAllAppUsers()
                .Where(x => x.FirstName.ToLower().Contains(cyrilic) ||
                       x.SecondName.ToLower().Contains(cyrilic) ||
                       x.LastName.ToLower().Contains(cyrilic) ||
                       x.FirstName.ToLower().Contains(latin) ||
                       x.SecondName.ToLower().Contains(latin) ||
                       x.LastName.ToLower().Contains(latin))
                 .OrderBy(x => x.FirstName)
                 .ThenBy(x => x.SecondName)
                 .ThenBy(x => x.LastName)
                 .ToList();
            var currentUser = appUsers.Find(x => x.Id == this.User.Identity.GetUserId());
            appUsers.Remove(currentUser);
            viewModel = appUsers.ConvertAll(x =>
                new UserSearchViewModel
                {
                    Id = x.Id,
                    FullName = x.FirstName + " " +
                             x.SecondName + " " +
                             x.LastName
                }).GetRange((page - 1) * 15, appUsers.Count - (page - 1) * 15 < 15 ? appUsers.Count - (page - 1) * 15 : 15);
            return PartialView("_SearchUser", viewModel);
        }

        //GET: Search course
        [HttpGet]
        public ActionResult SearchCourse(string q, int page)
        {
            var viewModel = new List<CourseSearchViewModel>();
            if (String.IsNullOrEmpty(q))
            {
                q = "";
            }
            q = q.ToLower();
            var latin = this.alphabetFunctions.ConvertCyrilicToLatin(q);
            var cyrilic = this.alphabetFunctions.ConvertLatinToCyrylic(q);
            List<Course> courses = new List<Course>();
            courses = this.courseService.GetAll()
                .Where(x => x.IsComplete == false)
                .Where(x => x.Name.ToLower().Contains(cyrilic) ||
                    x.Name.ToLower().Contains(latin))
                .OrderBy(x => x.Name)
                .ToList();
            viewModel = courses.ConvertAll(x =>
                new CourseSearchViewModel
                {
                    Name = x.Name,
                    Id = x.Id,
                    TeacherName = x.Teacher.ApplicationUser.FirstName + " " + x.Teacher.ApplicationUser.SecondName + " " + x.Teacher.ApplicationUser.LastName,
                    TeacherAppId = x.Teacher.ApplicationUserId
                }).GetRange((page - 1) * 10, courses.Count - (page - 1) * 10 < 10 ? courses.Count - (page - 1) * 10 : 10);
            if (User.IsInRole("Student"))
            {
                var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
                foreach (var item in viewModel)
                {
                    if (this.courseService.IfStudentParticipate(item.Id, student.Id))
                    {
                        item.Relation = "pr";
                    }
                    else if (this.requestToJoinService.IfStudentSendRequestToCourse(item.Id, student.Id))
                    {
                        item.Relation = "sr";
                    }
                    else
                    {
                        item.Relation = "np";
                    }
                }
            }
            return PartialView("_SearchCourse", viewModel);
        }

    }
}