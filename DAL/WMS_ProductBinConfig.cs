//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class WMS_ProductBinConfig
    {
        public int ID { get; set; }
        public string ProduceCategory { get; set; }
        public string ProduceLevel { get; set; }
        public string PartNo { get; set; }
        public string PartVersion { get; set; }
        public string BinCode { get; set; }
        public string PackingTypeCode { get; set; }
        public int Capacity { get; set; }
        public string CreateUserAccount { get; set; }
        public string CreateMachine { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int OccupiedCapacity { get; set; }
    
        public virtual WMS_Bin WMS_Bin { get; set; }
        public virtual WMS_PackingType WMS_PackingType { get; set; }
    }
}