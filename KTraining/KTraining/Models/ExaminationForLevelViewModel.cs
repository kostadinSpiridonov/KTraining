using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTraining.Models
{
    public class ExaminationForLevelViewModel
    {
        [Required]
        public int LevelId { get; set; }

        public List<StudentViewModel> Students { get; set; }
    }
}