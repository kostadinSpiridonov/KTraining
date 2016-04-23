using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface IPostService
    {
        void Add(Post model);
        ICollection<Post> GetPostsForCourse(int courseId);
    }

    public class PostService : BaseService, IPostService
    {
        /// <summary>
        /// Add post
        /// </summary>
        /// <param name="model"></param>
        public void Add(Post model)
        {
            this.context.Posts.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get posts for course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ICollection<Post> GetPostsForCourse(int courseId)
        {
            return this.context.Posts.Where(x => x.CourseId == courseId).ToList();
        }
    }
}
