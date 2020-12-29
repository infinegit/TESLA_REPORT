using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.MES.Library;
using I.MES.Models;
using I.Report.DAL;

namespace I.Report.Controllers
{
    public class CompanyFactoryController : BaseController
    {


        public ActionResult Index()
        {
            ComPanyOP op = new ComPanyOP();
            var data = op.GetList().FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult Update(SYS_Company company)
        {
            ComPanyOP op = new ComPanyOP();

            op.Update(company);

            return Json(new { state = "success" });
        }

        public ActionResult GetFactoryList()
        {
            var factoryList = new List<SYS_Factory>();
            var company = new ComPanyOP().GetList().FirstOrDefault();
            if (company != null)
            {
                FactoryOp op = new FactoryOp();
                factoryList = op.GetFactoryListByCompany(company.CompanyCode);
            }


            return Json(factoryList);
        }

        public ActionResult SaveFactorys(string companyCode, string datagrid)
        {
            Hashtable result = new Hashtable();
            try
            {
                List<SYS_Factory> factoryList = datagrid.ToEntities<List<SYS_Factory>>();
                #region 校验
                var q = factoryList.Where(p => string.IsNullOrEmpty(p.FactoryCode) || string.IsNullOrEmpty(p.FactoryName)).Count();
                if (q > 0)
                {
                    throw new Exception("工厂代码/工厂名称不能为空");
                }

                var r = from p in factoryList
                        group p by p.FactoryCode into g
                        where g.Count() >= 2
                        select new
                        {
                            g.Key,
                            Factory = g.Count()
                        };
                if (r != null && r.Count() > 0)
                {
                   // var t = r.First().Key;
                    throw new Exception("已存在相同的工厂代码" + r.First().Key);
                }
                #endregion


                FactoryOp op = new FactoryOp();

               // op.CreateAndUpdateFactoryList(companyCode, factoryList);

                //op.CreateAndUpdateFactoryList(companyCode, factoryList);
                result.Add("state", "OK");
                result.Add("message", "operate success！");
            }
            catch (Exception e)
            {
                result.Add("state", "Error");
                result.Add("message", "Program error，" + e.Message);
            }

            return Json(result);
        }
    }
}
