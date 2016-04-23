using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KTreining.Model
{
    public class AutomaticTest
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Rate { get; set; }

        [Required]
        public int Time { get; set; }

        private ICollection<CloseQuestion> closeQuestions;


        public AutomaticTest()
        {
            this.closeQuestions = new HashSet<CloseQuestion>();
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

        [Required]
        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
