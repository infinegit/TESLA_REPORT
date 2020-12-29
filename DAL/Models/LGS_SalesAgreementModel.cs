using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*****************************************************************************
*-----------------------------------------------------------------------------
*文 件 名: 
*计算机名：MAZEN-PC
*开发人员：(张茂忠)Mazen
*创建时间：2016/3/15 14:27:05
*描述说明：
*
*-----------------------------------------------------------------------------
* 版   本： V1.0          修改时间：           修改人： 
*更改历史：
*
*-----------------------------------------------------------------------------
*Copyright (C) 20013-2015 东尚信息科技有限公司
*-----------------------------------------------------------------------------
******************************************************************************/
namespace DAL.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LGS_SalesAgreementModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 行项目号
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string SaleAgrmNo { get; set; }
        /// <summary>
        /// 计划行号
        /// </summary>
        public int PlanItemNo { get; set; }
        /// <summary>
        /// 零件号
        /// </summary>
        public string PartNo { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 已发运数量
        /// </summary>
        public decimal DeliveryQty { get; set; }
        /// <summary>
        /// 发运数量
        /// </summary>
        public decimal ShipmentQty { get; set; }
        /// <summary>
        /// 发运库位
        /// </summary>
        public string StkCode { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryCode { get; set; }
        /// <summary>
        /// 交货日期（计划日期）
        /// </summary>
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        /// <summary>
        /// 是否交货冻结
        /// </summary>
        public Nullable<bool> IsFreeze { get; set; }
        /// <summary>
        /// 预留
        /// </summary>
        public string Reserve1 { get; set; }
        /// <summary>
        /// 出门数量
        /// </summary>
        public decimal LeaveQty { get; set; }
        /// <summary>
        /// 销售凭证类型
        /// </summary>
        public string SaleDocType { get; set; }
        /// <summary>
        /// 销售业务模式
        /// </summary>
        public string SalePattern { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BussinesType { get; set; }
    }
}
