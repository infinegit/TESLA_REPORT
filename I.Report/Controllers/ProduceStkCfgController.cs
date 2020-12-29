using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YFPO.MES.Library;
using YFPO.MES.Models;
using YFPO.MES.Tools;
using YFPO.Report.Config;
using YFPO.Report.DAL;

namespace YFPO.Report.Controllers
{
    public class ProduceStkCfgController : BaseController
    {
        ProcessScanOP op = new ProcessScanOP();
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
        public ActionResult AddCfg(string ScanSiteCode, string ScanAction, string ProdLineCode, string IssStk, string RctStk, string PartVersion, string PartNo)
        {
            try
            {
                MFG_ProduceStkCfg cfg = new MFG_ProduceStkCfg();
                cfg.FactoryCode = this.FactoryCode;
                cfg.ScanSiteCode = ScanSiteCode;
                cfg.ScanAction = ScanAction;
                cfg.ProdLineCode = ProdLineCode;
                cfg.IssStk = IssStk;
                cfg.RctStk = RctStk;
                cfg.PartVersion = PartVersion;
                cfg.PartNo = PartNo;

                op.InsertCfg(cfg);
                return Json(new { state = "success", message = "添加成功！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public ActionResult ModifyCfg(string ID, string ScanSiteCode, string ScanAction, string ProdLineCode, string IssStk, string RctStk, string PartVersion, string PartNo)
        {
            try
            {
                MFG_ProduceStkCfg cfg = new MFG_ProduceStkCfg();
                cfg.ID = Convert.ToInt32(string.IsNullOrEmpty(ID) ? "0" : ID);
                cfg.ScanSiteCode = ScanSiteCode;
                cfg.ScanAction = ScanAction;
                cfg.ProdLineCode = ProdLineCode;
                cfg.IssStk = IssStk;
                cfg.RctStk = RctStk;
                cfg.PartVersion = PartVersion;
                cfg.PartNo = PartNo;
                
                op.UpdateCfg(cfg);
                return Json(new { state = "success", message = "修改成功！" });
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
                return Json(new { state = "success", message = "删除成功！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

    }
}
