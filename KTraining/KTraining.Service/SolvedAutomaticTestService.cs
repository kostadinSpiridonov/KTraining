using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
                  
namespace KTraining.Service
{
    public interface ISolvedAutomaticTestService
    {
        int Add(SolvedAutomaticTest model);
        SolvedAutomaticTest GetById(int id);
        double GetPointForTest(SolvedAutomaticTest model);
        double GetPointForTest(int id);
        bool IsWholeTestComplete(int testId, int courseId);
        bool IsWholeTestCompleteTrack(int testId, int courseId,int n);
        void SetShow(List<int> testIds);
        void SetComplete(int testId);
        ICollection<SolvedAutomaticTest> GetForTeacher(int teacherId);
    }

    public class SolvedAutomaticTestService : BaseService, ISolvedAutomaticTestService
    {
        /// <summary>
        /// Add solved automatic test
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(SolvedAutomaticTest model)
        {
            this.context.SolvedAutomaticTests.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Get solved automatic test by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SolvedAutomaticTest GetById(int id)
        {
            return this.context.SolvedAutomaticTests.Find(id);
        }

        /// <summary>
        /// Get points for solved test
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetPointForTest(int id)
        {
            double points = 0;
            bool flag = true;
            var test = this.context.SolvedAutomaticTests.Find(id);
            foreach (var item in test.SolvedQuestions)
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
            var count = this.context.SolvedAutomaticTests.Where(x => x.TestId == testId)
                .Where(x => courseStudentIds.Contains(x.StudentId))
                .Where(x => x.IsComplete == true)
                .Where(x=>x.Show==false)
                .ToList().Count();
            if(this.context.AutomaticTestsForSolving.Where(x=>x.TestId==testId).Where(x=>x.CourseId==courseId).Count()==0)
            {
                return true;
            }
            if (count % courseStudentIds.Count == 0)
            {
                return true;
            }
            return false;
        }
        public bool IsWholeTestCompleteTrack(int testId, int courseId,int n)
        {
            /*var courseStudentIds = this.context.Courses.Find(courseId).Students.Select(x => x.Id).ToList();
            var count = this.context.SolvedAutomaticTests.Where(x => x.TestId == testId)
                .Where(x => courseStudentIds.Contains(x.StudentId))
                .Where(x => x.IsComplete == true)
                .ToList().Count();

            if ((count+n) % (courseStudentIds.Count) == 0)
            {
                return true;
            }*/
            return false;
        }

        /// <summary>
        /// Set show to solved tests (students will can see their solved tests)
        /// </summary>
        /// <param name="testIds"></param>
        public void SetShow(List<int> testIds)
        {
            foreach (var item in testIds)
            {
                this.context.SolvedAutomaticTests.Find(item).Show = true;
            }
            this.context.SaveChanges();
        }

        /// <summary>
        /// Set complete to definitely test
        /// </summary>
        /// <param name="testId"></param>
        public void SetComplete(int testId)
        {
            this.context.SolvedAutomaticTests.Find(testId).IsComplete = true;
            this.context.SaveChanges();
        }


        /// <summary>
        /// Get points for test
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public double GetPointForTest(SolvedAutomaticTest model)
        {
            double points = 0;
            bool flag = true;
            foreach (var item in model.SolvedQuestions)
            {
                foreach (var item2 in this.context.CloseQuestions.Find(item.CloseQuestionId).Answers.Where(x => x.Correct == true))
                {
                    if (!item.SelectedAnswers.Select(x => x.Id).ToList().Contains(item2.Id))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    points += this.context.CloseQuestions.Find(item.CloseQuestionId).Points;
                }
                flag = true;

            }
            return points;
        }

        /// <summary>
        /// /Get solved automatic test for teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ICollection<SolvedAutomaticTest> GetForTeacher(int teacherId)
        {
            return this.context.SolvedAutomaticTests.Include(x => x.Course).Where(x => x.Course.TeacherId == teacherId).ToList();
        }
    }
}
