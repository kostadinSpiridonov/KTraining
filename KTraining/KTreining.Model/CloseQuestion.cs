using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class CloseQuestion
    {
        private ICollection<CloseAnswer> answers;

        private ICollection<Image> images;
        private ICollection<AutomaticTest> automaticTests;
        private ICollection<ManualTest> manualTests;

        public CloseQuestion()
        {
            this.answers = new HashSet<CloseAnswer>();
            this.images = new HashSet<Image>();
            this.automaticTests = new HashSet<AutomaticTest>();
            this.manualTests = new HashSet<ManualTest>();
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public virtual ICollection<CloseAnswer> Answers
        {
            get
            {
                return this.answers;
            }

            set
            {
                this.answers = value;
            }
        }
        public virtual ICollection<AutomaticTest> AutomaticTests
        {
            get
            {
                return this.automaticTests;
            }

            set
            {
                this.automaticTests = value;
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

        [Required]
        public double Points { get; set; }

        public string HelpLink { get; set; }
    }
}
