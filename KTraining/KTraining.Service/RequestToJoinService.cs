using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace KTraining.Service
{
    public interface IRequestToJoinService
    {
        void Add(RequestToJoin model);
        bool IfStudentSendRequestToCourse(int courseId, int studentId);
        ICollection<RequestToJoin> GetRequestsForTeacher(int teacherId);
        RequestToJoin GetById(int id);
        void Remove(int id);
    }

    public class RequestToJoinService : BaseService, IRequestToJoinService
    {
        /// <summary>
        /// Add request to join
        /// </summary>
        /// <param name="model"></param>
        public void Add(RequestToJoin model)
        {
            this.context.RequestsToJoin.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Check whether student sent request to definitely course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool IfStudentSendRequestToCourse(int courseId, int studentId)
        {
            return this.context.RequestsToJoin.Any(x => x.CourseId == courseId && x.SendById == studentId);
        }


        /// <summary>
        /// Get requests for teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public ICollection<RequestToJoin> GetRequestsForTeacher(int teacherId)
        {
            return this.context.RequestsToJoin.Include(x=>x.Course).Where(x => x.Course.TeacherId == teacherId).ToList();
        }

        /// <summary>
        /// Get request by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RequestToJoin GetById(int id)
        {
            return this.context.RequestsToJoin.Find(id);
        }

        /// <summary>
        /// Remove request
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            var request = this.context.RequestsToJoin.Find(id);
            this.context.RequestsToJoin.Remove(request);
            this.context.SaveChanges();
        }
    }
}
