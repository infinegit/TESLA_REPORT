using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace I.Report
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitialBasicInfo();
            InitializeAsposeCells();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);//注册绑定
            AuthConfig.RegisterAuth();
        }

        protected void InitialBasicInfo()
        {
            //PersistenceStore.Current.PersistenceCode = System.Web.HttpContext.Current.Session.SessionID;
            //PersistenceStore.Current.PersistenceCode = BasicProperty.Log.LogID;
            BasicProperty.ClientInfo = new ClientInformation()
            {
                CurrentSysUser = Environment.UserName,
                System = Environment.OSVersion.ToString(),
                Machine = Environment.MachineName,
                LogID = BasicProperty.Log.LogID,
                TransferMethod = TransferType.Json
            };
        }

        public delegate void Action<FieldInfo, Type, String, Object, Integer>(FieldInfo field, Type chkType, string chkName, object obj, object value);
        /// <summary>
        /// 注册Aspose控件
        /// </summary>
        internal static void InitializeAsposeCells()
        {
            const BindingFlags BINDING_FLAGS_ALL = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            const string CLASS_LICENSER = "\u0092\u0092\u0008.\u001C";
            const string CLASS_LICENSERHELPER = "\u0011\u0001\u0006.\u001A";
            const string ENUM_ISTRIAL = "\u0092\u0092\u0008.\u001B";

            const string FIELD_LICENSER_CREATED_LICENSE = "\u0001";     // static
            const string FIELD_LICENSER_EXPIRY_DATE = "\u0002";         // instance
            const string FIELD_LICENSER_ISTRIAL = "\u0001";             // instance

            const string FIELD_LICENSERHELPER_INT128 = "\u0001";        // static
            const string FIELD_LICENSERHELPER_BOOLFALSE = "\u0001";     // static

            const int CONST_LICENSER_ISTRIAL = 1;
            const int CONST_LICENSERHELPER_INT128 = 128;
            const bool CONST_LICENSERHELPER_BOOLFALSE = false;

            //- Field setter for convinient
            Action<FieldInfo, Type, string, object, object> setValue =
                delegate(FieldInfo field, Type chkType, string chkName, object obj, object value)
                {
                    if ((field.FieldType == chkType) && (field.Name == chkName))
                    {
                        field.SetValue(obj, value);
                    }
                };


            //- Get types
            Assembly assembly = Assembly.GetAssembly(typeof(Aspose.Cells.License));
            Type typeLic = null, typeIsTrial = null, typeHelper = null;
            foreach (Type type in assembly.GetTypes())
            {
                if ((typeLic == null) && (type.FullName == CLASS_LICENSER))
                {
                    typeLic = type;
                }
                else if ((typeIsTrial == null) && (type.FullName == ENUM_ISTRIAL))
                {
                    typeIsTrial = type;
                }
                else if ((typeHelper == null) && (type.FullName == CLASS_LICENSERHELPER))
                {
                    typeHelper = type;
                }
            }
            if (typeLic == null || typeIsTrial == null || typeHelper == null)
            {
                throw new Exception();
            }

            //- In class_Licenser
            object license = Activator.CreateInstance(typeLic);
            foreach (FieldInfo field in typeLic.GetFields(BINDING_FLAGS_ALL))
            {
                setValue(field, typeLic, FIELD_LICENSER_CREATED_LICENSE, null, license);
                setValue(field, typeof(DateTime), FIELD_LICENSER_EXPIRY_DATE, license, DateTime.MaxValue);
                setValue(field, typeIsTrial, FIELD_LICENSER_ISTRIAL, license, CONST_LICENSER_ISTRIAL);
            }

            //- In class_LicenserHelper
            foreach (FieldInfo field in typeHelper.GetFields(BINDING_FLAGS_ALL))
            {
                setValue(field, typeof(int), FIELD_LICENSERHELPER_INT128, null, CONST_LICENSERHELPER_INT128);
                setValue(field, typeof(bool), FIELD_LICENSERHELPER_BOOLFALSE, null, CONST_LICENSERHELPER_BOOLFALSE);
            }
        }
    }
}