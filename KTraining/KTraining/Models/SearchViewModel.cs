using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class SearchViewModel
    {
        public string SearchWord { get; set; }

        public int UsersPageCount { get; set; }

        public int CoursesPageCount { get; set; }
    }
}