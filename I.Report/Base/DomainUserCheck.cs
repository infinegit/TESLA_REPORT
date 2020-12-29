using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace I.Report.Base
{
    /// <summary>
    /// 域用户验证
    /// </summary>
    public class DomainUserCheck
    {
        [DllImport("advapi32.dll")]
        private static extern bool LogonUser(string lpszUsername , string lpszDomain , string lpszPassword , int dwLogonType , int dwLogonProvider , ref IntPtr phToken);
        protected void Page_Load(object sender , EventArgs e)
        {

        }

        public bool ValidateUserAccount(string AstrDomainName , string AstrDomainAccount , string AstrDomainPassword)
        {

            const int LOGON32_LOGON_INTERACTIVE = 2; //通过网络验证账户合法性

            const int LOGON32_PROVIDER_DEFAULT = 0; //使用默认的Windows 2000/NT NTLM验证方

            IntPtr tokenHandle = new IntPtr(0);
            tokenHandle = IntPtr.Zero;



            string domainName = AstrDomainName; //域 如:officedomain
            string domainAccount = AstrDomainAccount; //域帐号 如:administrator
            string domainPassword = AstrDomainPassword;//密码
            bool checkok = LogonUser(domainAccount , domainName , domainPassword , LOGON32_LOGON_INTERACTIVE , LOGON32_PROVIDER_DEFAULT , ref tokenHandle);

            return checkok;
        }
    }
}