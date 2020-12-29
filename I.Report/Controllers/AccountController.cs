using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using I.MES.Library;
using I.MES.Models;
using I.Report.Config;
using I.MES.Tools;
using System.Collections.Specialized;
using System.Configuration;

namespace I.Report.Controllers
{
    public class AccountController : Controller
    {
        [AntiSqlInject]
        public ActionResult Login(string returnUrl)
        {
            int totalCompanies = 0;
            var configs = (System.Collections.IDictionary)ConfigurationManager.GetSection("companyConfig");
            var companies = new List<I.Report.Models.Company>();
            foreach (System.Collections.DictionaryEntry config in configs)
            {
                companies.Add(new Models.Company { CompanyName = config.Key.ToString(), CompanyUrl = config.Value.ToString() });

            }
            //List<DAL.RPT_Company> companies = new DAL.RptCompanyOP().GetCompanyList(string.Empty, 1, 100, out totalCompanies);

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Companies = Newtonsoft.Json.JsonConvert.SerializeObject(companies.OrderBy(c => c.CompanyName));
            return View();
        }

        [AntiSqlInject]
        public ActionResult GetCompanyFactories(string url)
        {
            try
            {
                //var op = new DAL.RptCompanyOP().GetData(systemId);
                Session["CompanyUrl"] = url;
                List<I.MES.Models.SYS_Factory> factories = new FactoryOp().GetFactoryList();
                string msg = string.Empty;
                string state = "success";
                if (factories == null || factories.Count <= 0)
                {
                    msg = "Unable to find the factory configuration under the system, system code：";
                    state = "error";
                }
                return Json(new { state = state, data = factories, message = msg });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [AntiSqlInject]
        public string Login(string userAccount, string userPassword, string systemId, string factory, string returnUrl)
        {
            try
            {
                if (Session["UserID"] != null && Session["FactoryCode"] != null)
                {
                    if (Session["UserID"].ToString() != userAccount
                        || Session["FactoryCode"].ToString() != factory)
                    {
                        return JsonHelper.ToJson(new ReturnResult()
                        {
                            Result = -2,
                            Message = "Existing user login, please close browser and login again！",
                        });
                    }
                }
                //company = company.Equals("TEST") ? "PD" : company;
                var company = new MES.Library.ComPanyOP().GetCompanyList().FirstOrDefault();
                //DAL.RPT_Company company = new DAL.RptCompanyOP().GetData(systemId);
                FormsAuthentication.SetAuthCookie(userAccount, false);
                //检查账号密码
                var mesUser = new MES.Library.UserOP().GetUser(userAccount);
                List<SYS_Factory> mesFactory = new MES.Library.UserOP().GetUserAvailFactories(userAccount);
                int num = 0;
                SYS_Factory factoryname = new MES.Library.UserOP().GetFactory(factory);
                string name = "";
                if (factoryname!=null)
                {
                    name = factoryname.FactoryName;
                }
                foreach (var item in mesFactory)
                {
                    if (factory==item.FactoryCode)
                    {
                        num = 1;
                    }
                }
                if (mesUser == null)
                {
                    return JsonHelper.ToJson(new ReturnResult()
                    {
                        Result = 0,
                        Message = string.Format("No such user({0})", userAccount),
                    });
                }
                if (mesUser.Password != userPassword)
                {
                    return JsonHelper.ToJson(new ReturnResult()
                    {
                        Result = 0,
                        Message = "Password error",
                    });
                }
                if (num == 0)
                {
                    return JsonHelper.ToJson(new ReturnResult()
                    {
                        Result = 0,
                        Message = string.Format("The user ({0}) does not have ({1}) permissions", userAccount, name),
                    });
                }
                //Report菜单权限
                //Session["Permission"] = (new I.Report.DAL.UserOP()).GetUserPermission(userAccount);
                Session["Permission"] = new I.MES.Library.UserOP().GetUserMenuPrivs(userAccount);
                Session["UserID"] = userAccount;
                //Session["SystemID"] = systemId;
                Session["CompanyCode"] = company.CompanyCode;
                Session["CompanyName"] = company.CompanyName;
                Session["FactoryCode"] = factory;
                Session["UserName"] = mesUser.UserName;

                //return RedirectToAction("Index", "Home");
                return JsonHelper.ToJson(new ReturnResult()
                {
                    Result = 1
                });
            }
            catch (Exception ex)
            {
                return JsonHelper.ToJson(new ReturnResult()
                {
                    Result = 0,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [AntiSqlInject]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        #region 帮助程序
        [AntiSqlInject]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region <<退出系统>>
        /// <summary>
        /// 安全退出
        /// </summary>
        /// <returns></returns>
        [AntiSqlInject]
        public ActionResult OutLogin()
        {
            try
            {
                //清空当前登录用户信息
                Session.Abandon();
                Session.Clear();
                var strJson = Json(new ReturnResult()
                {
                    Result = 1
                });
                return strJson;
            }
            catch (Exception ex)
            {
                return Json(new ReturnResult()
                {
                    Result = 0,
                    Message = ex.Message
                });
            }
        }
        #endregion
    }
}
