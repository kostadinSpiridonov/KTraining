using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class MaterialViewModel
    {
        public string Source { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
    }

    public class ShowMaterials
    {
        public int Id { get; set; }
        public List<MaterialViewModel> Materials { get; set; }

        public string CourseName { get; set; }
    }
}