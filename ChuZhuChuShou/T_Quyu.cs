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
    
    public partial class T_Quyu
    {
        public int ID { get; set; }
        public int T_CityID { get; set; }
        public string QY_connet { get; set; }
        public Nullable<bool> DelFlag { get; set; }
    
        public virtual T_City T_City { get; set; }
    }
}