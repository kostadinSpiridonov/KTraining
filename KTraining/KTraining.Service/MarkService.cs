using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface IMarkService
    {
        void Add(Mark model);
        ICollection<Mark> MarksForStudent(int id);
        void SetSeenStudentMarks(int studentId);
        int GetUnseenMarks(int studentId);
    }
    public class MarkService : BaseService, IMarkService
    {
        /// <summary>
        /// Add mark
        /// </summary>
        /// <param name="model"></param>
        public void Add(Mark model)
        {
            model.Seen = false;
            this.context.Marks.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get marks for student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ICollection<Mark> MarksForStudent(int id)
        {
            return this.context.Marks.Where(x => x.StudentId == id).ToList();
        }

        /// <summary>
        /// Set seen student's marks
        /// </summary>
        /// <param name="studentId"></param>
        public void SetSeenStudentMarks(int studentId)
        {
            var marks = this.context.Marks.Where(x => x.StudentId == studentId);
            foreach (var item in marks)
            {
                item.Seen = true;
            }
            this.context.SaveChanges();
        }

        /// <summary>
        /// Get unseen marks
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public int GetUnseenMarks(int studentId)
        {
            return this.context.Marks.Where(x => x.StudentId == studentId && x.Seen == false).Count();
        }
    }
}
