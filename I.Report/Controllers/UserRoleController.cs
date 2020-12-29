using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.MES.Library;

namespace I.Report.Controllers
{
    public class UserRoleController : BaseController
    {
        //
        // GET: /UserRole/

        public ActionResult Index(string UserAccount)
        {
            UserRoleOP op = new   UserRoleOP();


            var data = op.GetUserRole(UserAccount);

            return View(data);

        }

        [HttpPost]
        public ActionResult Query(string Account)
        {
            UserRoleOP op = new UserRoleOP();

            var data = op.GetRoleList(Account);
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
            UserRoleOP op = new UserRoleOP();

            var data = op.GetUserList();
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
            UserRoleOP op = new UserRoleOP();

            var data = op.GetUserRoleToList(Account);

            return Json(data);
        }

        [HttpPost]
        public ActionResult UpateUserRole(string Account,string  RoList)
        {
            UserRoleOP op = new UserRoleOP();

            var data = op.UpdateUserRole(Account, RoList);

            return Json(new { state = "success" });
        }
    }
}
