using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KTraining.Resources.Controllers;

namespace KTraining.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        // GET: /Notification/
        public ActionResult Index()
        {
            var userId = this.User.Identity.GetUserId();
            var notifications = this.notificationService.GetForUser(userId).Reverse();
            foreach (var item in notifications)
            {
                item.Content = this.convertResource.ConvertContentCode(item.Content);
            }
            return View(notifications);
        }

        //GET: Get count of notifications
        public JsonResult CountNotifications()
        {
            var userId = this.User.Identity.GetUserId();
            var notifications = this.notificationService.GetForUser(userId).Where(x => x.Seen == false).ToList(); 
            return Json(notifications.Count, JsonRequestBehavior.AllowGet);
        }

        //GET:Set seen notification
        public ActionResult SetSeen()
        {
            var userId = this.User.Identity.GetUserId();
            this.notificationService.SetSeen(userId);
            return null;
        }

    }
}