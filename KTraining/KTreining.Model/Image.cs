using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
