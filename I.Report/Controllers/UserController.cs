using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.MES.Library;
using I.MES.Models;
using I.MES.Tools;
using I.Report.Config;
using I.Report.DAL;

namespace I.Report.Controllers
{
    public class UserController : BaseController
    {
        I.MES.Library.UserOP op = new I.MES.Library.UserOP();
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Modify(string ID)
        {
            var data = op.GetCfgByID(Convert.ToInt32(ID));
            return View(data);
        }

        /// <summary>
        /// 新增
        /// </summary>
        public ActionResult AddCfg(string UserAccount, string UserName, string UserWorkID, string Password, string IsActive)
        {
            try
            {
                SYS_User cfg = new SYS_User();
                cfg.UserAccount = UserAccount;
                cfg.UserName = UserName;
                cfg.UserWorkID = UserWorkID;
                cfg.Password = Password;
                cfg.IsActive = IsActive == "0" ? false : true;
                cfg.LatestLoginTime = DateTime.Now;
                cfg.LatestLoginMachine = this.MachineName;

                op.InsertCfg(cfg);
                return Json(new { state = "success", message = "Added successfully！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public ActionResult ModifyCfg(string ID, string UserAccount, string UserName, string UserWorkID, string Password, string IsActive)
        {
            try
            {
                SYS_User cfg = new SYS_User();
                cfg.UserAccount = UserAccount;
                cfg.UserName = UserName;
                cfg.UserWorkID = UserWorkID;
                cfg.Password = Password;
                cfg.IsActive = IsActive == "0" ? false : true;
                cfg.ID = Convert.ToInt32(string.IsNullOrEmpty(ID) ? "0" : ID);

                op.UpdateCfg(cfg);
                return Json(new { state = "success", message = "Modification succeeded！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID删除配置信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult DeleteCfg(string ID)
        {
            try
            {
                op.DeleteCfg(Convert.ToInt32(ID));
                return Json(new { state = "success", message = "Deletion succeeded！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

    }
}
