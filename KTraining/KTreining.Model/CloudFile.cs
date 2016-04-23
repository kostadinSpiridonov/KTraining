using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class CloudFile
    {
        public int Id { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
