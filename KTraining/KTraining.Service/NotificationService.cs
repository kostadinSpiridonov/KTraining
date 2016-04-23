using KTreining.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KTraining.Service
{
    public interface INotificationService
    {
        ICollection<Notification> GetForUser(string id);
        void Add(Notification model);
        void SetSeen(string userId);
    }

    public class NotificationService : BaseService, INotificationService
    {
        /// <summary>
        /// Get notifications for definitely user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ICollection<Notification> GetForUser(string id)
        {
            return this.context.Notifications.Where(x => x.UserId == id).ToList();
        }

        /// <summary>
        /// Add notification
        /// </summary>
        /// <param name="model"></param>
        public void Add(Notification model)
        {
            this.context.Notifications.Add(model);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Set seen notification for definitely user
        /// </summary>
        /// <param name="userId"></param>
        public void SetSeen(string userId)
        {
            var notifications = this.context.Notifications.Where(x => x.UserId == userId);
            foreach (var item in notifications)
            {
                item.Seen = true;
            }
            this.context.SaveChanges();
        }
    }
}
