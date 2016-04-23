using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface ICourseImageService
    {
        void Add(CourseImage model);
        void Delete(int id);
        ICollection<CourseImage> GetImagesForCourse(int courseId);
        CourseImage GetById(int id);
    }

    public class CourseImageService : BaseService, ICourseImageService
    {
        /// <summary>
        /// Add course image
        /// </summary>
        /// <param name="model"></param>
        public void Add(CourseImage model)
        {
            this.context.CourseImages.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Delete course image
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var image = this.context.CourseImages.Find(id);
            this.cloudinaryService.DeleteImage(image.Source.Substring(0, image.Source.IndexOf(".")));
            this.context.CourseImages.Remove(image);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get images for definitely course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ICollection<CourseImage> GetImagesForCourse(int courseId)
        {
            return this.context.CourseImages.Where(x => x.CourseId == courseId).ToList();
        }

        /// <summary>
        /// Get course image by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseImage GetById(int id)
        {
            return this.context.CourseImages.Find(id);
        }
    }
}
