using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;
using I.MES.Library;
using I.MES.Models;
using I.Report.DAL;

public class BaseController : Controller
{

    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        PersistenceStore.Current.PersistenceCode = System.Web.HttpContext.Current.Session.SessionID;
        BasicProperty.ClientInfo = new ClientInformation()
        {
            CurrentSysUser = Environment.UserName,
            System = Request.UserAgent,
            Machine = Request.UserHostName,
            IP = Request.UserHostAddress,
            LogID = System.Web.HttpContext.Current.Session.SessionID,
            TransferMethod = TransferType.Json
        };
        //防注入
        var actionParameters = filterContext.ActionDescriptor.GetParameters();
        foreach (var p in actionParameters)
        {
            if (p.ParameterType == typeof(string))
            {
                if (filterContext.ActionParameters[p.ParameterName] != null)
                {
                    filterContext.ActionParameters[p.ParameterName] = HelperForHtml.FilterSql(filterContext.ActionParameters[p.ParameterName].ToString());
                }
            }
        }

        if (Request.HttpMethod.ToUpper() == "POST")
        {
            string token = Request["__RequestVerificationToken"];
            object saved = Session["RequestVerificationToken"];

            if (saved == null || saved.ToString() != token)
            {
                Session["RequestVerificationToken"] = token;
            }
            else
            {
                this.IsRepeat = true;
            }
        }

