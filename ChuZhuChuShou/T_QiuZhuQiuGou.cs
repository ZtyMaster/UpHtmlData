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
    
    public partial class T_QiuZhuQiuGou
    {
        public T_QiuZhuQiuGou()
        {
            this.SeeQzCzs = new HashSet<SeeQzCz>();
        }
    
        public long ID { get; set; }
        public string Hname { get; set; }
        public string QuYu { get; set; }
        public string JuShi { get; set; }
        public string Money { get; set; }
        public string RuZhu { get; set; }
        public System.DateTime FBtime { get; set; }
        public string Hperson { get; set; }
        public string Photo { get; set; }
        public string GuiShuDi { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<short> QiuZhuQiuGou { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<System.DateTime> AddUserTime { get; set; }
    
        public virtual T_City T_City { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<SeeQzCz> SeeQzCzs { get; set; }
    }
}
