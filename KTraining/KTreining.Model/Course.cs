using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace KTreining.Model
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        private ICollection<Student> students;
        private ICollection<Post> posts;
        private ICollection<RequestToJoin> requestsToJoin;
        private ICollection<CloudFile> materials;
        private ICollection<CourseImage> images;
        private ICollection<CourseLevel> levels;

        public Course()
        {
            this.students = new HashSet<Student>();
            this.posts = new HashSet<Post>();
            this.requestsToJoin = new HashSet<RequestToJoin>();
            this.materials = new HashSet<CloudFile>();
            this.images = new HashSet<CourseImage>();
            this.levels = new HashSet<CourseLevel>();
        }

        public virtual ICollection<Student> Students
        {
            get
            {
                return this.students;
            }
            set
            {
                this.students = value;
            }
        }

        public virtual ICollection<Post> Posts
        {
            get
            {
                return this.posts;
            }
            set
            {
                this.posts = value;
            }
        }

        public virtual ICollection<RequestToJoin> RequestsToJoin
        {
            get
            {
                return this.requestsToJoin;
            }
            set
            {
                this.requestsToJoin = value;
            }
        }

        public virtual ICollection<CloudFile> Materials
        {
            get
            {
                return this.materials;
            }
            set
            {
                this.materials = value;
            }
        }

        public virtual ICollection<CourseImage> Images
        {
            get
            {
                return this.images;
            }
            set
            {
                this.images = value;
            }
        }

        public virtual ICollection<CourseLevel> Levels
        {
            get
            {
                return this.levels;
            }
            set
            {
                this.levels = value;
            }
        }

        [Required]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public string Descrition { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public bool IsComplete { get; set; }
    }
}
