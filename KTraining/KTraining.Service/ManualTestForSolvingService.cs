using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace KTraining.Service
{
    public interface IManualTestForSolving
    {
        ManualTestForSolving GetById(int id);
        int CountTestWithId(int id);
        ICollection<ManualTestForSolving> GetSendFromTeacher(int teacherId);
        void RemoveByStudentCourseTest(int studentId, int courseId, int testId);
    }

    public class ManualTestForSolvingService : BaseService, IManualTestForSolving
    {
        /// <summary>
        /// Get manual test for solving by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ManualTestForSolving GetById(int id)
        {
            return this.context.ManualTestsForSolving.Find(id);
        }

        /// <summary>
        /// Get count of manual test for solving by definitely test id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CountTestWithId(int id)
        {
            return this.context.ManualTestsForSolving.Where(x => x.TestId == id).Count();
        }

        /// <summary>
        /// Get manual tests for solving sent from definitely tacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ICollection<ManualTestForSolving> GetSendFromTeacher(int teacherId)
        {
            return this.context.ManualTestsForSolving.Include(x => x.Test).Where(x => x.Test.TeacherId == teacherId).ToList();
        }

        /// <summary>
        /// Remove manual test for solving by student id, course id and test id
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseId"></param>
        /// <param name="testId"></param>
        public void RemoveByStudentCourseTest(int studentId, int courseId, int testId)
        {
            var solvedTest = this.context.ManualTestsForSolving.Where(x => x.StudentId == studentId
                && x.CourseId == courseId && x.TestId == testId).FirstOrDefault();
            this.context.ManualTestsForSolving.Remove(solvedTest);
            this.context.SaveChanges();
        }
    }
}
