using KTraining.Models;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KTraining.Controllers
{
    public class ImportQuestionController : BaseController
    {
        //POST: Import close question
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportClose(int id, HttpPostedFileBase file)
        {
            var questions = new List<CloseQuestion>();
            if (file != null && id != 0)
            {
                questions = this.importQuestionService.GetCloseQuestions(file.InputStream).ToList();
                foreach (var item in questions)
                {
                    item.TopicId = id;
                    this.closeQuestionService.Add(item);
                }
            }
            var viewModel = new ImportQuestionViewModel
            {
                ImportedQuestions = questions.Count,
                TopicId = id
            };
            return View(viewModel);
        }

        //POST: Import open question
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportOpen(int id, HttpPostedFileBase file)
        {
            var questions = new List<OpenQuestion>();
            if (file != null && id != 0)
            {
                questions = this.importQuestionService.GetOpenQuestions(file.InputStream).ToList();
                foreach (var item in questions)
                {
                    item.TopicId = id;
                    this.openQuestionService.Add(item);
                }
            }
            var viewModel = new ImportQuestionViewModel
            {
                ImportedQuestions = questions.Count,
                TopicId = id
            };
            return View(viewModel);
        }
    }
}