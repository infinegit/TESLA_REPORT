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
    
    public partial class MFG_ProdLine
    {
        public int ID { get; set; }
        public string ProdLineCode { get; set; }
        public string ProdLineName { get; set; }
        public string WCCode { get; set; }
        public string CreateUserAccount { get; set; }
        public string CreateMachine { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        public virtual MFG_WorkCenter MFG_WorkCenter { get; set; }
    }
}