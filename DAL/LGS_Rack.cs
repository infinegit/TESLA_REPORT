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
    
    public partial class LGS_Rack
    {
        public int ID { get; set; }
        public string RackCode { get; set; }
        public string PkgTypeCode { get; set; }
        public int RackStatus { get; set; }
        public string RackLocation { get; set; }
        public string CreateUserAccount { get; set; }
        public string CreateMachine { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int TSCode { get; set; }
        public int FlowType { get; set; }
    }
}