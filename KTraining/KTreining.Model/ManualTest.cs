using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class ManualTest
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Rate { get; set; }

        [Required]
        public int Time { get; set; }

        private ICollection<CloseQuestion> closeQuestions;
        private ICollection<OpenQuestion> openQuestions;

        public ManualTest()
        {
            this.closeQuestions = new HashSet<CloseQuestion>();
            this.openQuestions = new HashSet<OpenQuestion>();
        }

        public virtual ICollection<CloseQuestion> CloseQuestions
        {
            get
            {
                return this.closeQuestions;
            }
            set
            {
                this.closeQuestions = value;
            }
        }

        public virtual ICollection<OpenQuestion> OpenQuestions
        {
            get
            {
                return this.openQuestions;
            }
            set
            {
                this.openQuestions = value;
            }
        }

        [Required]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
