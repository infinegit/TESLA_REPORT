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
    public class PartInformationController : BaseController
    {
        PartOP op = new PartOP();
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增零件信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 通过ID修改零件信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Modify(string ID)
        {
            var data = op.GetCfgByID(Convert.ToInt32(ID));
            return View(data);
        }

        /// <summary>
        /// add part
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public ActionResult AddPart(string part)
        {
            MFG_Part pr = part.ToEntities<MFG_Part>();
            try
            {

                pr.CreateMachine = MachineName;
                pr.CreateTime = DateTime.Now;
                pr.CreateUser = this.UserID;

                op.AddPart(pr);
                return Json(new { state = "success", message = "Add Success！" });
            }

            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        /// <summary>
        /// update 
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>

        public ActionResult ModifyPart(string part)
        {

            try
            {
                MFG_Part pr = part.ToEntities<MFG_Part>();
                pr.ModifyMachine = MachineName;
                pr.ModifyTime = DateTime.Now;
                pr.ModifyUser = this.UserID;

                op.Update(pr);
                return Json(new { state = "success", message = "Edit Success！" });
            }

            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID删除零件信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult DeletePartByID(string ID)
        {
            try
            {
                op.DeletePart(Convert.ToInt32(ID));
                return Json(new { state = "success", message = "delete success！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

    }
}
