using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class ResultViewModel
    {
        public double CorrectAnswers { get; set; }

        public double Grade { get; set; }

        public string Description { get; set; }
    }
}