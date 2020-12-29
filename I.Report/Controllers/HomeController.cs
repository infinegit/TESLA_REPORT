using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.MES.Models;
using I.Report.DAL;
using I.MES.Library;

namespace I.Report.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var op = new I.MES.Library.UserOP();
            //DAL.MenuOP op = new DAL.MenuOP();
            //var menuList = op.GetList().OrderBy(p => p.OrderNum).ToList();
            ViewBag.CompanyName = Session["CompanyName"];
            ViewBag.FactoryCode = Session["FactoryCode"];
            ViewBag.userAccount = Session["UserID"];
            List<SYS_RPTMenu> menuList = new List<SYS_RPTMenu>();
            if (Session["UserID"] == null )
            {
                Redirect("~/Account/Login");
            }
            else
            {
                if (Session["UserID"].ToString().ToLower().Equals("admin"))
                {
                    menuList = op.GetAllMenu().OrderBy(p=>p.OrderNum).ToList();
                }
                else
                {
                    if (Session["Permission"] != null)
                    {
                        menuList = (Session["Permission"] as List<SYS_RPTMenu>).OrderBy(p => p.OrderNum).ToList();
                    }
                }
            }
            
            return View(menuList);

        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ChangePassword(string oldPwd,string newPwd)
        {
            ViewBag.userAccount = Session["UserID"];
            I.MES.Library.UserOP op = new MES.Library.UserOP();
            bool bo= op.ChangePwd(ViewBag.userAccount, oldPwd,newPwd);
            if (bo)
            {
                return Json(new { state = "success" });

            }
            else
            {
                return Json(new { state = "error" });
                
            }
        }

    }
}
