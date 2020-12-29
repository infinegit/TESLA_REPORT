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
    public class UserFactoryController : BaseController
    {
        FactoryOp op = new FactoryOp();
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
        public ActionResult AddCfg(string UserAccount, string FactoryCode, string Enabled, string IsDefault)
        {
            try
            {
                var User = op.GetUserByUserAccount(UserAccount);
                if (User == null)
                    return Json(new { state = "error", message = "Add failed！Reason: this user does not exist。" });

                var Factory = op.GetFactoryByFactoryCode(FactoryCode);
                if (Factory == null)
                    return Json(new { state = "error", message = "Add failed！Reason: this user does not exist。" });

                SYS_UserFactoryPriv cfg = new SYS_UserFactoryPriv();
                cfg.UserAccount = UserAccount;
                cfg.FactoryCode = FactoryCode;
                cfg.Enabled = Enabled == "0" ? false : true;
                cfg.IsDefault = IsDefault == "0" ? false : true;

                op.InsertCfg(cfg);
                return Json(new { state = "success", message = "Modification succeeded！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public ActionResult ModifyCfg(string ID, string UserAccount, string FactoryCode, string Enabled, string IsDefault)
        {
            try
            {
                var User = op.GetUserByUserAccount(UserAccount);
                if (User == null)
                    return Json(new { state = "error", message = "Add failed！Reason: this user does not exist。" });

                var Factory = op.GetFactoryByFactoryCode(FactoryCode);
                if (Factory == null)
                    return Json(new { state = "error", message = "Add failed！Reason: this user does not exist。" });

                SYS_UserFactoryPriv cfg = new SYS_UserFactoryPriv();
                cfg.ID = Convert.ToInt32(string.IsNullOrEmpty(ID) ? "0" : ID);
                cfg.UserAccount = UserAccount;
                cfg.FactoryCode = FactoryCode;
                cfg.Enabled = Enabled == "0" ? false : true;
                cfg.IsDefault = IsDefault == "0" ? false : true;
                
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
