using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface IAutomaticTestService
    {
        int Add(AutomaticTest model);
        ICollection<AutomaticTest> GetForTeacher(int id);
        void AddCloseQuestion(int questionId, int testId);
        void Delete(int id);
        AutomaticTest GetById(int id);
        double GetPointsForTest(int testId);
        void RemoveQuestion(int questionId, int testId);
    }

    public class AutomaticTestService : BaseService, IAutomaticTestService
    {
        /// <summary>
        /// Add automatic test
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(AutomaticTest model)
        {
            if (this.context.AutomaticTests.Where(x => x.Title == model.Title).FirstOrDefault() != null)
            {
                int index = 1;
                while (true)
                {
                    if (this.context.AutomaticTests.Where(x => x.Title == (model.Title + index.ToString())).FirstOrDefault() == null)
                    {
                        break;
                    }
                    index++;
                }
                model.Title = model.Title + index.ToString();
            }
            this.context.AutomaticTests.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Get automatics tests for teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ICollection<AutomaticTest> GetForTeacher(int id)
        {
            return this.context.AutomaticTests.Where(x => x.TeacherId == id).ToList();
        }

        /// <summary>
        /// Add close question to automatic test
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="testId"></param>
        public void AddCloseQuestion(int questionId, int testId)
        {
            var question = this.context.CloseQuestions.Find(questionId);
            this.context.AutomaticTests.Find(testId).CloseQuestions.Add(question);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Delete automatic test
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var test = this.context.AutomaticTests.Find(id);
            while (test.CloseQuestions.Count > 0)
            {
                test.CloseQuestions.Remove(test.CloseQuestions.First());
            }
            this.context.SaveChanges();
            this.context.AutomaticTests.Remove(test);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get automatic test by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AutomaticTest GetById(int id)
        {
            return this.context.AutomaticTests.Find(id);
        }

        /// <summary>
        /// Get max points for automatic test
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        public double GetPointsForTest(int testId)
        {
            double points = 0;
            var test = this.context.AutomaticTests.Find(testId);
            foreach (var item in test.CloseQuestions)
            {
                points += item.Points;
            }
            return points;
        }

        /// <summary>
        /// Remove question from definitely automatic test
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="testId"></param>
        public void RemoveQuestion(int questionId, int testId)
        {
            var test = this.context.AutomaticTests.Find(testId);
            test.CloseQuestions.Remove(test.CloseQuestions.Where(x => x.Id == questionId).FirstOrDefault());
            this.context.SaveChanges();
        }
    }
}
