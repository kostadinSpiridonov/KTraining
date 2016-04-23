using KTreining.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class CourseVideoViewModel
    {
        public List<Video> Videos { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; }
    }
}