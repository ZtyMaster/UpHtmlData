//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChuZhuChuShou
{
    using System;
    using System.Collections.Generic;
    
    public partial class TLoginbak
    {
        public long ID { get; set; }
        public int LGUserID { get; set; }
        public Nullable<System.DateTime> intime { get; set; }
        public Nullable<System.DateTime> outtime { get; set; }
        public Nullable<short> del { get; set; }
        public string LGip { get; set; }
        public string LGbak { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
    }
}
