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
    [Authorize(Roles = "Teacher")]
    public class TopicController : BaseController
    {
        // GET: Topics
        [HttpGet]
        public ActionResult Topics()
        {
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            var topics = this.topicService.GetForTeacher(teacher.Id);
            var viewModel = topics.ToList().ConvertAll(x =>
                new TopicViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            return View(viewModel);
        }

        //POST: Add topic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddTopicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = "";
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateVal.Errors)
                    {
                        errors += error.ErrorMessage;
                    }
                }
                TempData["AddTErr"] = errors;
                return RedirectToAction("Topics");
            }
            var teacher = this.userService.GetTeacherByAppUserId(this.User.Identity.GetUserId());
            this.topicService.Add(
                new Topic
                {
                    Name = model.Name,
                    TeacherId = teacher.Id
                });
            return RedirectToAction("Topics");
        }

        //GET: Type questions
        [HttpGet]
        public ActionResult TypeQuestions(int id)
        {
            if (BellongToCurrentUser(id, this.User.Identity.GetUserId()))
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Topics");
                }
                var viewModel = new TypeQuestionsViewModel
                {
                    TopicId = id,
                    TopicName = this.topicService.GetById(id).Name
                };
                return View(viewModel);
            }
            return Redirect("/");
        }

        //POST: Delete close topic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int topicId)
        {
            if (BellongToCurrentUser(topicId, this.User.Identity.GetUserId()))
            {
                try
                {
                    this.topicService.Delete(topicId);
                }
                catch
                {
                    TempData["DelError"] = Common.SomeoneUseThisTopic;
                    return Redirect("/Topic/TypeQuestions/" + topicId);
                }
                return Redirect("/Topic/Topics");
            }
            return Redirect("/");
        }

        //Check if the topic belongs to current user
        private bool BellongToCurrentUser(int id,string userId)
        {
            return this.topicService.GetById(id).Teacher.ApplicationUserId == userId;
        }
    }
}