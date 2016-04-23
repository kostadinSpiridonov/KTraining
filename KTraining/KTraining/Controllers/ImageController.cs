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
    [Authorize]
    public class ImageController : BaseController
    {
        // GET: ImagController
        [HttpGet]
        [Authorize(Roles="Teacher")]
        public ActionResult Delete(int id)
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            if (this.imageService.SearchImageForTeacher(id, teacher.Id))
            {
                if (id != 0)
                {
                    this.imageService.Delete(id);
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect("/");
        }

        // POST: Upload images for close answer
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult UploadForCloseAnswer(int id)
        {
            var answer = this.closeAnswerService.GetById(id);
            if (answer.Question.Topic.Teacher.ApplicationUserId == this.User.Identity.GetUserId())
            {
                var viewModel = new UploadImagesForCloseAnswer
                {
                    AnswerId = id,
                    QuestionId = answer.QuestionId
                };
                return PartialView("UploadForCloseAnswer", viewModel);
            }
            return Redirect("/");
        }

        // POST: Upload images for close questions
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadForCloseQuestion(UploadImagesForCloseQuestion model)
        {
            if(!ModelState.IsValid)
            {
                return Redirect("/CloseQuestion/Question/" + model.QuestionId);
            }
            int id;
            foreach (var file in model.Images)
            {
                if (file != null)
                {
                    var uploadResult = this.cloudinaryService.UploadImage(file.FileName, file.InputStream);
                    id = this.imageService.Add(new Image
                    {
                        Name = file.FileName,
                        Source = uploadResult
                    });
                    this.closeQuestionService.AddImage(model.QuestionId, id);
                }
            }
            return Redirect("/CloseQuestion/Question/" + model.QuestionId);
        }

        // POST: Upload images for close questions
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadForCloseAnswer(UploadImagesForCloseAnswer model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/CloseQuestion/Question/" + model.QuestionId);
            }
            int id;
            foreach (var file in model.Images)
            {
                if (file != null)
                {
                    var uploadResult = this.cloudinaryService.UploadImage(file.FileName, file.InputStream);
                    id = this.imageService.Add(new Image
                    {
                        Name = file.FileName,
                        Source = uploadResult
                    });
                    this.closeAnswerService.AddImage(model.AnswerId, id);
                }
            }
            return Redirect("/CloseQuestion/Question/" + model.QuestionId);
        }

        // POST: Upload images for open questions
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult UploadForOpenQuestion(UploadImagesForOpenQuestion model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/OpenQuestion/Question/" + model.QuestionId);
            }
            int id;
            foreach (var file in model.Images)
            {
                if (file != null)
                {
                    var uploadResult = this.cloudinaryService.UploadImage(file.FileName, file.InputStream);
                    id = this.imageService.Add(new Image
                    {
                        Name = file.FileName,
                        Source = uploadResult
                    });
                    this.openQuestionService.AddImage(model.QuestionId, id);
                }
            }
            return Redirect("/OpenQuestion/Question/" + model.QuestionId);
        }

        
    }
}