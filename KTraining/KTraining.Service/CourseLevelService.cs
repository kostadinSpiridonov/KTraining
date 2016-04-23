using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface ICourseLevelService
    {
        void Add(CourseLevel model);
        ICollection<CourseLevel> GetForCourse(int id);
        CourseLevel GetById(int id);
        void Update(CourseLevel model);
        void AddCourseLevelToStudent(int courseLevelId, int studentId);
    }

    public class CourseLevelService : BaseService, ICourseLevelService
    {
        /// <summary>
        /// Add course level
        /// </summary>
        /// <param name="model"></param>
        public void Add(CourseLevel model)
        {
            this.context.CourseLevels.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get levels for definitely course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ICollection<CourseLevel> GetForCourse(int id)
        {
            return this.context.CourseLevels.Where(x => x.CourseId == id).ToList();
        }

        /// <summary>
        /// Get level by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseLevel GetById(int id)
        {
            return this.context.CourseLevels.Find(id);
        }

        /// <summary>
        /// Update course level
        /// </summary>
        /// <param name="model"></param>
        public void Update(CourseLevel model)
        {
            var level = this.context.CourseLevels.Find(model.Id);
            level.AutomaticTestId = model.AutomaticTestId;
            level.Description = model.Description;
            level.ManualTestId = model.ManualTestId;
            level.Name = model.Name;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Add level to definitely student
        /// </summary>
        /// <param name="courseLevelId"></param>
        /// <param name="studentId"></param>
        public void AddCourseLevelToStudent(int courseLevelId, int studentId)
        {
            var student = this.context.Students.Find(studentId);
            var level = this.context.CourseLevels.Find(courseLevelId);
            student.CourseLevels.Add(level);
            this.context.SaveChanges();
        }
    }
}
