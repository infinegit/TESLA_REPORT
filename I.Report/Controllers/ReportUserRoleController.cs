using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.Report.DAL;

namespace I.Report.Controllers
{
    public class ReportUserRoleController : BaseController
    {
        //
        // GET: /ReportUserRole/

        public ActionResult Index(string UserAccount)
        {
            //UserOP op = new UserOP();
            //var data = op.GetList(UserAccount);
            ViewBag.User = UserAccount;
            return View();

        }

        [HttpPost]
        public ActionResult Query(string Account)
        {
            RptRoleOP op = new RptRoleOP();
            RptUserRoleOP Uop = new RptUserRoleOP();
            List<RPT_UserRole> RoleLst = Uop.GetListByUser(Account).ToList();
            var data = op.GetList(RoleLst);
            List<SelectListItem> sss = new List<SelectListItem>();

            foreach (var v in data)
            {
                sss.Add(new SelectListItem()
                {
                    Text = v.RoleName,
                    Value = v.RoleCode
                });
            }
            return Json(sss);
        }

        [HttpPost]
        public ActionResult QueryUser()
        {
            DAL.UserOP op = new DAL.UserOP();
            var data = op.GetList("");
            List<SelectListItem> sss = new List<SelectListItem>();

            sss.Add(new SelectListItem()
            {
                Text = "--Please select--",
                Value = "",
                Selected = true
            });
            foreach (var v in data)
            {
                sss.Add(new SelectListItem()
                {
                    Text = v.UserName,
                    Value = v.UserAccount.ToString(),
                    Selected = true
                });
            }
            return Json(sss);
        }

        [HttpPost]
        public ActionResult GetUserRoleToList(string Account)
        {
            RptUserRoleOP op = new RptUserRoleOP();
            var data = op.GetListByUser(Account);

            return Json(data);
        }

        [HttpPost]
        public ActionResult UpateUserRole(string Account, string RoList)
        {
            RptUserRoleOP op = new RptUserRoleOP();
            int resule = op.Update(Account, RoList, this.UserID);
            if(resule!=0)
            return Json(new { state = 1 });
            else
            return Json(new { state = "fail" });
        }

    }
}
