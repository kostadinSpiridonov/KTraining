using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace KTraining.Service
{
    public interface IAutomaticTestForSolvingService
    {
        AutomaticTestForSolving GetById(int id);
        int CountTestWithId(int id);
        ICollection<AutomaticTestForSolving> GetSendFromTeacher(int teacherId);
        void RemoveByStudentCourseTest(int studentId, int courseId, int testId);
    }

    public class AutomaticTestForSolvingService : BaseService, IAutomaticTestForSolvingService
    {
        /// <summary>
        /// Get automatic test for solving by if
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AutomaticTestForSolving GetById(int id)
        {
            return this.context.AutomaticTestsForSolving.Find(id);
        }

        /// <summary>
        /// Get count of automatic test for solving with definitely test id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CountTestWithId(int id)
        {
            return this.context.AutomaticTestsForSolving.Where(x => x.TestId == id).Count();
        }

        /// <summary>
        /// Get automatic tests for solving which are sent by definitely teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ICollection<AutomaticTestForSolving> GetSendFromTeacher(int teacherId)
        {
            return this.context.AutomaticTestsForSolving.Include(x=>x.Test).Where(x => x.Test.TeacherId == teacherId).ToList();
        }

        /// <summary>
        /// Remove automatic test for solving by student id, course id and test id
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseId"></param>
        /// <param name="testId"></param>
        public void RemoveByStudentCourseTest(int studentId, int courseId, int testId)
        {
            var solvedTest = this.context.AutomaticTestsForSolving.Where(x => x.StudentId == studentId
                && x.CourseId == courseId && x.TestId == testId).FirstOrDefault();
            this.context.AutomaticTestsForSolving.Remove(solvedTest);
            this.context.SaveChanges();
        }
    }
}
