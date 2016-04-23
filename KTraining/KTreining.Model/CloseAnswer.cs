using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KTreining.Model
{
    public class CloseAnswer
    {
        private ICollection<Image> images;

        public CloseAnswer()
        {
            this.images = new HashSet<Image>();
        }
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool Correct { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public virtual CloseQuestion Question { get; set; }

        public virtual ICollection<Image> Images
        {
            get
            {
                return this.images;
            }

            set
            {
                this.images = value;
            }
        }
    }
}
