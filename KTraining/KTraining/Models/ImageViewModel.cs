using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class ShowImageViewModel
    {
        public int Id { get; set; }

        public List<ImageViewModel> Images { get; set; }

        public string CourseName { get; set; }
    }

    public class ImageViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }
    }

    public class UploadImagesForCloseQuestion
    {

        [Required]
        public int QuestionId { get; set; }

        public HttpPostedFileBase[] Images { get; set; }
    }

    public class UploadImagesForCloseAnswer
    {

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int AnswerId { get; set; }

        public HttpPostedFileBase[] Images { get; set; }
    }

    public class UploadImagesForOpenQuestion
    {
        [Required]
        public int QuestionId { get; set; }

        public HttpPostedFileBase[] Images { get; set; }
    }
}