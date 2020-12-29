using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.MES.Library;

namespace I.Report.Controllers
{
    public class CompanyController : BaseController
    {
        //
        // GET: /Company/

        public ActionResult Index(string txtSearch = "")
        {
            ComPanyOP op = new ComPanyOP();

            int total;

            var data = op.GetList(txtSearch, PageNumber, ConstInfo.PAGE_SIZE, out total);

            ViewBag.TotalRecords = total;

            return View(data);
        }

        [HttpPost]
        public ActionResult Update(int ID, string CompanyName, string CompanyFullName)
        {
            ComPanyOP op = new ComPanyOP();

            int iResult = op.Update(ID, CompanyName, CompanyFullName);

            return Json(new { state = "success"});
        }

        public ActionResult GetCompanyList()
        {
            ComPanyOP op = new ComPanyOP();

            var data = op.GetCompanyList();

            return Json(data);
        }
    }
}
