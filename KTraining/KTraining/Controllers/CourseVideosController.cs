using KTraining.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using KTreining.Model;
using System.Net;
using System.Collections.Specialized;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize]
    public class CourseVideosController : BaseController
    {
        // GET: CourseVideos
        [HttpGet]
        public ActionResult Videos(int id)
        {
            var viewModel = new CourseVideoViewModel() 
            {
                CourseId=id,
                Videos=this.videoService.GetVideosForCourse(id).ToList(),
                CourseName=this.courseService.GetById(id).Name
            };
            viewModel.Videos.Reverse();
            if (this.courseService.IsComplete(id))
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    return View("VideosCompleted", viewModel);
                }
            }
            if (this.User.IsInRole("Teacher"))
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var courses = this.courseService.GetCoursesForTeacher(teacher.Id);
                if (courses.Any(x => x.Id == id))
                {
                    return View("VideosTeacher", viewModel);
                }
                return Redirect("/");

            }
            else if (this.User.IsInRole("Student"))
            {
                var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
                var courses = student.Courses;
                if (courses.Any(x => x.Id == id))
                {
                    return View("VideosStudent", viewModel);
                }
                return View("Participants", viewModel);
            }
            return Redirect("/");
        }

        //POST: Upload video
        [HttpPost]
        [Authorize(Roles="Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(string url,int courseId)
        {
            if (url != null && courseId != 0 && url != "")
            {
                Match regexMatch = Regex.Match(url, "^[^v]+v=(.{11}).*",
                          RegexOptions.IgnoreCase);
                var id = regexMatch.Groups[1];
                this.videoService.Add(
                    new Video
                    {
                        CourseId = courseId,
                        SourceId = id.Value,
                        Name = GetTitle(url)
                    });
                this.postService.Add(new Post
                {
                    Content = "^f8d83880ec11490284d74705e43ecc49^",
                    CourseId = courseId,
                    UserId = this.User.Identity.GetUserId(),
                    Date = DateTime.Now

                });
            }
            else
            {
                TempData["AddVidError"] = Common.FieldRequired;
            }
           return Redirect("/CourseVideos/Videos/" + courseId);
        }

        //GET: Get video titile
        [HttpGet]
        private string GetTitle(string url)
        {
            string id = GetArgs(url, "v", '?');
            WebClient client = new WebClient();
            return GetArgs(client.DownloadString("http://youtube.com/get_video_info?video_id=" + id), "title", '&');
        }

        //GET: Arguments for video title
        [HttpGet]
        private string GetArgs(string args, string key, char query)
        {
            int iqs = args.IndexOf(query);
            string querystring = null;

            if (iqs != -1)
            {
                querystring = (iqs < args.Length - 1) ? args.Substring(iqs + 1) : String.Empty;
                NameValueCollection nvcArgs = HttpUtility.ParseQueryString(querystring);
                return nvcArgs[key];
            }
            return String.Empty; // or throw an error
        }

        //GET: Delete video
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                var courseid = this.videoService.GetById(id).CourseId;
                if (this.courseService.GetById(courseid).Teacher.ApplicationUserId == this.User.Identity.GetUserId())
                {
                    this.videoService.Delete(id);
                    return Redirect("/CourseVideos/Videos/" + courseid);
                }
                return Redirect("/");
            }
            return Redirect("/");
        }
    }
}