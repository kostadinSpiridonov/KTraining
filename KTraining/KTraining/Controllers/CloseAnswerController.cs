using KTraining.Models;
using KTraining.Service;
using KTreining.Model;
using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class CloseAnswerController : BaseController
    {
        // POST: Add close answer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddCloseAnswerViewModel model)
        {
            if (model.Content != null && OtherFunctions.IsHasJS(model.Content))
            {
                TempData["EditErrors"] = Common.FieldAnsDanger;
                return Redirect("/CloseQuestion/Question/" + model.QuestionId);
            }
            if (!ModelState.IsValid)
            {
                if (model.Content == null)
                {
                    TempData["EditErrors"] = Common.FieldAnsRequired + " <br/>";
                }
                return Redirect("/CloseQuestion/Question/" + model.QuestionId);
            }
            var question = this.closeQuestionService.GetById(model.QuestionId);
            if (!ModelState.IsValid)
            {
                return Redirect("/CloseQuestion/Question/" + question.Id);
            }
            var asnwerId = this.closeAnswerService.Add(
                new CloseAnswer
                {
                    Content = model.Content,
                    Correct = model.Correct,
                    QuestionId = model.QuestionId
                });
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
                    this.closeAnswerService.AddImage(asnwerId, id);
                }
            }
            return Redirect("/CloseQuestion/Question/" + question.Id);
        }

        //POST: Edit close answer
        [HttpPost]
        public ActionResult Edit(UpdateCloseAnswerViewModel model)
        {
            int correctAnswers = 0;
            foreach (var item in this.closeQuestionService.GetById(model.QuestionId).Answers)
            {
                if (item.Id != model.AnswerId && item.Correct)
                {
                    correctAnswers++;
                }
            }
            if (model.Correct)
            {
                correctAnswers++;
            }
            if (model.Content != null && OtherFunctions.IsHasJS(model.Content))
            {
                TempData["EditErrors"] = Common.FieldAnsDanger;
                return Redirect("/CloseQuestion/Question/" + model.QuestionId);
            }
            if (correctAnswers == 0)
            {
                TempData["EditErrors"] = Common.OneCorrectAns;
                return Redirect("/CloseQuestion/Question/" + model.QuestionId);
            }
            if (!ModelState.IsValid)
            {
                if (model.Content == null)
                {
                    TempData["EditErrors"] = Common.FieldAnsRequired+" <br/>";
                }
                return Redirect("/CloseQuestion/Question/" + model.QuestionId);
            }
            this.closeAnswerService.Update(new CloseAnswer
            {
                Content = model.Content,
                Id = model.AnswerId,
                Correct = model.Correct,
            });
            return Redirect("/CloseQuestion/Question/" + model.QuestionId);
        }

        //GET: Edit close answer
        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
            if (BellongToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                var answer = this.closeAnswerService.GetById(id);
                var model = new UpdateCloseAnswerViewModel
                    {
                        AnswerId = answer.Id,
                        Content = answer.Content,
                        Correct = answer.Correct,
                        QuestionId = answer.QuestionId
                    };
                return PartialView("Edit", model);
            }
            return Redirect("/");
        }

        //POST: Delete close answer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int answerId)
        {
            if (BellongToCurrentUser(answerId, this.User.Identity.GetUserId()))
            {
                var questionId = this.closeAnswerService.GetById(answerId).QuestionId;
                this.closeAnswerService.Delete(answerId);
                return Redirect("/CloseQuestion/Question/" + questionId);
            }
            return Redirect("/");
        }
        
        //Check if the answer belongs to current user
        private bool BellongToCurrentUser(int id, string userId)
        {
            return this.closeAnswerService.GetById(id).Question.Topic.Teacher.ApplicationUserId == userId;
        }

    }
}