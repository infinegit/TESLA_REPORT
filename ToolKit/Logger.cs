using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net;

namespace ToolKit
{
    public class Logger
    {
        private static Logger defaultLog;
        public static Logger DefaultLog
        {
            get
            {
                if (defaultLog == null)
                {
                    defaultLog = new Logger();
                }
                return defaultLog;
            }
        }

        public Logger()
        {
            logger = LogManager.GetLogger("");
        }

        public Logger(string logID)
        {
            logger = LogManager.GetLogger(logID);
        }

        private ILog logger;

        /// <summary>
        /// 日志记录者
        /// </summary>
        public ILog LogManage
        {
            get
            {

                return logger;
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message">信息</param>
        public void Info(object message)
        {
            LogManage.Info(addInfo + message);
        }

        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="message">调试信息</param>
        public void Debug(object message)
        {
            LogManage.Debug(addInfo + message);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">错误信息</param>
        public void Error(object message)
        {
            LogManage.Error(addInfo + message);
        }

        /// <summary>
        /// 致命日志
        /// </summary>
        /// <param name="message">致命信息</param>
        public void Fatal(object message)
        {
            LogManage.Fatal(addInfo + message);

        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message">警告信息</param>
        public void Warn(object message)
        {
            LogManage.Warn(addInfo + message);
        }

        private string addInfo
        {
            get
            {
                string rtn = "[OS:" + OS + "][Browser:" + HttpContext.Current.Request.Browser.Type + "]";
                if (HttpContext.Current.Session != null)
                {
                    rtn += "[User:" + HttpContext.Current.Session["UserID"] + "]";
                }
                return rtn;
            }
        }

        private string os;
        private string OS
        {
            get
            {
                if (string.IsNullOrEmpty(os))
                {
                    string userAgent = HttpContext.Current.Request.UserAgent;
                    if (userAgent == null)
                    {
                        os = "未知系统";
                        return os;
                    }
                    os = HttpContext.Current.Request.Browser.Platform;
                    if (userAgent.Contains("NT 6.1"))
                    {
                        os = "Windows 7";
                    }
                    if (userAgent.Contains("NT 6.0"))
                    {
                        os = "Windows Vista/Server 2008";
                    }
                    else if (userAgent.Contains("NT 5.2"))
                    {
                        os = "Windows Server 2003";
                    }
                    else if (userAgent.Contains("NT 5.1"))
                    {
                        os = "Windows XP";
                    }
                    else if (userAgent.Contains("NT 5"))
                    {
                        os = "Windows 2000";
                    }
                    else if (userAgent.Contains("Mac"))
                    {
                        os = "Mac";
                    }
                    else if (userAgent.Contains("Linux"))
                    {
                        os = "Linux";
                    }
                    else if (userAgent.Contains("SunOS"))
                    {
                        os = "SunOS";
                    }
                    else if (userAgent.Contains("Android"))
                    {
                        os = "Android";
                    }
                }
                return os;
            }
        }
    }
}
