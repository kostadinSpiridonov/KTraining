using KTraining.Models;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Teacher,Student")]
    public class PostController : BaseController
    {
        //GET: Get course's posts
        [HttpGet]
        public ActionResult Posts(int id)
        {
            var course = this.courseService.GetById(id);
            if (course.Students.Select(x => x.ApplicationUserId).Contains(this.User.Identity.GetUserId())||
                course.Teacher.ApplicationUserId==this.User.Identity.GetUserId())
            {
                var viewModel = new CoursePostsViewModel
                {
                    Name = course.Name,
                    Id = course.Id,
                    Posts = course.Posts.Reverse().ToList()
                };
                foreach (var item in viewModel.Posts)
                {
                    if(item.Content.Contains("bb81ab8cfb1745708af8ab41b4b74368")
                        ||item.Content.Contains("t4c66efaa35043beaaeb5f7e7c6a1ed1")
                        ||item.Content.Contains("f8d83880ec11490284d74705e43ecc49")
                        ||item.Content.Contains("d1b83438729e43b886f48920a0478a0f")
                        || item.Content.Contains("ab81e1b26160402e86c134cc4bc53f55")
                        )
                    {
                        item.Content = this.convertResource.ConvertContentCode(item.Content);
                    }
                }
                return View(viewModel);
            }
            return Redirect("/");
        }

        //POST: Add post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(PostAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Post/Posts/" + model.CourseId);
            }
            this.postService.Add(new Post
                {
                    Content = model.Content,
                    CourseId = model.CourseId,
                    Date = DateTime.Now,
                    UserId = this.User.Identity.GetUserId()
                });
            return Redirect("/Post/Posts/" + model.CourseId);
        }
    }
}