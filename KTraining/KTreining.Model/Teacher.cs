using System;
using System.Collections.Generic;
using System.Linq;

namespace KTreining.Model
{
    public class Teacher
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        private ICollection<Course> courses;
        private ICollection<Topic> topics;
        private ICollection<AutomaticTest> automaticTests;
        private ICollection<ManualTest> manualTests;

        public Teacher()
        {
            this.courses = new HashSet<Course>();
            this.topics = new HashSet<Topic>();
            this.automaticTests = new HashSet<AutomaticTest>();
            this.manualTests = new HashSet<ManualTest>();
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

        public virtual ICollection<Topic> Topics
        {
            get
            {
                return this.topics;
            }
            set
            {
                this.topics = value;
            }
        }

        public virtual ICollection<AutomaticTest> AutomaticTests
        {
            get
            {
                return this.automaticTests;
            }
            set
            {
                this.automaticTests = value;
            }
        }

        public virtual ICollection<ManualTest> ManualTests
        {
            get
            {
                return this.manualTests;
            }
            set
            {
                this.manualTests = value;
            }
        }

    }
}
