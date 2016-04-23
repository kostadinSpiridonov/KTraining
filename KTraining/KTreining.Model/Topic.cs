using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class Topic
    {
        public int Id { get; set; }

        private ICollection<CloseQuestion> closeQuestions;
        public ICollection<OpenQuestion> openQuestions;

        public Topic()
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

        [Required]
        public string Name { get; set; }
    }
}
