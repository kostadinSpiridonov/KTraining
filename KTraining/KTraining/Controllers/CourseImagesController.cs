using KTraining.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTreining.Model;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize]
    public class CourseImagesController : BaseController
    {
        // GET: Course Images
        [Authorize(Roles = "Teacher,Student")]
        [HttpGet]
        public ActionResult Images(int id)
        {
            var images = this.courseImageService.GetImagesForCourse(id);
            var viewModel =
                new ShowImageViewModel
            {
                Id=id,
                Images=images.ToList()
                .ConvertAll(x=>
                new ImageViewModel
                {
                    Id=x.Id,
                    Name=x.Name,
                    Source =this.cloudinaryService.GetImageUrl(x.Source)
                }),
                CourseName=this.courseService.GetById(id).Name
            };
            if (this.courseService.IsComplete(id))
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    return View("ImagesCompleted", viewModel);
                }
            }
            if (this.User.IsInRole("Teacher"))
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var courses = this.courseService.GetCoursesForTeacher(teacher.Id);
                if (courses.Any(x => x.Id == id))
                {
                    return View("ImagesTeacher", viewModel);
                }
                return Redirect("/");

            }
            return View(viewModel);
        }

        //POST: Upload images
        [Authorize(Roles = "Teacher,Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(int id, HttpPostedFileBase[] images)
        {
            if (images[0] != null)
            {
                foreach (var file in images)
                {
                    var name = this.cloudinaryService.UploadImage(file.FileName, file.InputStream);
                    this.courseImageService.Add(
                        new KTreining.Model.CourseImage
                        {
                            Source = name,
                            CourseId = id,
                            Name = file.FileName,
                            ApplicationUserId = this.User.Identity.GetUserId()
                        });

                    string userName = "";
                    if (this.User.IsInRole("Teacher"))
                    {
                        var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                        userName = teacher.ApplicationUser.FirstName + " " + teacher.ApplicationUser.SecondName + " " + teacher.ApplicationUser.LastName;
                    }
                    else if (this.User.IsInRole("Student"))
                    {
                        var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
                        userName = student.ApplicationUser.FirstName + " " + student.ApplicationUser.SecondName + " " + student.ApplicationUser.LastName;
                    }

                    this.postService.Add(new Post
                    {
                        Content =Common.AddNewImage,
                        CourseId = id,
                        UserId = this.User.Identity.GetUserId(),
                        Date = DateTime.Now

                    });
                }
                return Redirect("/CourseImages/Images/" + id);
            } 
            return Redirect("/CourseImages/Images/" + id);
        }

        //GET: Delete image
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                var courseid = this.courseImageService.GetById(id).CourseId;
                if (this.courseService.GetById(courseid).Teacher.ApplicationUserId == this.User.Identity.GetUserId())
                {
                    this.courseImageService.Delete(id);
                    return Redirect("/CourseImages/Images/" + courseid);
                }
                return Redirect("/");
            }
            return Redirect("/");
        }
    }
}