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
    public class RouteSettingController : BaseController
    {
        ProdRouteOP prodRoteOP = new ProdRouteOP();
        //
        // GET: /RouteSetting/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPordLineList()
        {
            var data = prodRoteOP.getProdLineList("");
            return Json(data);
        }

        /// <summary>
        /// 新增工艺路径
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// 增加工艺路径
        /// </summary>
        /// <param name="procRote"></param>
        /// <returns></returns>
        public ActionResult AddProcRote(string procRote)
        {
            MFG_ProcRoute pr = procRote.ToEntities<MFG_ProcRoute>();
            pr.ID = StringTools.GetGUID();
            try
            {
                pr.CreateUser = this.UserID;
                pr.CreateTime = DateTime.Now;
                pr.CreateMachine = MachineName;
                pr.FactoryCode = this.FactoryCode;
                prodRoteOP.AddProcRote(pr);

                return Json(new { state = "success", message = "Added successfully！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "success", message = ex.Message });
            }
        }
        /// <summary>
        /// 添加工艺工站
        /// </summary>
        /// <param name="procStation"></param>
        /// <returns></returns>
        public ActionResult AddProcStation(string procStation)
        {
            MFG_RouteStation pr = procStation.ToEntities<MFG_RouteStation>();
            pr.ID = StringTools.GetGUID();
            try
            {
                pr.CreateUser = this.UserID;
                pr.CreateTime = DateTime.Now;
                pr.CreateMachine = MachineName;
                pr.FactoryCode = this.FactoryCode;
                prodRoteOP.addRouteStation(pr);

                return Json(new { state = "success", message = "Added successfully！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "success", message = ex.Message });
            }
        }
        /// <summary>
        /// 获取功能列表
        /// </summary>
        /// <returns></returns>
        public ActionResult getPluignList()
        {
            var data = prodRoteOP.getScanPluginList();
            return Json(data);
        }
        /// <summary>
        /// 添加工步
        /// </summary>
        /// <param name="workStep"></param>
        /// <returns></returns>
        public ActionResult AddWorkStep(string workStep)
        {
            MFG_ProdWorkSetp pr = workStep.ToEntities<MFG_ProdWorkSetp>();
            pr.ID = StringTools.GetGUID();
            try
            {

                pr.CreateUser = this.UserID;
                pr.CreateTime = DateTime.Now;
                pr.CreateMachine = MachineName;
                prodRoteOP.addProdWorkSetp(pr);

                return Json(new { state = "success", message = "Added successfully！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "success", message = ex.Message });
            }
        }

        /// <summary>
        /// 添加工步
        /// </summary>
        /// <param name="workStep"></param>
        /// <returns></returns>
        public ActionResult UpdateWorkStep(string workStep)
        {
            MFG_ProdWorkSetp pr = workStep.ToEntities<MFG_ProdWorkSetp>();
            try
            {

                pr.ModifyMachine = MachineName;
                pr.ModifyTime = DateTime.Now;
                pr.ModifyUser = this.UserID;
                prodRoteOP.updatePordWorkStep(pr);

                return Json(new { state = "success", message = "Added successfully！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "success", message = ex.Message });
            }
        }
        /// <summary>
        /// 删除工艺工站
        /// </summary>
        /// <param name="procStationID"></param>
        /// <returns></returns>
        public ActionResult deleteProcStation(string procStationID)
        {
            try
            {
                string mes = prodRoteOP.deleteRouteStation(procStationID);
                if (mes == "")
                {
                    return Json(new { state = "success", message = "Added successfully！" });
                }
                else
                {
                    return Json(new { state = "fail", message = mes });
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = "fail", message = ex.Message });
            }
        }
        /// <summary>
        /// 更新工艺工站
        /// </summary>
        /// <param name="procStation"></param>
        /// <returns></returns>
        public ActionResult updateProcStation(string procStation)
        {

            MFG_RouteStation pr = procStation.ToEntities<MFG_RouteStation>();
            try
            {
                pr.ModifyUser = this.UserID;
                pr.ModifyTime = DateTime.Now;
                pr.ModifyMachine = MachineName;

                prodRoteOP.updateRouteStation(pr);

                return Json(new { state = "success", message = "Added successfully！" });


            }
            catch (Exception ex)
            {
                return Json(new { state = "fail", message = ex.Message });
            }
        }
        /// <summary>
        /// 删除工步
        /// </summary>
        /// <param name="workStepID"></param>
        /// <returns></returns>
        public ActionResult deleteWorkStep(string workStepID)
        {
            try
            {
                prodRoteOP.deletePordWorkStep(workStepID);
                return Json(new { state = "success", message = "Added successfully！" });

            }
            catch (Exception ex)
            {
                return Json(new { state = "fail", message = ex.Message });
            }
        }


        /// <summary>
        /// 修改工艺路径
        /// </summary>
        /// <param name="procRote"></param>
        /// <returns></returns>
        public ActionResult ModifyProcRote(string procRote)
        {
            MFG_ProcRoute pr = procRote.ToEntities<MFG_ProcRoute>();
            try
            {
                MFG_ProcRoute oriProcRote = prodRoteOP.getProcRote(pr.ID);
                oriProcRote.ModifyUser = this.UserID;
                oriProcRote.ModifyTime = DateTime.Now;
                oriProcRote.ModifyMachine = MachineName;
                oriProcRote.FactoryCode = this.FactoryCode;
                oriProcRote.RoteName = pr.RoteName;
                prodRoteOP.updateProcRote(oriProcRote);

                return Json(new { state = "success", message = "Modification succeeded！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "success", message = ex.Message });
            }
        }
        /// <summary>
        /// 删除工艺路径
        /// </summary>
        /// <param name="procID"></param>
        /// <returns></returns>
        public ActionResult deleteProcRote(string procID)
        {
            try
            {
                string mes = prodRoteOP.deleteProcRote(procID);
                if (string.IsNullOrEmpty(mes))
                {
                    return Json(new { state = "success", message = "Deletion succeeded！" });
                }
                else
                {
                    return Json(new { state = "fail", message = mes });
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = "fail", message = ex.Message });
            }
        }

        /// <summary>
        /// 通过ID修改零件信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Modify(string ID)
        {
            var data = prodRoteOP.getProcRote(ID);
            return View(data);
        }

        /// <summary>
        /// 工艺要求
        /// </summary>
        /// <returns></returns>
        public ActionResult TechRequipment(string ID)
        {
            var data = prodRoteOP.getProcRote(ID);
            return View(data);
        }
        /// <summary>
        /// 根据工艺路径ID获取工艺工站
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult getRoteStationByRoteID(string ID)
        {
            var data = prodRoteOP.getRoteStationList(ID);
            return Json(data);
        }
        /// <summary>
        /// 根据工艺工站ID获取工步
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult getWorkStepByRoteStationID(string ID)
        {
            var data = prodRoteOP.getPordWorkStepList(ID).OrderBy(p =>
                {
                    if (p.RowNum.Contains('-'))
                    {
                        return p.RowNum.Length == 3 ? p.RowNum.Replace('-', '0') : p.RowNum.Replace("-", "");
                    }
                    else
                    {
                        return p.RowNum;
                    }

                }
                );
            return Json(data);
        }

        public ActionResult getWorkStepParamsByStepID(string ID)
        {
            var data = prodRoteOP.getWorkStepParamsByStepID(ID);
            return Json(data);
        }
        /// <summary>
        /// 修改工步参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult updateWorkStepParam(string param)
        {
            MFG_ProdWorkStepParam pr = param.ToEntities<MFG_ProdWorkStepParam>();
            try
            {
                prodRoteOP.updateWorkStep(pr);
                return Json(new { state = "success", message = "Modification succeeded！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "success", message = ex.Message });
            }
        }
        /// <summary>
        /// 新增工位
        /// </summary>
        /// <returns></returns>
        public ActionResult AddStation()
        {
            return View();
        }

    }
}
