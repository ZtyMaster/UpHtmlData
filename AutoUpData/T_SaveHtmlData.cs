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
    
    public partial class T_SaveHtmlData
    {
        public long ID { get; set; }
        public long HtmldataID { get; set; }
        public int UserID { get; set; }
        public short DelFlag { get; set; }
        public System.DateTime SaveTime { get; set; }
        public Nullable<int> BiaoJiId { get; set; }
        public Nullable<System.DateTime> BiaoJiTime { get; set; }
        public Nullable<short> GongGong { get; set; }
        public Nullable<int> MasterID { get; set; }
    
        public virtual T_BiaoJiInfo T_BiaoJiInfo { get; set; }
        public virtual T_FGJHtmlData T_FGJHtmlData { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual UserInfo UserInfo1 { get; set; }
        public virtual UserInfo UserInfo11 { get; set; }
    }
}
