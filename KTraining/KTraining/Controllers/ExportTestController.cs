using KTraining.Resources.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace KTraining.Controllers
{
    [Authorize(Roles="Teacher")]
    public class ExportTestController : BaseController
    {
        // POST: Export automatic test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AutomaticTest(int id)
        {
            try
            {
                var document= this.exportTestService.ExportAutomaticTest(id);
                string fileName = this.automaticTestService.GetById(id).Title;
                var contentType = "application/rtf";
                var bytes = Encoding.UTF8.GetBytes(document);
                var result = new FileContentResult(bytes, contentType);
                result.FileDownloadName = fileName+".rtf";
                return result;
            }
            catch
            {
                TempData["ExportError"] = Common.ProblemTryAgain;
                return Redirect("/AutomaticTest/Show/" + id);
            }
        }
      
        // POST: Export manual test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManualTest(int id)
        {
            try
            {
                var document = this.exportTestService.ExportManualTest(id);
                string fileName = this.manualTestService.GetById(id).Title;
                var contentType = "application/rtf";
                var bytes = Encoding.UTF8.GetBytes(document);
                var result = new FileContentResult(bytes, contentType);
                result.FileDownloadName = fileName +".rtf";
                return result;
            }
            catch
            {
                TempData["ExportError"] = Common.ProblemTryAgain;
                return Redirect("/ManualTest/Show/" + id);
            }
        }
	}
}