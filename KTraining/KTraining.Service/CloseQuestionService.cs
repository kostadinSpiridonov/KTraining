using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface ICloseQuestionService
    {
        int Add(CloseQuestion model);
        ICollection<CloseQuestion> GetForTopic(int topicId);
        CloseQuestion GetById(int id);
        void AddImage(int questionId, int imageId);
        void Update(CloseQuestion model);
        void Delete(int id);
        CloseQuestion GetQuestionByTestAndIndex(int testId, int questionIndex);
    }

    public class CloseQuestionService : BaseService, ICloseQuestionService
    {
        /// <summary>
        /// Add close question
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(CloseQuestion model)
        {
            this.context.CloseQuestions.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Get close questions for definitely topic
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public ICollection<CloseQuestion> GetForTopic(int topicId)
        {
            return this.context.CloseQuestions.Where(x => x.TopicId == topicId).ToList();
        }

        /// <summary>
        /// Get close question by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CloseQuestion GetById(int id)
        {
            return this.context.CloseQuestions.Find(id);
        }

        /// <summary>
        /// Add image to close question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="imageId"></param>
        public void AddImage(int questionId, int imageId)
        {
            var image = this.context.Images.Find(imageId);
            this.context.CloseQuestions.Find(questionId).Images.Add(image);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Update- close question
        /// </summary>
        /// <param name="model"></param>
        public void Update(CloseQuestion model)
        {
            var question = this.context.CloseQuestions.Find(model.Id);
            question.Content = model.Content;
            question.Points = model.Points;
            question.HelpLink = model.HelpLink;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Delete close question
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var question = this.context.CloseQuestions.Find(id);
            while (question.Images.Count > 0)
            {
                this.cloudinaryService.DeleteImage(question.Images.First().Source.Substring(0, question.Images.First().Source.IndexOf(".")));
                this.context.Images.Remove(question.Images.First());
            }
            foreach (var item in question.Answers)
            {
                while (item.Images.Count > 0)
                {
                    this.cloudinaryService.DeleteImage(item.Images.First().Source.Substring(0, item.Images.First().Source.IndexOf(".")));
                    this.context.Images.Remove(item.Images.First());
                }
            }
            this.context.CloseQuestions.Remove(question);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get close question by test id and question index
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="questionIndex"></param>
        /// <returns></returns>
        public CloseQuestion GetQuestionByTestAndIndex(int testId, int questionIndex)
        {
            return this.context.AutomaticTests.Find(testId).CloseQuestions.ElementAt(questionIndex);
        }
    }
}
