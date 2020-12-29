using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace I.Report
{
    //public class RouteConfig
    //{
    //    public static void RegisterRoutes(RouteCollection routes)
    //    {
    //        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

    //        routes.MapRoute(
    //            name: "Default",
    //            url: "{controller}/{action}/{id}",
    //            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
    //        );
    //    }
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "Localization", // 路由名称
            url: "{lang}/{controller}/{action}/{id}", // 带有参数的 URL
            constraints: new { lang = "zh-CN|en-US" }, //限制可输入的语言项
            defaults: new { lang = "en-US", controller = "Home", action = "Index", id = UrlParameter.Optional }//参数默认值
        );
        }
    }
}