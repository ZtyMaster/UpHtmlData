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
    
    public partial class T_ChuZhuInfo
    {
        public T_ChuZhuInfo()
        {
            this.SeeQzCzs = new HashSet<SeeQzCz>();
        }
    
        public long ID { get; set; }
        public string ChuZhuName { get; set; }
        public string ChuZhuHref { get; set; }
        public string LianXiPerson { get; set; }
        public string LianXiPhoto { get; set; }
        public string Addess { get; set; }
        public Nullable<System.DateTime> FbTime { get; set; }
        public string XiaoQu { get; set; }
        public Nullable<decimal> Money { get; set; }
        public Nullable<decimal> PingMi { get; set; }
        public string Tingshi { get; set; }
        public string Images { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<short> Del { get; set; }
        public Nullable<int> PingMi_int { get; set; }
        public Nullable<int> Money_int { get; set; }
        public string LaiYuan { get; set; }
        public string Bak { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> AdduserTime { get; set; }
    
        public virtual T_City T_City { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<SeeQzCz> SeeQzCzs { get; set; }
    }
}
