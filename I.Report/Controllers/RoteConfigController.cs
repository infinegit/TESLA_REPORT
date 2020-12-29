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

namespace I.Report.Controllers
{
    public class RoteConfigController : BaseController
    {
        RoteConfigOP rop = new RoteConfigOP();
        // GET: /RoteConfig/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// 添加工艺配置
        /// </summary>
        /// <param name="roteConfig"></param>
        /// <returns></returns>
        public ActionResult AddRoteConfig(string roteConfig)
        {
            MFG_RoteConfig rc = roteConfig.ToEntities<MFG_RoteConfig>();

            try
            {
                rc.ID = StringTools.GetGUID();
                rc.VicheModel = "";
                rc.CreateUser = this.UserID;
                rc.CreateTime = DateTime.Now;
                rc.CreateMachine = MachineName;
                rop.addProdRoteConfig(rc);

                return Json(new { state = "success", message = "Added successfully！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "success", message = ex.Message });
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="roteConfigID"></param>
        /// <returns></returns>
        public ActionResult deleteRoteConfig(string roteConfigID)
        {
            try
            {
                rop.deleteRoteConfigByID(roteConfigID);

                return Json(new { state = "success", message = "Added successfully！" });

            }
            catch (Exception ex)
            {
                return Json(new { state = "fail", message = ex.Message });
            }
        }
        /// <summary>
        /// 获取工艺列表
        /// </summary>
        /// <returns></returns>
        public ActionResult getRouteList()
        {
            return Json(new ProdRouteOP().getProcRoteList(""));
        }
        /// <summary>
        /// 获取零件列表
        /// </summary>
        /// <returns></returns>
        public ActionResult getPartList()
        {
            return  Json(new PartOP().GetPartList());
        }
    }
}
