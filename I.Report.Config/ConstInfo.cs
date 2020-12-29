using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ConstInfo
{
    public const int PAGE_SIZE = 50;

    /// <summary>
    /// 库存质量状态 
    /// 对应表：QCM_StkQualityStatus
    /// </summary>
    public struct StkQualityStatus
    {
        /// <summary>
        /// 非限制
        /// </summary>
        public const string OK = "OK";
        /// <summary>
        /// 冻结
        /// </summary>
        public const string FREEZE = "FREEZE";
        /// <summary>
        /// 待检
        /// </summary>
        public const string QCPEND = "QCPEND";
    }
    #region 发货类型
    /// <summary>
    ///发货类型
    /// </summary>
    public struct DeliveryType
    {
        /// <summary>
        /// 采购订单退货-供应商退货
        /// </summary>
        public const string DeliveryPO_Vendor = "10";
        /// <summary>
        /// 采购订单退货-公司间退货
        /// </summary>
        public const string DeliveryPO_Factory = "11";
        /// <summary>
        /// 采购订单退货-加驳发运
        /// </summary>
        public const string DeliveryPO_JB = "12";
        /// <summary>
        /// 发货单-非寄售加驳
        /// </summary>
        public const string DeliveryPO_FH = "13";
        /// <summary>
        /// 委外退货
        /// </summary>
        public const string DeliveryPO_Delegation = "15";
        /// <summary>
        /// 发货单-非寄售加驳
        /// </summary>
        public const string DeliveryPO_PUS = "16";
        /// <summary>
        /// 合资公司间发货单
        /// </summary>
        public const string DeliveryPO_JV = "18";
        /// <summary>
        /// 客户销售发运
        /// </summary>
        public const string DeliverySD_Cus = "20";
        /// <summary>
        /// 销售公司发运
        /// </summary>
        public const string DeliverySD_Company = "21";
        /// <summary>
        /// 转储发运-备料发运
        /// </summary>
        public const string DeliveryTO_Prepare = "30";
        /// <summary>
        /// 转储发运-集中发运（BTO）
        /// </summary>
        public const string DeliveryTO_BTO = "31";
        /// <summary>
        /// 转储发运-工厂间移库
        /// </summary>
        public const string DeliveryTO_FactoryBtwTrans = "32";
        /// <summary>
        /// 转储发运-工厂间退货
        /// </summary>
        public const string DeliveryTO_FactoryBtwReturn = "33";
        /// <summary>
        /// 转储发运-工厂间转储
        /// </summary>
        public const string DeliveryTO_FactoryInsideTrans = "34";

        /// <summary>
        /// 转储发运-工厂间移库(采购订单自动转换成To移库单)
        /// </summary>
        public const string DeliveryTO_AutoFactoryBtwTrans = "35";
        /// <summary>
        /// 排序发运
        /// </summary>
        public const string DeliverySort = "40";
    }
    #endregion
    /// <summary>
    /// 库存管理状态码
    /// </summary>
    public struct StkMngStatusCode
    {
        /// <summary>
        /// 正常件
        /// </summary>
        public const string Normal = "0";
        /// <summary>
        /// 委外
        /// </summary>
        public const string Delegation = "1";
        /// <summary>
        /// 寄售
        /// </summary>
        public const string Consign = "2";
    }
    /// <summary>
    /// BIN位改变类型
    /// </summary>
    public struct BinChangeType
    {
        /// <summary>
        /// 上架
        /// </summary>
        public const string UpBin = "UpBin";
        /// <summary>
        /// 下架
        /// </summary>
        public const string DownBin = "DownBin";
    }
    #region 设备管理
    /// <summary>
    /// 报警类型
    /// </summary>
    public struct EMSAlarmType
    {
        /// <summary>
        /// 机修支持,创建EM工单
        /// </summary>
        public const string Equip = "Equip";
        /// <summary>
        /// 质量支持
        /// </summary>
        public const string Quality = "Quality";
        /// <summary>
        /// 模修支持
        /// </summary>
        public const string Mould = "Mould";
        /// <summary>
        /// 工艺支持
        /// </summary>
        public const string Technic = "Technic";
        /// <summary>
        /// 物流支持
        /// </summary>
        public const string Logistic = "Logistic";
    }
    /// <summary>
    /// 工单类型
    /// </summary>
    public struct EMSOrderType
    {
        /// <summary>
        /// EM工单
        /// </summary>
        public const string EM = "EM";
        /// <summary>
        /// PD工单
        /// </summary>
        public const string PD = "PD";
        /// <summary>
        /// PM工单
        /// </summary>
        public const string PM = "PM";
        /// <summary>
        /// IPQC巡检单
        /// </summary>
        public const string IPQC = "IPQC";
        /// <summary>
        /// PDCA工单
        /// </summary>
        public const string PDCA = "PDCA";
    }
    /// <summary>
    /// 通知单类型
    /// </summary>
    public struct EMSNoticeOrderType
    {
        /// <summary>
        /// PD、PM、CI工单
        /// </summary>
        public const string PD = "M1";
        /// <summary>
        /// EM工单
        /// </summary>
        public const string EM = "M2";
        /// <summary>
        /// 巡检通知单
        /// </summary>
        public const string IPQC = "M3";
    }
    /// <summary>
    /// 工单同步表类型
    /// </summary>
    public struct EMSSynOrderType
    {
        /// <summary>
        /// EM工单
        /// </summary>
        public const string EM = "ZPM2";
        /// <summary>
        /// PD工单
        /// </summary>
        public const string PD = "ZPM3";
    }

    /// <summary>
    /// 工单同步表类型
    /// </summary>
    public struct EMSWorkFlowAction
    {
        /// <summary>
        /// 开始维修
        /// </summary>
        public const string StartMain = "StartMaintain";
        /// <summary>
        /// 维持生产
        /// </summary>
        public const string KeepProd = "KeepProd";
        /// <summary>
        /// 维修完成
        /// </summary>
        public const string MaintainComplete = "MaintainComplete";
        /// <summary>
        /// 正常关闭
        /// </summary>
        public const string Close = "Close";
        /// <summary>
        /// 下一步
        /// </summary>
        public const string Next = "Next";
        /// <summary>
        /// 不同意，生产PD工单
        /// </summary>
        public const string GoToPD = "GoToPD";
        /// <summary>
        /// 生成PD工单
        /// </summary>
        public const string CreatePD = "CreatePD";
        /// <summary>
        /// 退回
        /// </summary>
        public const string GoBack = "GoBack";
        /// <summary>
        /// 退回到创建
        /// </summary>
        public const string GoBackCreate = "GoBackCreate";
        /// <summary>
        /// 预执行PD\PM工单
        /// </summary>
        public const string PrePare = "PrePare";
        /// <summary>
        /// 取消预执行PD\PM工单
        /// </summary>
        public const string CancelPrePare = "CancelPrePare";
        /// <summary>
        /// 重做PD\PM工单
        /// </summary>
        public const string ReDo = "ReDo";
        /// <summary>
        /// 取消
        /// </summary>
        public const string Cancel = "Cancel";
        /// <summary>
        /// 跟着PD工单关闭
        /// </summary>
        public const string PDClose = "PDClose";
    }

    /// <summary>
    /// 工单同步表类型
    /// </summary>
    public struct EMSUserRoles
    {
        /// <summary>
        /// EMSDBZZ
        /// </summary>
        public const string EMSDBZZ = "EMSDBZZ";
        /// <summary>
        /// EMSEngineer
        /// </summary>
        public const string EMSEngineer = "EMSEngineer";
        /// <summary>
        /// EMSEngineerLead
        /// </summary>
        public const string EMSEngineerLead = "EMSEngineerLead";
        /// <summary>
        /// EMSMaintain
        /// </summary>
        public const string EMSMaintain = "EMSMaintain";
        /// <summary>
        /// EMSMaintainLead
        /// </summary>
        public const string EMSMaintainLead = "EMSMaintainLead";
        /// <summary>
        /// EMSProdLead
        /// </summary>
        public const string EMSProdLead = "EMSProdLead";
        /// <summary>
        /// 注塑班组长
        /// </summary>
        public const string ZS001 = "ZS001";
        /// <summary>
        /// 涂装班组长
        /// </summary>
        public const string TZ001 = "TZ001";
        /// <summary>
        /// 涂装班组长
        /// </summary>
        public const string ZP001 = "ZP001";
    }

    /// <summary>
    /// EM工单状态
    /// </summary>
    public enum EMSEMOrderStatus
    {
        /// <summary>
        /// 创建            
        /// </summary>
        Create = 1000,
        /// <summary>
        /// 开始维修            
        /// </summary>
        StartMaintain = 2000,
        /// <summary>
        /// 维持生产
        /// </summary>
        KeepProd = 3000,
        /// <summary>
        /// 维修完成
        /// </summary>
        MaintainComplete = 4000,
        /// <summary>
        /// 维持生产维修工补充资料提交
        /// </summary>
        KepProdSubmit = 5000,
        /// <summary>
        /// 维修完成维修工补充资料提交
        /// </summary>
        MaintainCompleteSubmit = 6000,
        /// <summary>
        /// 工程师已审核，提交给主管工程师
        /// </summary>
        EngineerLeadAudit = 7000,
        /// <summary>
        /// 主管工程师审核同意，关闭
        /// </summary>
        Close = 9100,
        /// <summary>
        /// 维修工取消报修
        /// </summary>
        Cancel = 9200,
        /// <summary>
        /// 维持生产关闭，转成PD
        /// </summary>
        GoToPD = 9300,
        /// <summary>
        /// 不同意处理结果，转成PD
        /// </summary>
        CreatePD = 9400,
        /// <summary>
        /// 强制关闭
        /// </summary>
        ForceClose = 9500,
    }
    /// <summary>
    /// PD、PM、CI工单状态
    /// </summary>
    public enum EMSPDOrderStatus
    {
        /// <summary>
        /// 创建            
        /// </summary>
        Create = 1000,
        /// <summary>
        /// 工程师不认可处理结果，重回工单池
        /// </summary>
        GoBack = 1100,
        /// <summary>
        /// 预执行
        /// </summary>
        Prepare = 2000,
        /// <summary>
        /// 分配到维修工
        /// </summary>
        Allocation = 3000,
        /// <summary>
        /// 维修完成，维修工补充资料提交
        /// </summary>
        MaintainCompleteSubmit = 4000,
        /// <summary>
        /// 工程师已审核，提交给主管工程师
        /// </summary>
        EngineerLeadAudit = 5000,
        /// <summary>
        /// 主管工程师审核同意，关闭
        /// </summary>
        Close = 9100,
        /// <summary>
        /// 维修工取消报修
        /// </summary>
        PDCI = 9200,
        /// <summary>
        /// 生成PDCA
        /// </summary>
        ForceClose = 9300
    }
    /// <summary>
    /// 巡检通知单状态
    /// </summary>
    public enum EMSIPQCOrderStatus
    {
        /// <summary>
        /// 创建            
        /// </summary>
        Create = 1000,
        /// <summary>
        /// 生成PD工单审核中
        /// </summary>
        PDAudit = 2000,
        /// <summary>
        /// 工程师正常关闭
        /// </summary>
        Close = 9100,
        /// <summary>
        /// 转成PD关闭
        /// </summary>
        PDClose = 9200,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 9300,
    }
    /// <summary>
    /// 通知单状态
    /// </summary>
    public enum EMSNoticeOrderStatus
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Update = 2,
        /// <summary>
        /// 完成
        /// </summary>
        Complete = 3
    }
    /// <summary>
    /// 通知单操作类型
    /// </summary>
    public enum EMSNoticeOperateType
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Update = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 3
    }

    #endregion

}

