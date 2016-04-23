using KTraining.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTreining.Model;
using KTraining.Service;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize(Roles="Teacher")]
    public class CloseQuestionController : BaseController
    {
        // GET: Get close questions for topic
        [HttpGet]
        public ActionResult Questions(int id)
        {
            if (BellongToCurrentUserT(id, this.User.Identity.GetUserId()))
            {
                var question = this.closeQuestionService.GetForTopic(id);
                var viewModel =
                    new CloseQuestionShowViewModel
                    {
                        TopicId = id,
                        CloseQuestions = question.ToList().ConvertAll(x =>
                            new CloseQuestionViewModel
                            {
                                Content = x.Content,
                                Id = x.Id
                            })
                    };
                return View(viewModel);
            }
            return Redirect("/");
        }

        //GET: Add close question
        [HttpGet]
        public ActionResult Add(int id)
        {
            if (BellongToCurrentUserT(id, this.User.Identity.GetUserId()))
            {
                return View(new AddCloseQuestionViewModel { TopicId = id, Points = 1 });
            }
            return Redirect("/");
        }

        //POST: Add question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddCloseQuestionViewModel model)
        {
            try
            {
                foreach (var item in model.Answers)
                {
                    if (OtherFunctions.IsHasJS(item.Content))
                    {
                        item.Content = "";
                        ModelState.AddModelError("",Common.FieldAnsDanger);
                        return View(model);
                    }
                }
            }
            catch { }

            if (model.Content == null || model.Content == "" )
            {
                ModelState.AddModelError("", Common.FieldQRequired);
                return View(model);
            }
            if (OtherFunctions.IsHasJS(model.Content))
            {
                ModelState.AddModelError("", Common.FiledQDanger);
                return View(model);
            }
            int correctAnswers = 0;
            foreach (var item in model.Answers)
            {
                if(item.Correct)
                {
                    correctAnswers++;
                }
            }
            if(correctAnswers==0)
            {
                ModelState.AddModelError("",Common.OneCorrectAns);
                return View(model);
            }
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var questionId = this.closeQuestionService.Add(new CloseQuestion
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
                    id= this.imageService.Add(new Image
                        {
                            Name=file.FileName,
                            Source = uploadResult
                        });
                    this.closeQuestionService.AddImage(questionId, id);
                }
            }
            foreach(var item in model.Answers)
            {
                var aId = this.closeAnswerService.Add(
                    new CloseAnswer
                    {
                        Content = item.Content,
                        Correct = item.Correct,
                        QuestionId = questionId
                    });
                foreach (var file in item.Images)
                {
                    if (file != null)
                    {
                        var uploadResult = this.cloudinaryService.UploadImage(file.FileName, file.InputStream);
                        id = this.imageService.Add(new Image
                        {
                            Name = file.FileName,
                            Source = uploadResult
                        });
                        this.closeAnswerService.AddImage(aId, id);
                    }
                }
            }
            return Redirect("/CloseQuestion/Question/" + questionId);
        }

        //GET: Get a question
        [HttpGet]
        public ActionResult Question(int id)
        {
            if (BellongToCurrentUserQ(id, this.User.Identity.GetUserId()))
            {
                var question = this.closeQuestionService.GetById(id);
                var viewModel = new CloseQuestionFullViewModel
                {
                    Content = question.Content,
                    Images = this.cloudinaryService.AddPathToQuestionImageName(question.Images).ToList(),
                    Points = question.Points,
                    Topic = question.Topic,
                    Id = question.Id,
                    Answers = question.Answers.ToList(),
                    HelpLink = question.HelpLink
                };
                foreach (var item in viewModel.Answers)
                {
                    item.Images = this.cloudinaryService.AddPathToQuestionImageName(item.Images).ToList();
                }
                return View(viewModel);
            }
            return Redirect("/");
        }

        //POST: Edit a question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CloseQuestionFullViewModel model)
        {
            if (!ModelState.IsValid || model.Content == null || OtherFunctions.IsHasJS(model.Content))
            {
                string errors = "";
                if (model.Content != null && OtherFunctions.IsHasJS(model.Content))
                {
                    errors += Common.FiledQDanger+"<br/>";
                }
                if(model.Content==null)
                {
                    errors += Common.FieldQRequired+ "<br/>";
                }
                if(model.Points==0)
                {
                    errors += (Common.ShouldBeNum+" <br/>");
                }
                TempData["EditErrors"] = errors;
                return Redirect("/CloseQuestion/Question/" + model.Id);
            }
            this.closeQuestionService.Update(new CloseQuestion
                {
                    Content = model.Content,
                    Points = model.Points,
                    Id = model.Id,
                    HelpLink = model.HelpLink
                });
            return Redirect("/CloseQuestion/Question/" + model.Id);
        }

        //POST: Delete close question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int questionId)
        {
            var topicId = this.closeQuestionService.GetById(questionId).TopicId;
            try
            {
                this.closeQuestionService.Delete(questionId);
            }
            catch
            {
                TempData["DelError"] = Common.SomeoneUserThisQ;
                return Redirect("/CloseQuestion/Question/" + questionId);
            }
            return Redirect("/CloseQuestion/Questions/" + topicId);
        }
        
        //Check if the question belongs to current user
        private bool BellongToCurrentUserQ(int id,string userId)
        {
            return this.closeQuestionService.GetById(id).Topic.Teacher.ApplicationUserId == userId;
        }

        //Check if the topic belongs to current user
        private bool BellongToCurrentUserT(int id, string userId)
        {
            return this.topicService.GetById(id).Teacher.ApplicationUserId == userId;
        }
    }
}