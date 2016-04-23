using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface ICourseService
    {
        int Add(Course model);
        void Remove(int id);
        Course GetById(int id);
        ICollection<Course> GetCoursesForTeacher(int teacherId);
        void AddStudentToCourse(int courseId, int studentId);
        ICollection<Course> GetAll();
        bool IfStudentParticipate(int courseId, int studentId);
        void RemoveStudent(int courseId, int studentId);
        ICollection<Course> GetCoursesForStudent(int studentId);
        ICollection<Course> LatestTenCourses();
        ICollection<Course> MostFamous();
        void Complete(int id);
        bool IsComplete(int id);
        void Update(Course model);
    }

    public class CourseService : BaseService, ICourseService
    {
        /// <summary>
        /// Add course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Course model)
        {
            this.context.Courses.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Remove course
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            var model = this.context.Courses.Find(id);
            this.context.Courses.Remove(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get course by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Course GetById(int id)
        {
            return this.context.Courses.Find(id);
        }

        /// <summary>
        /// Get courses for definitely teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ICollection<Course> GetCoursesForTeacher(int teacherId)
        {
            return this.context.Courses.Where(x => x.TeacherId == teacherId).Where(x => x.IsComplete == false).ToList();
        }

        /// <summary>
        /// Add student to course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        public void AddStudentToCourse(int courseId, int studentId)
        {
            var student = this.context.Students.Find(studentId);
            var course = this.context.Courses.Find(courseId);
            course.Students.Add(student);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get al courses
        /// </summary>
        /// <returns></returns>
        public ICollection<Course> GetAll()
        {
            return this.context.Courses.ToList();
        }

        /// <summary>
        /// Check if definitely student participate in course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool IfStudentParticipate(int courseId, int studentId)
        {
            return this.context.Courses.Find(courseId).Students.Any(x => x.Id == studentId);
        }

        /// <summary>
        /// Remove student from course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        public void RemoveStudent(int courseId, int studentId)
        {
            var student = this.context.Students.Find(studentId);
            var course = this.context.Courses.Find(courseId);
            course.Students.Remove(student);
            this.context.SaveChanges();
        }
        
        /// <summary>
        /// Get courses for definitely student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public ICollection<Course> GetCoursesForStudent(int studentId)
        {
            var courses = this.context.Courses.ToList().Where(x => x.IsComplete == false).ToList();
            var nCourses = courses.ToList();
            foreach (var item in courses)
            {
                foreach (var it in item.Students)
                {
                    if (it.Id == studentId)
                    {
                        nCourses.Remove(item);
                    }
                }
            }
            return nCourses;
        }

        /// <summary>
        /// Get Latest ten courses
        /// </summary>
        /// <returns></returns>
        public ICollection<Course> LatestTenCourses()
        {
            var courses = this.context.Courses.Where(x => x.IsComplete == false).ToList();
            courses = courses.OrderBy(x => x.CreationDate).ToList();
            courses.Reverse();
            courses = courses.Take(10).ToList();
            return courses;
        }

        /// <summary>
        /// Get most famous courses
        /// </summary>
        /// <returns></returns>
        public ICollection<Course> MostFamous()
        {
            var courses = this.context.Courses.Where(x => x.IsComplete == false).ToList();
            courses = courses.OrderBy(x => x.Students.Count).ToList();
            courses.Reverse();
            courses = courses.Take(10).ToList();
            return courses;
        }


        /// <summary>
        /// Complete course
        /// </summary>
        /// <param name="id"></param>
        public void Complete(int id)
        {
            var course = this.context.Courses.Find(id);
            double courseMark = 0;
            foreach (var item in course.Students)
            {
                foreach (var mark in item.Marks)
                {
                    if (mark.CourseId == id)
                    {
                        courseMark += mark.MarkNum;
                    }
                }
                this.context.Notifications.Add(new Notification
                {
                    UserId = item.ApplicationUserId,
                    Content = "Вие завършихте курса " + course.Name + " с " + courseMark + ".",
                    Link = "/Course/CompletedStudentCourses"

                });
                courseMark = courseMark / item.Marks.Count;
                this.context.StudentCompletedCourses.Add(
                    new StudentCompletedCourse
                    {
                        CourseId = id,
                        Mark = Math.Round(courseMark, 2),
                        StudentId = item.Id
                    });
                courseMark = 0;
            }
            while (course.Posts.Count > 0)
            {
                this.context.Posts.Remove(course.Posts.First());
            }
            while (course.Students.Count > 0)
            {
                course.Students.Remove(course.Students.First());
            }
            var atestForSolving = this.context.AutomaticTestsForSolving.Where(x => x.CourseId == id).ToList();
            while (atestForSolving.Count > 0)
            {
                this.context.AutomaticTestsForSolving.Remove(atestForSolving.First());
                atestForSolving.Remove(atestForSolving.First());
            }
            var mtestForSolving = this.context.ManualTestsForSolving.Where(x => x.CourseId == id).ToList();
            while (mtestForSolving.Count > 0)
            {
                this.context.ManualTestsForSolving.Remove(mtestForSolving.First());
                mtestForSolving.Remove(mtestForSolving.First());
            }
            course.IsComplete = true;
            this.context.SaveChanges();
        }


        /// <summary>
        /// Check whether course is complete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsComplete(int id)
        {
            return this.context.Courses.Find(id).IsComplete;
        }

        /// <summary>
        /// Update course
        /// </summary>
        /// <param name="model"></param>
        public void Update(Course model)
        {
            var course = this.context.Courses.Find(model.Id);
            course.Descrition = model.Descrition;
            course.Name = model.Name;
            this.context.SaveChanges();
        }
    }
}
