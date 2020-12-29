using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.Report.DAL;

namespace I.Report.Controllers
{
    public class FacController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index(string txtSearch = "")
        {

            RptCompanyOP op = new RptCompanyOP();

            int total;

            var data = op.GetList(txtSearch, PageNumber, ConstInfo.PAGE_SIZE, out total);

            ViewBag.TotalRecords = total;

            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RPT_Company data)
        {
            data.CreateTime = DateTime.Now;
            data.CreateUser = this.UserID;

            RptCompanyOP op = new  RptCompanyOP();
            int i = op.Create(data);

            return Json(new { state = "success" });
        }

        public ActionResult Edit(int id)
        {
            RptCompanyOP op = new RptCompanyOP();
            var data = op.GetData(id);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(RPT_Company data)
        {
            if (!string.IsNullOrEmpty(data.CompanyCode))
            {
                RptCompanyOP op = new RptCompanyOP();

                op.Update(data);
                return Json(new { state = "success" });
            }
            else
            {
                return Json(new { state = "error" });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            DAL.RptCompanyOP op = new DAL.RptCompanyOP();
            int i = op.Delete(id);
            return Json(new { state = "success" });
        }

        public ActionResult GetCompanyList(string txtSearch = "")
        {
            DAL.RptCompanyOP op = new DAL.RptCompanyOP();

            int total;

            //获取当前页码
            int page = int.Parse(Request["page"]);

            var data = op.GetCompanyList(txtSearch, page, ConstInfo.PAGE_SIZE, out total);

            Hashtable ht = new Hashtable();

            ht.Add("rows", data);
            ht.Add("total", total);

            return Json(ht, JsonRequestBehavior.AllowGet);
        }

    }
}
