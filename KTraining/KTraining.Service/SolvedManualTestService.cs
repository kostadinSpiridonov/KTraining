using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace KTraining.Service
{
    public interface ISolvedManualTestService
    {
        int Add(SolvedManualTest model);
        SolvedManualTest GetById(int id);
        double GetPointForTest(int id);
        bool IsWholeTestComplete(int testId, int courseId);
        void SetComplete(int testId);
        void SetChecked(int testId);
        ICollection<SolvedManualTest> GetForTeacher(int teacherId);
        void SetMark(int testId, double mark);
    }

    public class SolvedManualTestService : BaseService, ISolvedManualTestService
    {
        /// <summary>
        /// Add solved manual test
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(SolvedManualTest model)
        {
            this.context.SolvedManualTests.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }
        
        /// <summary>
        /// Get solved manual test by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SolvedManualTest GetById(int id)
        {
            return this.context.SolvedManualTests.Find(id);
        }

        /// <summary>
        /// Get points for test
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetPointForTest(int id)
        {
            var test = this.context.SolvedManualTests.Find(id);
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
        /// Check whether the whole test is complete
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool IsWholeTestComplete(int testId, int courseId)
        {
            var courseStudentIds = this.context.Courses.Find(courseId).Students.Select(x => x.Id).ToList();
            var count = this.context.SolvedManualTests.Where(x => x.TestId == testId)
                .Where(x => courseStudentIds.Contains(x.StudentId))
                .Where(x => x.IsComplete == true)
                .ToList().Count();

            if (count % courseStudentIds.Count == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set complete to test
        /// </summary>
        /// <param name="testId"></param>
        public void SetComplete(int testId)
        {
            this.context.SolvedManualTests.Find(testId).IsComplete = true;
            this.context.SaveChanges();
        }


        /// <summary>
        /// Get solved manual tests for teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ICollection<SolvedManualTest> GetForTeacher(int teacherId)
        {
            return this.context.SolvedManualTests.Include(x => x.Course).Where(x => x.Course.TeacherId == teacherId).ToList();
        }

        /// <summary>
        /// Set checked to solved manual tests
        /// </summary>
        /// <param name="testId"></param>
        public void SetChecked(int testId)
        {
            this.context.SolvedManualTests.Find(testId).IsChecked = true;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Set mark to solved manual test
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="mark"></param>
        public void SetMark(int testId, double mark)
        {
            this.context.SolvedManualTests.Find(testId).Mark = mark;
            this.context.SaveChanges();
        }
    }
}
