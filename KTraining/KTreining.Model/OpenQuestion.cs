using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class OpenQuestion
    {
        private ICollection<Image> images;
        private ICollection<ManualTest> manualTests;

        public OpenQuestion()
        {
            this.images = new HashSet<Image>();
            this.manualTests = new HashSet<ManualTest>();
        }

        public int Id { get; set; }

        public string Content { get; set; }

        [Required]
        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }

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

        public virtual ICollection<ManualTest> ManualTests
        {
            get
            {
                return this.manualTests;
            }
            set
            {
                this.manualTests = value;
            }
        }

        [Required]
        public double Points { get; set; }

        public string HelpLink { get; set; }

    }
}
