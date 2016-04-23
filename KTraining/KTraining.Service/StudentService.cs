using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace KTraining.Service
{
    public interface IStudentService
    {
        Student GetById(int id);
        bool ExistStudent(string studentName);
        Student GetStudentByEmail(string email);
        Student GetStudentByAppUserId(string id);
        Student GetStudentByUsername(string username);
        ICollection<Student> GetStudentsWithoutCourse(int courseId);
        void AddTestToSolve(int studentId, int testId, string testType, int courseId);
        void RemoveAutoTestToSolve(int studentId, int testId, int courseId);
        void RemoveManualTestToSolve(int studentId, int testId, int courseId);
        void RemoveManualTestForLevelToSolve(int studentId, int testId, int courseId);
        ICollection<Student> GetStudentsWithoutCourseLevelForCourse(int levelId, int courseId);
    }

    public class StudentService : BaseService, IStudentService
    {
        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Student GetById(int id)
        {
            return this.context.Students.Find(id);
        }

        /// <summary>
        /// Get student by application user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Student GetStudentByAppUserId(string id)
        {
            return this.context.Students.Where(x => x.ApplicationUserId == id).First();
        }

        /// <summary>
        /// Check whether student exist
        /// </summary>
        /// <param name="studentName"></param>
        /// <returns></returns>
        public bool ExistStudent(string studentName)
        {
            return this.context.Students.Include(x => x.ApplicationUser).Any(x => x.ApplicationUser.UserName == studentName);
        }

        /// <summary>
        /// Get student by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Student GetStudentByEmail(string email)
        {
            return this.context.Students.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser.Email == email).First();
        }

        /// <summary>
        /// Get student by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Student GetStudentByUsername(string username)
        {
            return this.context.Students.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser.UserName == username).First();
        }

        /// <summary>
        /// Get student without definitely course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ICollection<Student> GetStudentsWithoutCourse(int courseId)
        {
            var studentsInCourse = this.context.Courses.Find(courseId).Students.ToList();
            var allStudents = this.context.Students.ToList();
            foreach (var item in studentsInCourse)
            {
                if (allStudents.Contains(item))
                {
                    allStudents.Remove(item);
                }
            }
            return allStudents;
        }

        /// <summary>
        /// Add test to solve to definitely student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="testId"></param>
        /// <param name="testType"></param>
        /// <param name="courseId"></param>
        public void AddTestToSolve(int studentId, int testId, string testType, int courseId)
        {
            if (testType == "auto")
            {
                if (!this.context.AutomaticTestsForSolving.Any(x => x.CourseId == courseId && x.StudentId == studentId && x.TestId == testId))
                {
                    if (this.context.SolvedAutomaticTests.Where(x => x.StudentId == studentId && x.TestId == testId && x.Show == false).Count() == 0)
                    {
                        this.context.AutomaticTestsForSolving.Add(
                            new AutomaticTestForSolving
                            {
                                CourseId = courseId,
                                StudentId = studentId,
                                TestId = testId
                            });
                    }
                }
            }
            else if (testType == "manual")
            {
                if (!this.context.ManualTestsForSolving.Any(x => x.CourseId == courseId && x.StudentId == studentId && x.TestId == testId))
                {
                    if (this.context.SolvedManualTests.Where(x => x.StudentId == studentId && x.TestId == testId &&x.IsChecked==false).Count() == 0)
                    {
                        this.context.ManualTestsForSolving.Add(
                            new ManualTestForSolving
                            {
                                CourseId = courseId,
                                StudentId = studentId,
                                TestId = testId
                            });
                    }
                }
            }
            this.context.SaveChanges();
        }

        /// <summary>
        /// Remove auto tests to solve from student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="testId"></param>
        /// <param name="courseId"></param>
        public void RemoveAutoTestToSolve(int studentId, int testId, int courseId)
        {
            var student = this.context.Students.Find(studentId);
            var test = student.AutomaticTestsToSolve.Where(x => x.Id == testId && x.CourseId == courseId).First();
            this.context.AutomaticTestsForSolving.Remove(test);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Remove manual test to solve from student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="testId"></param>
        /// <param name="courseId"></param>
        public void RemoveManualTestToSolve(int studentId, int testId, int courseId)
        {
            var student = this.context.Students.Find(studentId);
            var test = student.ManualTestsToSolve.Where(x => x.Id == testId && x.CourseId == courseId).First();
            this.context.ManualTestsForSolving.Remove(test);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Remove manual test for level to solve from student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="testId"></param>
        /// <param name="courseId"></param>
        public void RemoveManualTestForLevelToSolve(int studentId, int testId, int courseId)
        {
            var student = this.context.Students.Find(studentId);
            var test = student.LevelTestsToSolve.Where(x => x.Id == testId && x.CourseLevel.CourseId == courseId).First();
            this.context.LevelTestsForSolving.Remove(test);
            this.context.SaveChanges();
        }


        /// <summary>
        /// Get student without levels for course
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        ICollection<Student> IStudentService.GetStudentsWithoutCourseLevelForCourse(int levelId, int courseId)
        {
            var level = this.context.CourseLevels.Find(levelId);
            return this.context.Courses.Find(courseId).Students.Where(x => (!x.CourseLevels.Contains(level))).ToList();
        }
    }
}
