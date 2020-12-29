using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.MES.Library;
using I.MES.Tools;
using I.Report.Config;
using System.Data;
using System.Collections;
using I.MES.Models;
using System.IO;
using Aspose.Cells;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.InteropServices;

namespace I.Report.Areas.Report.Controllers
{

    /// <summary>
    /// 所有报表的公共控制器
    /// </summary>
    public class ReportController : BaseController
    {
        //
        // GET: /Report/Report/
        
        public ActionResult Index()
        {
            return View();
        }

        #region <<公共报表数据查询方法(带分页)>>
        [HttpPost]
        public ActionResult GetReportData(List<I.MES.Models.WhereCondition> whereList, string page)
        {
            try
            {
                ReportOP op = new ReportOP();
                //总记录数
                int intRowTotal = 0;
                //获取当前页码
                int intPage = string.IsNullOrWhiteSpace(page) ? 1 : int.Parse(page);

                DataSet ds = op.GetReportData(whereList, intPage, ConstInfo.PAGE_SIZE, out intRowTotal);
                var strReportJson = ToJson(ds.Tables[1]);
                //JsonHelper.ToJson(ds.Tables[1]);
                return Json(new ReturnResult
                {
                    Result = 1,
                    Message = intRowTotal.ToString(),
                    Data = strReportJson
                });
            }
            catch (Exception ex)
            {
                return Json(new ReturnResult
                {
                    Result = 0,
                    Message = ex.Message,
                    Data = ""
                });
            }
        }

        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new IsoDateTimeConverter()
            {
                DateTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss"
            });
        }
        #endregion

        #region <<获取明细>>

        public string GetReportDetail()
        {
            try
            {
                ReportOP op = new ReportOP();
                List<I.MES.Models.WhereCondition> whereList = new List<WhereCondition>();
                int intTotal = Request.QueryString.AllKeys.Count();
                string strKey = "";
                string strValue = "";
                for (int i = 0; i < intTotal; i++)
                {
                    strKey = Request.QueryString.AllKeys[i];
                    strValue = Request.QueryString[strKey];
                    if (string.IsNullOrWhiteSpace(strKey))
                    {
                        continue;
                    }
                    whereList.Add(new WhereCondition()
                    {
                        ColumnName = strKey,
                        Key = strKey,
                        Operator = "=",
                        Value = strValue
                    });
                }
                DataSet ds = op.GetAllReportData(whereList);
                /* 视图字段被修改？？ 导致添加统计行报错*/
                DataRow drr = ds.Tables[0].NewRow();
                try
                {
                    drr["PartDesc"] = "总合计";
                    drr["ReturnQty"] = ds.Tables[0].Compute("sum(ReturnQty)", "TRUE");
                    drr["RecQty"] = ds.Tables[0].Compute("sum(RecQty)", "TRUE");
                }
                catch { }
                ds.Tables[0].Rows.Add(drr);
               
                var strReportJson = ToJson(ds.Tables[0]);

                return JsonHelper.ToJson(ds.Tables[0]);

            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        #endregion

        #region <<导出EXCEL>>
        public void ExportProcOut()
        {
            string ret = "";
            try
            {
                ReportOP op = new ReportOP();
                DataSet dsExport = null;
                string strJson = Server.UrlDecode(Request["w"]);
                List<I.MES.Models.WhereCondition> whereList;
                if (!string.IsNullOrWhiteSpace(strJson))
                {
                    whereList = strJson.ToEntities<List<I.MES.Models.WhereCondition>>();

                    dsExport = op.GetAllReportDataProc(whereList);
                }

                Workbook workbook = AsposeExcelTools.DataTableToExcel2(dsExport);
                if (workbook != null)
                {
                    this.DeleteDownLoadTempFile();

                    //直接导出
                    string filename = "/DownLoad/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    workbook.Save(Server.MapPath(filename), SaveFormat.Xlsx);
                    ret = filename;


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //Response.Buffer = false;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(ret);
            Response.Flush();
            Response.End();
        }
        public void ExportOut()
        {
            string ret = "";
            try
            {
                ReportOP op = new ReportOP();
                DataSet dsExport = null;
                string strJson = Server.UrlDecode(Request["w"]);
                List<I.MES.Models.WhereCondition> whereList;
                if (!string.IsNullOrWhiteSpace(strJson))
                {
                    whereList = strJson.ToEntities<List<I.MES.Models.WhereCondition>>();

                    dsExport = op.GetAllReportData(whereList);
                }
                Workbook workbook = AsposeExcelTools.DataTableToExcel2(dsExport.Tables[0]);
                if (workbook != null)
                {
                    this.DeleteDownLoadTempFile();
                    //直接导出
                    Stream outputStream = Response.OutputStream;
                    string filename = "/DownLoad/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    workbook.Save(Server.MapPath(filename), SaveFormat.Xlsx);


                    ret = filename;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Response.Buffer = false;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(ret);
            Response.Flush();
            Response.End();
        }


        public void Export(string type,string pd)
        {
            string ret = "";
            try
            {
                ReportOP op = new ReportOP();
                DataSet dsExport = null;
                string strJson = Server.UrlDecode(Request["w"]);
                //string strjson2 = Server.UrlDecode(pd);
                List<I.MES.Models.WhereCondition> whereList;
                List<I.MES.Models.ExportCell> whereList2 ;
                if (!string.IsNullOrWhiteSpace(strJson))
                {
                    whereList = strJson.ToEntities<List<I.MES.Models.WhereCondition>>();
                    whereList2 = pd.ToEntities<List<ExportCell>>();

                    dsExport = op.GetAllReport(whereList, whereList2, type);



                }

                Workbook workbook = AsposeExcelTools.DataTableToExcel2(dsExport.Tables[1]);
                if (workbook != null)
                {
                    this.DeleteDownLoadTempFile();
                    //直接导出
                    Stream outputStream = Response.OutputStream;
                    string filename = "/DownLoad/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    workbook.Save(Server.MapPath(filename), SaveFormat.Xlsx);


                    ret = filename;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Response.Buffer = false;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(ret);
            Response.Flush();
            Response.End();
        }
        #endregion

        #region <<通过存储过程查询数据--通用>>
        [HttpPost]
        public ActionResult GetDataProc(List<I.MES.Models.WhereCondition> whereList, string page)
        {
            try
            {
                ReportOP op = new ReportOP();
                //总记录数
                int intRowTotal = 0;
                //获取当前页码
                int intPage = string.IsNullOrWhiteSpace(page) ? 1 : int.Parse(page);

                DataSet ds = op.GetAllReportDataProc(whereList);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(row);
                }
                var strReportJson = I.MES.Tools.JsonHelper.ToJson(ds.Tables[0]);
                //JsonHelper.ToJson(ds.Tables[1]);
                return Json(new ReturnResult
                {
                    Result = 1,
                    Message = ds.Tables[0].Rows.Count.ToString(),
                    Data = strReportJson
                });
            }
            catch (Exception ex)
            {
                return Json(new ReturnResult
                {
                    Result = 0,
                    Message = ex.Message,
                    Data = ""
                });
            }
        }
        #endregion

        #region <<通过存储过程查询数据--通用>>
        [HttpPost]
        public ActionResult GetPageDataProc(List<I.MES.Models.WhereCondition> whereList, string page)
        {
            try
            {
                ReportOP op = new ReportOP();
                //总记录数
                int intRowTotal = 0;
                //获取当前页码
                int intPage = string.IsNullOrWhiteSpace(page) ? 1 : int.Parse(page);
                whereList.Add(new MES.Models.WhereCondition
                {
                    Key = "page",
                    ColumnName = "page",
                    Operator = "",
                    Value = page
                });
                whereList.Add(new MES.Models.WhereCondition
                {
                    Key = "pagesize",
                    ColumnName = "pagesize",
                    Operator = "",
                    Value = ConstInfo.PAGE_SIZE.ToString()
                });
                DataSet ds = op.GetAllReportDataProc(whereList);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(row);
                }
                var strReportJson = ToJson(ds.Tables[1]);
                //JsonHelper.ToJson(ds.Tables[1]);
                return Json(new ReturnResult
                {
                    Result = 1,
                    Message = ds.Tables[0].Rows[0][0].ToString(),
                    Data = strReportJson
                });
            }
            catch (Exception ex)
            {
                return Json(new ReturnResult
                {
                    Result = 0,
                    Message = ex.Message,
                    Data = ""
                });
            }
        }
        #endregion

        #region <<通过存储过程查询数据--缺陷统计>>
        [HttpPost]
        public ActionResult GetReportDataProc(List<I.MES.Models.WhereCondition> whereList, string page)
        {
            try
            {
                ReportOP op = new ReportOP();
                //总记录数
                int intRowTotal = 0;
                //获取当前页码
                int intPage = string.IsNullOrWhiteSpace(page) ? 1 : int.Parse(page);

                DataSet ds = op.GetAllReportDataProc(whereList);
                DataTable dt = Processing(ds);
                var strReportJson = ToJson(dt);
                //JsonHelper.ToJson(ds.Tables[1]);
                return Json(new ReturnResult
                {
                    Result = 1,
                    Message = ds.Tables[0].Rows.Count.ToString(),
                    Data = strReportJson
                });
            }
            catch (Exception ex)
            {
                return Json(new ReturnResult
                {
                    Result = 0,
                    Message = ex.Message,
                    Data = ""
                });
            }
        }
        #endregion

        #region <<缺陷统计查询报表数据处理>>
        public DataTable Processing(DataSet ds)
        {

            if (ds.Tables.Count == 0)
            {
                return new DataTable();
            }
            //明细
            DataTable dtDetail = ds.Tables[0];

            //返回table
            DataTable dtReturn = ds.Tables[0].Clone();

            if (dtDetail.Rows.Count > 0)
            {
                //零件汇总 
                DataTable dtPartTotal = ds.Tables[1];
                //类别汇总
                DataTable dtVehicleMode = ds.Tables[2];

                
                //所有的零件
                var PartList =
                    dtPartTotal.AsEnumerable().Select(p => p.Field<string>("description")).Distinct();
                //所有类别
                var VehicleModeList =
                    dtVehicleMode.AsEnumerable().Select(p => p.Field<string>("VehicleMode")).Distinct();
                DataRow drOld = ds.Tables[0].NewRow();
                DataView dataView = dtVehicleMode.DefaultView;

                DataTable dtSum = dataView.ToTable(false, "DefectName");

                DataTable dtqtySum = dataView.ToTable(false, "qty");

                //类别
                foreach (var strVehicleMode in VehicleModeList)
                {

                    //类别汇总
                    var drVehicleModeTotal = dtVehicleMode.Select(string.Format("VehicleMode='{0}'", strVehicleMode));
                    //零件
                    foreach (var strPart in PartList)
                    {
                        //明细
                        //dtDetail.AsEnumerable().Where(
                        //   p => p.Field<string>("零件描述").ToString() == strPart
                        //   && p.Field<string>("类别").ToString() == strVehicleMode);
                        var drDetailList = dtDetail.Select(string.Format("零件描述='{0}' AND 类别='{1}'",
                            strPart, strVehicleMode));

                        if (drDetailList.Length >= 1)
                        {
                            var dtDetailList = drDetailList.CopyToDataTable();
                            dtReturn.Merge(dtDetailList);
                        }
                        else
                        {
                            continue;
                        }
                        //零件汇总
                        //var drPartTotalList = dtPartTotal.Select(string.Format("description='{0}'", strPart));
                        //DataRow drPartTotalNew = dtReturn.NewRow();
                        //drPartTotalNew["类别"] = strPart + "合计";
                        //foreach (var dr in drPartTotalList)
                        //{
                        //    drPartTotalNew[dr["DefectName"].ToString()] = dr["qty"];
                        //}
                        //dtReturn.Rows.Add(drPartTotalNew);
                    }

                    DataRow drVehicleNew = dtReturn.NewRow();
                    drVehicleNew["类别"] = strVehicleMode + "合计";
                    foreach (var dr in drVehicleModeTotal)
                    {
                        drVehicleNew[dr["DefectName"].ToString()] = dr["qty"];

                    }

                    dtReturn.Rows.Add(drVehicleNew);

                }

                DataRow drNew = dtReturn.NewRow();
                drNew["类别"] = "总合计";
                int count = 0;
                foreach (DataRow item in dtSum.Rows)
                {
                    drNew[item[0].ToString()] = dtVehicleMode.Compute("Sum(qty)", "DefectName='" + item[0].ToString() + "'").ToString();
                }
                string str = "";
                foreach (DataRow item in dataView.ToTable(true).Rows)
                {
                    count += Convert.ToInt32(item["qty"]);
                    str +=":"+item["qty"] + "  ";
                }
                drNew["零件描述"] = count; 
                dtReturn.Rows.Add(drNew);
            }
            return dtReturn;
        }
        #endregion
        /// <summary>
        /// 通用导出Excle
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public ActionResult Excle(DataTable dt)
        {
            string path = "~/DownLoadTemplate/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Millisecond + ".xls";
            if (dt != null)
            {
                Microsoft.Office.Interop.Excel.Application xlApp = null;
                try
                {
                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (xlApp != null)
                {
                    try
                    {
                        Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
                        object oMissing = System.Reflection.Missing.Value;
                        Microsoft.Office.Interop.Excel.Worksheet xlSheet = null;

                        xlSheet = xlBook.Worksheets[1];
                        xlSheet.Name = dt.TableName;

                        int rowIndex = 1;
                        int colIndex = 1;
                        int colCount = dt.Columns.Count;
                        int rowCount = dt.Rows.Count;

                        //列名的处理
                        for (int i = 0; i < colCount; i++)
                        {
                            xlSheet.Cells[rowIndex, colIndex] = dt.Columns[i].ColumnName;
                            colIndex++;
                        }
                        //列名加粗显示
                        xlSheet.get_Range(xlSheet.Cells[rowIndex, 1], xlSheet.Cells[rowIndex, colCount]).Font.Bold = true;
                        xlSheet.get_Range(xlSheet.Cells[rowIndex, 1], xlSheet.Cells[rowCount + 1, colCount]).Font.Name = "Arial";
                        xlSheet.get_Range(xlSheet.Cells[rowIndex, 1], xlSheet.Cells[rowCount + 1, colCount]).Font.Size = "10";
                        rowIndex++;

                        for (int i = 0; i < rowCount; i++)
                        {
                            colIndex = 1;
                            for (int j = 0; j < colCount; j++)
                            {
                                xlSheet.Cells[rowIndex, colIndex] = dt.Rows[i][j].ToString();
                                colIndex++;
                            }
                            rowIndex++;
                        }
                        xlSheet.Cells.EntireColumn.AutoFit();

                        xlApp.DisplayAlerts = false;
                        path = Path.GetFullPath(path);
                        xlBook.SaveCopyAs(path);
                        xlBook.Close(false, null, null);
                        xlApp.Workbooks.Close();
                        Marshal.ReleaseComObject(xlSheet);
                        Marshal.ReleaseComObject(xlBook);
                        xlBook = null;
                    }
                    catch (Exception ex)
                    {
                        return Json(new ReturnResult
                        {
                            Data = "",
                            Message = ex.Message,
                            Result = 1
                        });
                    }
                    finally
                    {
                        xlApp.Quit();
                        Marshal.ReleaseComObject(xlApp);
                        int generation = System.GC.GetGeneration(xlApp);
                        xlApp = null;
                        System.GC.Collect(generation);
                    }
                }
            }
            return Json(new ReturnResult
            {
                Data = "",
                Message = "导出成功",
                Result = 0
            });
        }
    }
}