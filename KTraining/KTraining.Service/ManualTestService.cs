using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface IManualTestService
    {
        int Add(ManualTest model);
        ICollection<ManualTest> GetForTeacher(int id);
        void AddCloseQuestion(int questionId, int testId);
        void AddOpenQuestion(int questionId, int testId);
        void Delete(int id);
        ManualTest GetById(int id);
        double MaxPoints(int id);
        void RemoveCloseQuestion(int questionId, int testId);
        void RemoveOpenQuestion(int questionId, int testId);
    }
    public class ManualTestService : BaseService, IManualTestService
    {
        /// <summary>
        /// Add manual test
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(ManualTest model)
        {

            if (this.context.ManualTests.Where(x => x.Title == model.Title).FirstOrDefault() != null)
            {
                int index = 1;
                while (true)
                {
                    if (this.context.ManualTests.Where(x => x.Title == (model.Title + index.ToString())).FirstOrDefault() == null)
                    {
                        break;
                    }
                    index++;
                }
                model.Title = model.Title + index.ToString();
            }
            this.context.ManualTests.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Get manual tests for teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ICollection<ManualTest> GetForTeacher(int id)
        {
            return this.context.ManualTests.Where(x => x.TeacherId == id).ToList();
        }

        /// <summary>
        /// Add close question to manual test
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="testId"></param>
        public void AddCloseQuestion(int questionId, int testId)
        {
            var test = this.context.ManualTests.Find(testId);
            var question = this.context.CloseQuestions.Find(questionId);
            test.CloseQuestions.Add(question);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Add open qusetion to manual test
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="testId"></param>
        public void AddOpenQuestion(int questionId, int testId)
        {
            var test = this.context.ManualTests.Find(testId);
            var question = this.context.OpenQuestions.Find(questionId);
            test.OpenQuestions.Add(question);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Delete manual test
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var test = this.context.ManualTests.Find(id);
            while (test.CloseQuestions.Count > 0)
            {
                test.CloseQuestions.Remove(test.CloseQuestions.First());
            }
            while (test.OpenQuestions.Count > 0)
            {
                test.OpenQuestions.Remove(test.OpenQuestions.First());
            }
            this.context.SaveChanges();
            this.context.ManualTests.Remove(test);
            this.context.SaveChanges();
        }


        /// <summary>
        /// Get manual test by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ManualTest GetById(int id)
        {
            return this.context.ManualTests.Find(id);
        }

        /// <summary>
        /// Get max points for manual test
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MaxPoints(int id)
        {
            var test = this.context.ManualTests.Find(id);
            double maxPoints = 0;
            foreach (var item in test.CloseQuestions)
            {
                maxPoints += item.Points;
            }
            foreach (var item in test.OpenQuestions)
            {
                maxPoints += item.Points;
            }
            return maxPoints;
        }

        /// <summary>
        /// Remove close question from manual test
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="testId"></param>
        public void RemoveCloseQuestion(int questionId, int testId)
        {
            var test = this.context.ManualTests.Find(testId);
            test.CloseQuestions.Remove(test.CloseQuestions.Where(x => x.Id == questionId).FirstOrDefault());
            this.context.SaveChanges();
        }

        /// <summary>
        /// Remove open question from manual test
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="testId"></param>
        public void RemoveOpenQuestion(int questionId, int testId)
        {
            var test = this.context.ManualTests.Find(testId);
            test.OpenQuestions.Remove(test.OpenQuestions.Where(x => x.Id == questionId).FirstOrDefault());
            this.context.SaveChanges();
        }
    }
}
