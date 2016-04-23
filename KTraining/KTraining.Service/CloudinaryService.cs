using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using KTreining.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KTraining.Service
{
    public class CloudinaryService
    {
        private readonly Account account;
        private readonly Cloudinary cloudinary;

        /// <summary>
        /// Create cloudinary and sign in
        /// </summary>
        /// <param name="cloudName"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        public CloudinaryService(string cloudName, string apiKey, string apiSecret)
        {
            this.account = new Account(cloudName, apiKey, apiSecret);
            cloudinary = new Cloudinary(this.account);
        }

        /// <summary>
        /// Upload image in cloudinary
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string UploadImage(string fileName, Stream stream)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new CloudinaryDotNet.Actions.FileDescription(fileName, stream),
                UniqueFilename = true,
                Transformation = new Transformation().Quality(30)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.Uri.Segments.Last().ToString();

        }

        /// <summary>
        /// Upload file in cloudinary
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string UploadFile(string fileName, Stream stream)
        {
            var uploadParams = new RawUploadParams()
            {
                File = new CloudinaryDotNet.Actions.FileDescription(fileName, stream),
                UniqueFilename = true
            };

            var uploadResult = cloudinary.Upload(uploadParams, "raw");
            return uploadResult.Uri.Segments.Last().ToString();

        }

        /// <summary>
        /// Get image url by image name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetImageUrl(string name)
        {
            return cloudinary.Api.UrlImgUp.BuildUrl(name);

        }

        /// <summary>
        /// Get file url by file name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetFileUrl(string name)
        {
            return cloudinary.Api.Url.ResourceType("raw").Action("upload").BuildUrl(name);
        }

        /// <summary>
        /// Add source url to course images
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public ICollection<CourseImage> AddPathToCourseImageName(ICollection<CourseImage> images)
        {
            var newImages = images.ToList().ConvertAll(
                x => new CourseImage
                {
                    Id = x.Id,
                    Source = GetImageUrl(x.Source),
                });
            return newImages;
        }

        /// <summary>
        /// Add source url to question images
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public ICollection<Image> AddPathToQuestionImageName(ICollection<Image> images)
        {
            var newImages = images.ToList().ConvertAll(
                x => new Image
                {
                    Id = x.Id,
                    Source = GetImageUrl(x.Source)
                });
            return newImages;
        }

        /// <summary>
        /// Delete file from cloudinary
        /// </summary>
        /// <param name="name"></param>
        public void DeleteFile(string name)
        {
            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { name },
                Invalidate = true,
                ResourceType = ResourceType.Raw
            };
            this.cloudinary.DeleteResources(delParams);
        }

        /// <summary>
        /// Delete image from cloudinary
        /// </summary>
        /// <param name="name"></param>
        public void DeleteImage(string name)
        {
            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { name },
                Invalidate = true,
                ResourceType = ResourceType.Image
            };
            this.cloudinary.DeleteResources(delParams);
        }
    }
}
