using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface IVideoService
    {
        void Add(Video model);
        ICollection<Video> GetVideosForCourse(int courseId);
        Video GetById(int id);
        void Delete(int id);
    }

    public class VideoService : BaseService, IVideoService
    {
        /// <summary>
        /// Add video
        /// </summary>
        /// <param name="model"></param>
        public void Add(Video model)
        {
            this.context.Videos.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get videos for course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ICollection<Video> GetVideosForCourse(int courseId)
        {
            return this.context.Videos.Where(x => x.CourseId == courseId).ToList();
        }

        /// <summary>
        /// Get video by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Video GetById(int id)
        {
            return this.context.Videos.Find(id);
        }

        /// <summary>
        /// Delete video
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var video = GetById(id);
            this.context.Videos.Remove(video);
            this.context.SaveChanges();
        }
    }
}
