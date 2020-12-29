using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YFPO.Report.Views
{
    public class ShowReportController : Controller
    {
        //
        // GET: /ShowReport/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetURL()
        {
            string url = Request.QueryString["url"];
            return Json(url);
        }

    }
}
