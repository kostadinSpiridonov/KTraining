using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace KTraining.Service
{
    public interface ISolvedManualTestForLevelService
    {

        int Add(SolvedManualTestForLevel model);
        SolvedManualTestForLevel GetById(int id);
        double GetPointForTest(int id);
        ICollection<SolvedManualTestForLevel> GetForTeacher(int teacherId);
        void Remove(int id);

    }
    public class SolvedManualTestForLevelService : BaseService, ISolvedManualTestForLevelService
    {
        /// <summary>
        /// Add solved manutal test for level
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(SolvedManualTestForLevel model)
        {
            this.context.SolvedManualTestsForLevel.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Get solved manualte test for level by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SolvedManualTestForLevel GetById(int id)
        {
            return this.context.SolvedManualTestsForLevel.Find(id);
        }

        /// <summary>
        /// Get points for definitely test
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetPointForTest(int id)
        {
            var test = this.context.SolvedManualTestsForLevel.Find(id);
            double points = 0;
            bool flag = true;
            foreach (var item in test.SolvedCloseQuestions)
            {
                foreach (var item2 in item.CloseQuestion.Answers.Where(x => x.Correct == true))
                {
                    if (!item.SelectedAnswers.Select(x => x.Id).ToList().Contains(item2.Id))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    points += item.CloseQuestion.Points;
                }
                flag = true;

            }
            foreach (var item in test.SolvedOpenQuestions)
            {
                points += item.Points;
            }
            return points;
        }


        /// <summary>
        /// Get solved manual tests for level for teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ICollection<SolvedManualTestForLevel> GetForTeacher(int teacherId)
        {
            return this.context.SolvedManualTestsForLevel.Include(x=>x.CourseLevel).Where(x => x.CourseLevel.Course.TeacherId == teacherId).ToList();
        }

        /// <summary>
        /// Remove solved manual test for level
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            var test = this.context.SolvedManualTestsForLevel.Find(id);
            while(test.SolvedOpenQuestions.Count()!=0)
            {
                this.context.SolvedOpenQuestions.Remove(test.SolvedOpenQuestions.First());
            } 
            while (test.SolvedCloseQuestions.Count() != 0)
            {
                this.context.SolvedCloseQuestions.Remove(test.SolvedCloseQuestions.First());
            }
            this.context.SaveChanges();
            this.context.SolvedManualTestsForLevel.Remove(test);
            this.context.SaveChanges();
        }
    }
}