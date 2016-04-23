using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class RemoveTrackViewModel
    {
        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public int TestId { get; set; }

        public bool IsManualTest { get; set; }

        public int CountForSeen { get; set; }
    }
}