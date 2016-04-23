using KTreining.Model;
using System;
using System.Linq;

namespace KTraining.Service
{
    public interface ICloudFileService
    {
        void Add(CloudFile model);
        CloudFile GetByName(string name);
        CloudFile GetById(int id);
        void Delete(int id);
    }

    public class CloudFileService : BaseService, ICloudFileService
    {
        /// <summary>
        /// Add cloud file
        /// </summary>
        /// <param name="model"></param>
        public void Add(CloudFile model)
        {
            this.context.CloudFiles.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get clod file by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CloudFile GetByName(string name)
        {
            return this.context.CloudFiles.Where(x => x.Source == name).First();
        }

        /// <summary>
        /// Delete cloud file
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var file = this.context.CloudFiles.Find(id);
            this.cloudinaryService.DeleteFile(file.Source);
            this.context.CloudFiles.Remove(file);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get cloud file by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CloudFile GetById(int id)
        {
            return this.context.CloudFiles.Find(id);
        }
    }
}
