using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string Content { get; set; }

        public string Link { get; set; }

        public bool Seen { get; set; }
    }
}
