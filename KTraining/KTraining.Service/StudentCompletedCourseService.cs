using KTreining.Model;
using System;
using System.Linq;

namespace KTraining.Service
{
    public interface IStudentCompletedCourse
    {
        void Add(StudentCompletedCourse model);
    }

    public class StudentCompletedCourseService : BaseService, IStudentCompletedCourse
    {
        /// <summary>
        /// /Add student complete course
        /// </summary>
        /// <param name="model"></param>
        public void Add(StudentCompletedCourse model)
        {
            this.context.StudentCompletedCourses.Add(model);
            this.context.SaveChanges();
        }
    }
}
