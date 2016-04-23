using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface ILevelTestForSolvingService
    {
        void Add(LevelTestForSolving model);
        ICollection<LevelTestForSolving> GetTestsForStudent(int id);
        LevelTestForSolving GetById(int id);
        void Remove(int id);
        bool HasTest(int studentId, int courseLevelId);
    }

    public class LevelTestForSolvingService : BaseService, ILevelTestForSolvingService
    {
        /// <summary>
        /// Add level test for solving
        /// </summary>
        /// <param name="model"></param>
        public void Add(LevelTestForSolving model)
        {
            this.context.LevelTestsForSolving.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get level tests for solving for  definitely student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ICollection<LevelTestForSolving> GetTestsForStudent(int id)
        {
            return this.context.LevelTestsForSolving.Where(x => x.StudentId == id).ToList();
        }

        /// <summary>
        /// Get level test for solving by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LevelTestForSolving GetById(int id)
        {
            return this.context.LevelTestsForSolving.Find(id);
        }


        /// <summary>
        /// Remove level test for solving
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            var test = this.context.LevelTestsForSolving.Find(id);
            this.context.LevelTestsForSolving.Remove(test);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Check whether the definitely student has definitely level test for solving
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseLevelId"></param>
        /// <returns></returns>
        public bool HasTest(int studentId, int courseLevelId)
        {
            return this.context.LevelTestsForSolving.Any(x => x.CourseLevelId == courseLevelId && x.StudentId == studentId);
        }
    }
}
