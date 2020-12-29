using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I.Report.Config
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [Serializable]
    public class ReturnResult
    {
        /// <summary>
        /// 返回结果 0 ：失败 1 ：成功 -1:超时
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data1 { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data2 { get; set; }
         
    }
}
