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
    
    public partial class MFG_ScanPluginParam
    {
        public int ID { get; set; }
        public string PluginID { get; set; }
        public string ParamName { get; set; }
        public string ParamDesc { get; set; }
        public bool IsNecessary { get; set; }
        public string CreateUserAccount { get; set; }
        public string CreateMachine { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        public virtual MFG_ScanPlugin MFG_ScanPlugin { get; set; }
    }
}
