using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.MES.Library;
using I.MES.Models;
using I.MES.Tools;
using I.Report.DAL;

namespace I.Report.Controllers
{
    public class VehicleModeController : BaseController
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MFG_VehicleMode vMode)
        {
            try
            {
                string whereStr = "and VehicleMode = '" + vMode.VehicleMode + "'";
                DataSet existVehicleModeDataSet = new ReportOP().GetAllForReport("MFG_VehicleMode", whereStr);
                var existVehicleMode = ModelHelper.DataSetToIList<MFG_VehicleMode>(existVehicleModeDataSet).FirstOrDefault();
                if (existVehicleMode != null)
                {
                    throw new Exception("Model code already exists");
                }

                ReportOP reportOp = new ReportOP();

                PrepareModel(vMode, true);
                reportOp.CreateForReport("MFG_VehicleMode", JsonHelper.ToJson(vMode));
                return Json(new { state = "success", message = "Model added successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        public ActionResult Edit(int id)
        {
            string whereStr = "and ID = " + id;
            DataSet vehicleModeDataSet = new ReportOP().GetAllForReport("MFG_VehicleMode", whereStr);
            var vehicleMode = ModelHelper.DataSetToIList<MFG_VehicleMode>(vehicleModeDataSet).FirstOrDefault();
            return View(vehicleMode);
        }

        [HttpPost]
        public ActionResult Edit(MFG_VehicleMode vMode)
        {
            try
            {
                if (!string.IsNullOrEmpty(vMode.VehicleMode))
                {

                    PrepareModel(vMode, true);
                    new ReportOP().UpdateForReport("MFG_VehicleMode", JsonHelper.ToJson(vMode));
                    return Json(new { state = "success", message = "Model added successfully" });
                }
                else
                {
                    return Json(new { state = "error", message = "Model code is empty" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                string sqlStr = "Delete from MFG_VehicleMode where ID = " + id;
                new ReportOP().DeleteForReport(sqlStr);
                return Json(new { state = "success", message = "Deletion succeeded" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }

        }

        public ActionResult GetVehicleModeList(string vehicleMode, string vehicleName)
        {
            ReportOP op = new ReportOP();

            int total = 0;
            int page = int.Parse(Request["page"]);
            #region 查询条件
            List<I.MES.Models.WhereCondition> whereList = new List<I.MES.Models.WhereCondition>();
            whereList.Add(new MES.Models.WhereCondition
            {
                Key = "ViewName",
                Value = "MFG_VehicleMode"
            });

            whereList.Add(new MES.Models.WhereCondition
            {
                Key = "SortColumn",
                Value = "VehicleMode"
            });

            whereList.Add(new MES.Models.WhereCondition
            {
                Key = "SortDirection",
                Value = "Asc"
            });

            whereList.Add(new MES.Models.WhereCondition
            {
                Key = "VehicleMode",
                ColumnName = "VehicleMode",
                Operator = "",
                Value = vehicleMode
            });
            whereList.Add(new MES.Models.WhereCondition
            {
                Key = "VehicleName",
                ColumnName = "VehicleName",
                Operator = "",
                Value = vehicleName
            });

            #endregion


            DataSet ds = op.GetReportData(whereList, page, ConstInfo.PAGE_SIZE, out total);
            var data = ModelHelper.DataSetToIList<MFG_VehicleMode>(ds, 1);

            ViewBag.TotalRecords = total;

            Hashtable ht = new Hashtable();
            ht.Add("rows", data);
            ht.Add("total", total);

            return Json(ht, JsonRequestBehavior.AllowGet);

        }

    }
}
