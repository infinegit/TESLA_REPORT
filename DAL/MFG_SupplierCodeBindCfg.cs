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
    
    public partial class MFG_SupplierCodeBindCfg
    {
        public int ID { get; set; }
        public string CfgName { get; set; }
        public string Description { get; set; }
        public string MatchValue { get; set; }
        public int CodeLength { get; set; }
        public int CodeStartPosition { get; set; }
        public int MatchValueLength { get; set; }
        public bool Enabled { get; set; }
        public string CreateUserAccount { get; set; }
        public string CreateMachine { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<bool> isUnique { get; set; }
    }
}