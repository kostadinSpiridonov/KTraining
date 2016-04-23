using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class ShowMarkViewModel
    {
        public string CourseName { get; set; }

        public double MarkValue { get; set; }

        public string Date { get; set; }

        public int CourseId { get; set; }

        public string Descrition { get; set; }
    }
}