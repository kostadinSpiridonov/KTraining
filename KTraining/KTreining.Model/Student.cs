using System;
using System.Collections.Generic;
using System.Linq;

namespace KTreining.Model
{
    public class Student
    {

        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        private ICollection<Course> courses;
        private ICollection<AutomaticTestForSolving> automaticTestsToSolve;
        private ICollection<ManualTestForSolving> manualTestsToSolve;
        private ICollection<SolvedAutomaticTest> solvedAutomaticTests;
        private ICollection<SolvedManualTest> solvedManualTests;
        private ICollection<Mark> marks;
        private ICollection<StudentCompletedCourse> completedCourses;
        private ICollection<LevelTestForSolving> levelTestsToSolve;
        private ICollection<CourseLevel> courseLevels;

        public Student()
        {
            this.courses = new HashSet<Course>();
            this.automaticTestsToSolve = new HashSet<AutomaticTestForSolving>();
            this.manualTestsToSolve = new HashSet<ManualTestForSolving>();
            this.solvedAutomaticTests = new HashSet<SolvedAutomaticTest>();
            this.solvedManualTests = new HashSet<SolvedManualTest>();
            this.levelTestsToSolve = new HashSet<LevelTestForSolving>();
            this.courseLevels = new HashSet<CourseLevel>();
        }

        public virtual ICollection<CourseLevel> CourseLevels
        {
            get
            {
                return this.courseLevels;
            }
            set
            {
                this.courseLevels = value;
            }
        }

        public virtual ICollection<Course> Courses
        {
            get
            {
                return this.courses;
            }
            set
            {
                this.courses = value;
            }
        }

        public virtual ICollection<ManualTestForSolving> ManualTestsToSolve
        {
            get
            {
                return this.manualTestsToSolve;
            }
            set
            {
                this.manualTestsToSolve = value;
            }
        }

        public virtual ICollection<AutomaticTestForSolving> AutomaticTestsToSolve
        {
            get
            {
                return this.automaticTestsToSolve;
            }
            set
            {
                this.automaticTestsToSolve = value;
            }
        }

        public virtual ICollection<LevelTestForSolving> LevelTestsToSolve
        {
            get
            {
                return this.levelTestsToSolve;
            }
            set
            {
                this.levelTestsToSolve = value;
            }
        }

        public virtual ICollection<SolvedAutomaticTest> SolvedAutomaticTests
        {
            get
            {
                return this.solvedAutomaticTests;
            }
            set
            {
                this.solvedAutomaticTests = value;
            }
        }

        public virtual ICollection<Mark> Marks
        {
            get
            {
                return this.marks;
            }
            set
            {
                this.marks = value;
            }
        }

        public virtual ICollection<SolvedManualTest> SolvedManualTests
        {
            get
            {
                return this.solvedManualTests;
            }
            set
            {
                this.solvedManualTests = value;
            }
        }

        public virtual ICollection<StudentCompletedCourse> CompletedCourses
        {
            get
            {
                return this.completedCourses;
            }
            set
            {
                this.completedCourses = value;
            }
        }
    }
}
