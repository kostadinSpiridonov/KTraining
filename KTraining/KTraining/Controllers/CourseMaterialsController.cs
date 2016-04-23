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
    public class CourseMaterialsController : BaseController
    {
        //GET: Materials
        [Authorize(Roles = "Teacher,Student")]
        [HttpGet]
        public ActionResult Materials(int id)
        {
            var materials = this.courseService.GetById(id).Materials.Reverse();
            var viewModel =
                new ShowMaterials
                {
                    Id = id,
                    Materials = materials.ToList().ConvertAll(x => new MaterialViewModel
                    {
                        Id=x.Id,
                        Name = x.Name,
                        Source = this.cloudinaryService.GetFileUrl(x.Source)
                    }),
                    CourseName=this.courseService.GetById(id).Name
                };

            if (this.courseService.IsComplete(id))
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    return View("MaterialsCompleted", viewModel);
                }
            }

            if (this.User.IsInRole("Teacher"))
            {
                var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
                var courses = this.courseService.GetCoursesForTeacher(teacher.Id);
                if (courses.Any(x => x.Id == id))
                {
                    return View("MaterialsTeacher", viewModel);
                }
                return Redirect("/");

            }
            else if (this.User.IsInRole("Student"))
            {
                var student = this.studentService.GetStudentByAppUserId(this.User.Identity.GetUserId());
                var courses = student.Courses;
                if (courses.Any(x => x.Id == id))
                {
                    return View("MaterialsStudent", viewModel);
                }
                return View("Participants", viewModel);
            }
            return Redirect("/");
        }

        //POST:Upload file
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(int id,HttpPostedFileBase[] files)
        {
            if (files != null&&files[0]!=null)
            {
                foreach (var file in files)
                {
                    var name = this.cloudinaryService.UploadFile(file.FileName, file.InputStream);
                    this.cloudFilesService.Add(
                        new KTreining.Model.CloudFile
                        {
                            Source = name,
                            CourseId = id,
                            Name = file.FileName
                        });
                    this.postService.Add(new Post
                    {
                        Content =Common.AddMaterial,
                        CourseId = id,
                        UserId = this.User.Identity.GetUserId(),
                        Date = DateTime.Now

                    });
                }

                return Redirect("/CourseMaterials/Materials/"+id);
            }
            return Redirect("/CourseMaterials/Materials/" + id);
        }

        //GET:Delete file
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteFile(int id)
        {
            if(id!=0)
            {
                var courseid = this.cloudFilesService.GetById(id).CourseId;
                if (this.courseService.GetById(courseid).Teacher.ApplicationUserId == this.User.Identity.GetUserId())
                {
                    this.cloudFilesService.Delete(id);
                    return Redirect("/CourseMaterials/Materials/" + courseid);
                }
                return Redirect("/");
            }
            return Redirect("/");
        }
    }
}