using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KTraining.Controllers
{
    public class HomeController : BaseController
    {
        //GET: Index page
        public ActionResult Index()
        {
            return View();
            
        }
        
        //GET: Change lanugage
        [HttpGet]
        public ActionResult Lang(string lang, string returnUrl)
        {
            Response.Cookies["_lang"].Value = lang;
            return Redirect(returnUrl);
        }

    }
}