using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface IOpenQuestionService
    {
        int Add(OpenQuestion model);
        ICollection<OpenQuestion> GetForTopic(int topicId);
        void AddImage(int questionId, int imageId);
        OpenQuestion GetById(int id);
        void Update(OpenQuestion model);
        void Delete(int id);
    }

    public class OpenQuestionService : BaseService, IOpenQuestionService
    {
        /// <summary>
        /// Add open question
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(OpenQuestion model)
        {
            this.context.OpenQuestions.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Get open questions for topic
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public ICollection<OpenQuestion> GetForTopic(int topicId)
        {
            return this.context.OpenQuestions.Where(x => x.TopicId == topicId).ToList();
        }

        /// <summary>
        /// Add image to open question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="imageId"></param>
        public void AddImage(int questionId, int imageId)
        {
            var question = this.context.OpenQuestions.Find(questionId);
            var image = this.context.Images.Find(imageId);
            question.Images.Add(image);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get open qiestion by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpenQuestion GetById(int id)
        {
            return this.context.OpenQuestions.Find(id);
        }

        /// <summary>
        /// Update open question
        /// </summary>
        /// <param name="model"></param>
        public void Update(OpenQuestion model)
        {
            var question = this.context.OpenQuestions.Find(model.Id);
            question.Content = model.Content;
            question.Points = model.Points;
            this.context.SaveChanges();
        }

        /// <summary>
        /// /Delete open question
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var question = this.context.OpenQuestions.Find(id);
            while (question.Images.Count > 0)
            {
                this.cloudinaryService.DeleteImage(question.Images.First().Source.Substring(0, question.Images.First().Source.IndexOf(".")));
                this.context.Images.Remove(question.Images.First());
            }
            this.context.OpenQuestions.Remove(question);
            this.context.SaveChanges();
        }
    }
}
