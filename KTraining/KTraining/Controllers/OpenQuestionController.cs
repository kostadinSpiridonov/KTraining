using KTraining.Models;
using KTraining.Service;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize(Roles="Teacher")]
    public class OpenQuestionController : BaseController
    {
        // GET:Get Open Questions for topic
        [HttpGet]
        public ActionResult Questions(int id)
        {
            if (BellongToCurrentUserT(id, this.User.Identity.GetUserId()))
            {
                var questions = this.openQuestionService.GetForTopic(id);
                var viewModel = new OpenQuestionsViewModel
                {
                    TopicId = id,
                    Questions = questions.ToList()
                    .ConvertAll(x =>
                    new OpenQuestionViewModel
                    {
                        Id = x.Id,
                        Content = x.Content
                    })
                };
                return View(viewModel);
            }
            return Redirect("/");
        }

        //GET: Add open question
        [HttpGet]
        public ActionResult Add(int id)
        {
            if (BellongToCurrentUserT(id, this.User.Identity.GetUserId()))
            {
                return View(new AddOpenQuestionViewModel { TopicId = id, Points = 2 });
            }
            return Redirect("/");
        }

        //POST: Add question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddOpenQuestionViewModel model)
        {
            if (model.Content!=null&& OtherFunctions.IsHasJS(model.Content))
            {
                ModelState.AddModelError("", Common.FiledQDanger);
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var questionId = this.openQuestionService.Add(new OpenQuestion
            {
                Content = model.Content,
                Points = model.Points,
                TopicId = model.TopicId,
                HelpLink = model.HelpLink
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
                    this.openQuestionService.AddImage(questionId, id);
                }
            }
            return Redirect("/OpenQuestion/Questions/" + model.TopicId);
        }

        //GET: Get a question
        [HttpGet]
        public ActionResult Question(int id)
        {
            if (BellongToCurrentUserQ(id, this.User.Identity.GetUserId()))
            {
                var question = this.openQuestionService.GetById(id);
                var viewModel = new OpenQuestionFullViewModel
                {
                    Content = question.Content,
                    Images = this.cloudinaryService.AddPathToQuestionImageName(question.Images).ToList(),
                    Points = question.Points,
                    Topic = question.Topic,
                    Id = question.Id
                };
                return View(viewModel);
            }
            return Redirect("/");
        }

        //POST: Edit a question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OpenQuestionFullViewModel model)
        {
            if (model.Content!=null&& OtherFunctions.IsHasJS(model.Content))
            {
                TempData["ConError"] = Common.FiledQDanger;
                return Redirect("/OpenQuestion/Question/" + model.Id);
            }
            if (!ModelState.IsValid)
            {
                if (model.Content == null)
                {
                    TempData["ConError"] = Common.FieldQRequired;
                }
                if(model.Points==0)
                {
                    TempData["PointError"] = Common.ShouldBeNum;
                }
                return Redirect("/OpenQuestion/Question/" + model.Id);
            }
            this.openQuestionService.Update(new OpenQuestion
            {
                Content = model.Content,
                Points = model.Points,
                Id = model.Id,
                HelpLink = model.HelpLink
            });
            return Redirect("/OpenQuestion/Question/" + model.Id);
        }

        //POST: Delete open question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int questionId)
        {
            var topicId = this.openQuestionService.GetById(questionId).TopicId;
            try
            {
                this.openQuestionService.Delete(questionId);
            }
            catch
            {
                TempData["DelError"] = Common.SomeoneUserThisQ;
                return Redirect("/OpenQuestion/Question/" + questionId);
            }
            return Redirect("/OpenQuestion/Questions/" + topicId);
        }

        //Check if the open question belongs to current user
        private bool BellongToCurrentUserQ(int id, string userId)
        {
            return this.openQuestionService.GetById(id).Topic.Teacher.ApplicationUserId == userId;
        }

        //Check if the topic belongs to current user
        private bool BellongToCurrentUserT(int id, string userId)
        {
            return this.topicService.GetById(id).Teacher.ApplicationUserId == userId;
        }
    }
}