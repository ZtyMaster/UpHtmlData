//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoUpData
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_UpHerfCity
    {
        public long ID { get; set; }
        public Nullable<int> City_ID { get; set; }
        public string Items { get; set; }
        public string Href { get; set; }
        public Nullable<int> AddUser { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
        public Nullable<short> Del { get; set; }
        public string textbak { get; set; }
    
        public virtual T_City T_City { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
