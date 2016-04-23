using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface ITopicService
    {
        void Add(Topic model);
        ICollection<Topic> GetForTeacher(int teacherId);
        void Delete(int id);
        Topic GetById(int id);
    }

    public class TopicService : BaseService, ITopicService
    {
        /// <summary>
        /// Add topic
        /// </summary>
        /// <param name="model"></param>
        public void Add(Topic model)
        {
            this.context.Topics.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get topics for teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ICollection<Topic> GetForTeacher(int teacherId)
        {
            return this.context.Topics.Where(x => x.TeacherId == teacherId).ToList();
        }

        /// <summary>
        /// Delete topic
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var topic = this.context.Topics.Find(id);
            foreach (var question in topic.CloseQuestions)
            {
                while (question.Images.Count > 0)
                {
                    this.cloudinaryService.DeleteImage(question.Images.First().Source);
                    this.context.Images.Remove(question.Images.First());
                }

                foreach (var answer in question.Answers)
                {
                    while (answer.Images.Count > 0)
                    {
                        this.cloudinaryService.DeleteImage(answer.Images.First().Source);
                        this.context.Images.Remove(answer.Images.First());
                    }
                }
            }
            this.context.Topics.Remove(topic);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get topic by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Topic GetById(int id)
        {
            return this.context.Topics.Find(id);
        }
    }
}
