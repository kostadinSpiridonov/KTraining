using KTreining.Model;
using System;
using System.Linq;
using System.Data.Entity;

namespace KTraining.Service
{
    public interface IImageService
    {
        int Add(Image model);
        void Delete(int id);
        Image GetById(int id);
        bool SearchImageForTeacher(int id, int teacherId);
    }

    public class ImageService : BaseService, IImageService
    {
        /// <summary>
        /// Add image
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Image model)
        {
            this.context.Images.Add(model);
            this.context.SaveChanges();
            return model.Id;
        }

        /// <summary>
        /// Delete image
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var image = this.context.Images.Find(id);
            this.cloudinaryService.DeleteImage(image.Source.Substring(0, image.Source.IndexOf('.')));
            this.context.Images.Remove(image);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get image by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Image GetById(int id)
        {
            return this.context.Images.Find(id);
        }

        /// <summary>
        /// Search image for teacher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public bool SearchImageForTeacher(int id, int teacherId)
        {
            var contain1 = false;
            var contain2 = false;
            var contain3 = false;
            contain1 = this.context.CloseAnswers.Include(x => x.Question).Include(x => x.Images)
                .Where(x => x.Question.Topic.TeacherId == teacherId).Any(x => x.Images.Select(t => t.Id).Contains(id));
            contain2 = this.context.CloseQuestions.Include(x => x.Topic).Include(x => x.Images)
                .Where(x => x.Topic.TeacherId == teacherId).Any(x => x.Images.Select(t => t.Id).Contains(id));
            contain3 = this.context.OpenQuestions.Include(x => x.Topic).Include(x => x.Images)
                .Where(x => x.Topic.TeacherId == teacherId).Any(x => x.Images.Select(t => t.Id).Contains(id));
            if (contain1 || contain2 || contain3)
            {
                return true;
            }
            return false;
        }
    }
}
