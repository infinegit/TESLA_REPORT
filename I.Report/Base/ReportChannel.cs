using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using I.MES.GlobalCore;
using I.MES.Tools;

namespace I.Report.Base
{
    public class ReportChannel : I.MES.ClientCore.IChannel
    {
        I.MES.ClientCore.Channels.WCFChannel baseChannel;
        public ReportChannel()
        {
            string url = (string)System.Web.HttpContext.Current.Session["CompanyUrl"];
            baseChannel = new MES.ClientCore.Channels.WCFChannel(url);
        }

        #region IChannel 成员

        public event MES.Tools.CompleteEventHandler AsynCompleted;

        public void Close()
        {
            baseChannel.Close();
        }

        public void Open()
        {
            baseChannel.Open();
        }

        public object Send(object data)
        {
            BaseInformation_I info = new Resolver<BaseInformation_I>().Resolve(data);
            GetLoginInfo(info.ClientInfo);
            ICombinator __combinator = new I.MES.GlobalCore.Combinator.CSCombinator();
            Compiler __compiler = new Compiler();

            return baseChannel.Send(__compiler.Compile(__combinator.Combination(info)));
        }

        public void SendAsyn(object data)
        {
            baseChannel.SendAsyn(data);
        }

        public TransferType Transfer
        {
            get { return baseChannel.Transfer; }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            baseChannel.Dispose();
        }
        #endregion


        public void GetLoginInfo(ClientInformation ClientInfo)
        {
            if (System.Web.HttpContext.Current.Session["UserID"] != null)
            {
                if (ClientInfo != null)
                {
                    ClientInfo.LoginUser = System.Web.HttpContext.Current.Session["UserID"] as string;
                    ClientInfo.FactoryCode = System.Web.HttpContext.Current.Session["FactoryCode"] as string;
                    ClientInfo.CompanyCode = System.Web.HttpContext.Current.Session["CompanyCode"] as string;
                }
            }
            else
            {
                if (ClientInfo != null)
                {
                    ClientInfo.LoginUser = "未登录";
                    ClientInfo.FactoryCode = "未选择";
                    ClientInfo.CompanyCode = "未选择";
                }
            }
        }

    }
}