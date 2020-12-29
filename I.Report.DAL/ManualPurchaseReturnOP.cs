using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I.Report.DAL
{
    public class ManualPurchaseReturnOP
    {
        public string OrderNo { get; set; } //订单编号
        public string OrderType { get; set; }//订单类型
        public string PartNo { get; set; }//零件号
        public string RetrunNum { get; set; }//退货数量
        public string FactoryNo { get; set; }//工厂代码
        public string StockAdress { get; set; }//库位代码
        public int SupplierCode { get; set; }//供应商代码
        public string ItemType { get; set; } //特殊库存
        public DateTime RetrunTime { get; set; }//退货时间
        public string MachineName {set;get; }//退货机器编号
        public string UserID { set; get; }//操作人
    }
}
