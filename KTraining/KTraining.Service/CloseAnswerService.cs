using KTreining.Model;
using System;
using System.Linq;

namespace KTraining.Service
{
    public interface ICloseAnswerService
    {
        int Add(CloseAnswer model);
        void AddImage(int answerId, int imageId);
        void Update(CloseAnswer model);
        CloseAnswer GetById(int id);
        void Delete(int id);
    }

    public class CloseAnswerService : BaseService, ICloseAnswerService
    {
        /// <summary>
        /// Add close answer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(CloseAnswer model)
        {
            this.context.CloseAnswers.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Add image to close answer
        /// </summary>
        /// <param name="answerId"></param>
        /// <param name="imageId"></param>
        public void AddImage(int answerId, int imageId)
        {
            var image = this.context.Images.Find(imageId);
            this.context.CloseAnswers.Find(answerId).Images.Add(image);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Update close answer
        /// </summary>
        /// <param name="model"></param>
        public void Update(CloseAnswer model)
        {
            var answer = this.context.CloseAnswers.Find(model.Id);
            answer.Content = model.Content;
            answer.Correct = model.Correct;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get close answer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CloseAnswer GetById(int id)
        {
            return this.context.CloseAnswers.Find(id);
        }

        /// <summary>
        /// Delete close answer
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var answer = this.context.CloseAnswers.Find(id);
            while (answer.Images.Count > 0)
            {
                this.cloudinaryService.DeleteImage(answer.Images.First().Source.Substring(0, answer.Images.First().Source.IndexOf('.')));
                this.context.Images.Remove(answer.Images.First());
            }
            this.context.CloseAnswers.Remove(answer);
            this.context.SaveChanges();
        }
    }
}