        this.PageNumber = string.IsNullOrEmpty(Request["__PageNumber"]) ? 1 : int.Parse(Request["__PageNumber"]);
        //获取User信息
        string userID = Session["UserID"] as string;
        if (string.IsNullOrEmpty(userID))
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new I.Report.Config.ReturnResult()
                    {
                        Result = -1,
                        Message = "登陆超时",
                        Data = ""

                    }
                };
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            return;
        }

        //初始化公司信息
        HttpCookie cookieCompany = Request.Cookies["CompanyCode"];
        if (!string.IsNullOrEmpty(Session["CompanyCode"] as string))
        {
            this.CompanyCode = Session["CompanyCode"] as string;
        }
        else if (cookieCompany != null)
        {
            this.CompanyCode = cookieCompany.Value;
        }
        else
        {
            FormsAuthentication.RedirectToLoginPage();
            return;
        }
        //初始化工厂
        if (!string.IsNullOrEmpty(Session["FactoryCode"] as string))
        {
            this.FactoryCode = Session["FactoryCode"] as string;
        }
        else
        {
            FormsAuthentication.RedirectToLoginPage();
            return;
        }

        this.UserID = userID;

        this.UserName = Session["UserName"] as string;
        //int sessionTimeout = Session.Timeout;
        //UserID = "EmsEngLead01"; //EmsDBZZ01 EmsMaintain01 EmsEngineer01

        string page = filterContext.RouteData.Values["Controller"] + "." + filterContext.RouteData.Values["Action"];

        //初始化Log
        Log = Logger.CurrentLog;
        string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        string actionName = filterContext.ActionDescriptor.ActionName;
        string param = "";
        foreach (var p in filterContext.ActionParameters)
        {
            param += "," + p.Key + "=" + p.Value.ToString2();
        }
        if (param != "")
        {
            param = param.Substring(1);
        }

        //获取机器名
        //this.MachineName = Request.ServerVariables["REMOTE_HOST"];
        this.MachineName = BasicProperty.ClientInfo.Machine;

        Log.Info("开始执行 Controller：" + controllerName + " Action:" + actionName + " (" + param + ")");
        base.OnActionExecuting(filterContext);

    }

    /// <summary>
    /// 登录UserAccount
    /// </summary>
    public string UserID
    {
        get;
        private set;
    }

    public Logger Log
    {
        get;
        private set;
    }

    public bool IsRepeat
    {
        get;
        private set;
    }

    private int pageNumber;
    public int PageNumber
    {
        get
        {
            return this.pageNumber;
        }
        private set
        {
            ViewBag.PageNumber = value;
            this.pageNumber = value;
        }
    }

    public string CompanyCode
    {
        get;
        private set;
    }
    public string FactoryCode
    {
        get;
        private set;
    }

    public string MachineName
    {
        private set;
        get;
    }

    /// <summary>
    /// 登录人名称
    /// </summary>
    public string UserName
    {
        private set;
        get;
    }


    public string ServerUrl
    {
        get
        {
            return (string)Session["CompanyUrl"];
        }
        set
        {
            Session["CompanyUrl"] = value;
        }
    }


    public void PrepareModel(Object instance, bool needFillValue)
    {
        try
        {
            ModelHelper.TrimModel(instance);

            if (needFillValue)
            {
                PropertyInfo[] propertyInfo = instance.GetType().GetProperties();

                #region create 3个字段
                var createMachineField = propertyInfo.Where(p => p.CanWrite && p.Name == "CreateMachine").FirstOrDefault();
                if (string.IsNullOrEmpty(createMachineField.GetValue(instance).ToString()))
                {
                    createMachineField.SetValue(instance, this.MachineName);
                }

                var createTimeField = propertyInfo.Where(p => p.CanWrite && p.Name == "CreateTime").FirstOrDefault();
                if (createTimeField.GetValue(instance).ToString() == DateTime.MinValue.ToString())
                {
                    createTimeField.SetValue(instance, DateTime.Now);
                }

                var createUserAccountField = propertyInfo.Where(p => p.CanWrite && p.Name == "CreateUserAccount").FirstOrDefault();
                if (string.IsNullOrEmpty(createUserAccountField.GetValue(instance).ToString()))
                {
                    createUserAccountField.SetValue(instance, this.UserID);
                }

                #endregion


                #region latest 3个字段
                var latestModifyMachineField = propertyInfo.Where(p => p.CanWrite && p.Name == "LatestModifyMachine").FirstOrDefault();
                if (latestModifyMachineField != null)
                { 
                latestModifyMachineField.SetValue(instance, this.MachineName);
                }

                var latestModifyTimeField = propertyInfo.Where(p => p.CanWrite && p.Name == "LatestModifyTime").FirstOrDefault();
                if (latestModifyTimeField != null)
                { 
                latestModifyTimeField.SetValue(instance, DateTime.Now);
                }

                var latestModifyUserAccountField = propertyInfo.Where(p => p.CanWrite && p.Name == "LatestModifyUserAccount").FirstOrDefault();
                if(latestModifyUserAccountField != null)
                { 
                latestModifyUserAccountField.SetValue(instance, this.UserID);
                }
                #endregion


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region 把那些ComboBox内容都先放在这里，省的每个页面写了

    /// <summary>
    /// 产品分类
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllProduceCategoryList()
    {
        return null;
    }

    /// <summary>
    /// 零件版本
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllPartVersionList()
    {
        return null;
    }

    /// <summary>
    /// 车型
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllVehicleModeList()
    {
        var data = new ProductOP().GetVehicleModeList("");
        return Json(data);
    }

    /// <summary>
    ///缺陷分类
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllDefectList()
    {
        return null;
    }

    /// <summary>
    /// 产品层级
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllProduceLevelList()
    {
        return null;
    }

    /// <summary>
    /// 扫描点
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllScanSiteCodeList()
    {
        return null;
    }

    /// <summary>
    /// 控制状态
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllControlStatusList()
    {
        return null;
    }

    /// <summary>
    /// 质量状态
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllQualityStatusList()
    {
        return null;
    }

    /// <summary>
    /// 获取当前用户的工厂权限
    /// </summary>
    /// <returns></returns>
    public ActionResult GetFactoryListByUserRole()
    {
        var data = new FactoryOp().GetFactoryListByUserRole(this.UserID);
        return Json(data);
    }

    /// <summary>
    /// 扫描操作组
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllScanOperateGroupList()
    {
        return null;
    }

    /// <summary>
    /// 获取包装类型
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllPackageTypeList()
    {
        return null;
    }

    /// <summary>
    /// 获取所有操作
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllScanActionList()
    {

        return null;
    }

    /// <summary>
    /// 获取所有库位
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllStkCodeList()
    {
        return null;
    }

    /// <summary>
    /// 获取所有仓库
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllWareHouseList()
    {
        return null;
    }

    /// <summary>
    /// 获取所有移库类型
    /// </summary>
    /// <returns></returns>
    public ActionResult GetSYS_StkTransType()
    {
        return null;
    }

    /// <summary>
    /// 获取所有生产线
    /// </summary>
    /// <returns></returns>
    public ActionResult GetAllProdLine()
    {
        return null;
    }

    /// <summary>
    /// 获取所有生产区域
    /// </summary>
    /// <returns></returns>
    public ActionResult ProducePlace()
    {
        return null;
    }

    #region 零件相关一组
    /// <summary>
    ///根据产品分类获取层级
    /// </summary>
    /// <param name="categoryCode"></param>
    /// <returns></returns>
    public ActionResult GetPartProduceLevelListByCategory(string categoryCode)
    {
        var produceLevelSet = new ReportOP().GetAllForReport("MFG_ProduceLevel", " and exists(select 1 from MFG_Part p where p.ProduceLevel = MFG_ProduceLevel.ProduceLevelCode and  p.ProduceCategory = '" + categoryCode + "')");
        var produceLevelList = ModelHelper.DataSetToIList<MFG_ProduceLevel>(produceLevelSet);
        return Json(produceLevelList);
    }

    /// <summary>
    /// 根据产品分类和产品层级获取零件
    /// </summary>
    /// <param name="categoryCode"></param>
    /// <param name="produceLevel"></param>
    /// <returns></returns>
    public ActionResult GetPartListByCategoryAndLevel(string categoryCode, string produceLevel)
    {

        var partSet = new ReportOP().GetAllForReport("MFG_Part", " and ProduceLevel = '" + produceLevel + "' and ProduceCategory = '" + categoryCode + "'");
        var partList = ModelHelper.DataSetToIList<MFG_Part>(partSet);
        var q = from p in partList
                group p by new { PartNo = p.PartNo, Description = p.PartLevel } into g
                select new
                {
                    g.Key.PartNo,
                    g.Key.Description
                };
        return Json(q);
    }

    /// <summary>
    /// 根据产品分类，产品层级和零件号获取层级
    /// </summary>
    /// <param name="categoryCode"></param>
    /// <param name="produceLevel"></param>
    /// <param name="partNo"></param>
    /// <returns></returns>
    public ActionResult GetPartVersionListByCategoryAndLevelAndPart(string categoryCode, string produceLevel, string partNo)
    {

        var partVersionSet = new ReportOP().GetAllForReport("MFG_PartVersion", " and exists (select 1 from MFG_Part where PartVersion = MFG_PartVersion.PartVersion and ProduceLevel = '" + produceLevel + "' and ProduceCategory = '" + categoryCode + "' and PartNo ='" + partNo + "')");
        //var partVersionList = null;// ModelHelper.DataSetToIList<MFG_PartVersion>(partVersionSet);
        return Json("");
    }

    #endregion

    #region category1,2,3
    public ActionResult GetPartCategory1List()
    {
        var partCategoryList = GetPartCategoryList("Category1");
        return Json(partCategoryList);
    }

    public ActionResult GetPartCategory2List()
    {
        var partCategoryList = GetPartCategoryList("Category2");
        return Json(partCategoryList);
    }

    public ActionResult GetPartCategory3List()
    {
        var partCategoryList = GetPartCategoryList("Category3");
        return Json(partCategoryList);
    }

    public List<MFG_PartCategory> GetPartCategoryList(string categoryCode)
    {
        return null;

    }
    #endregion

    #endregion

    /// <summary>
    /// 删除10天前导出、下载、打印时产生临时文件和文件夹
    /// </summary>
    public void DeleteDownLoadTempFile()
    {
        var files = new DirectoryInfo(Server.MapPath("/DownLoad/")).GetFiles()
                            .Where(p => p.CreationTime < DateTime.Now.AddDays(-10)).ToList();
        foreach (var file in files)
        {
            file.Delete();
        }

        var dirs = new DirectoryInfo(Server.MapPath("/DownLoad/")).GetDirectories()
            .Where(p => p.CreationTime < DateTime.Now.AddDays(-10)).ToList();
        foreach (var dir in dirs)
        {
            dir.Delete(true);
        }
    }
}

