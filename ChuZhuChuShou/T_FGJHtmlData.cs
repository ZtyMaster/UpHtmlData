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
    
    public partial class T_FGJHtmlData
    {
        public T_FGJHtmlData()
        {
            this.T_SeeClickPhoto = new HashSet<T_SeeClickPhoto>();
            this.T_ZhuaiJiaBak = new HashSet<T_ZhuaiJiaBak>();
        }
    
        public long ID { get; set; }
        public string HLName { get; set; }
        public string HLhref { get; set; }
        public string PersonName { get; set; }
        public string Address { get; set; }
        public string photo { get; set; }
        public System.DateTime FbTime { get; set; }
        public string FwSumMoney { get; set; }
        public string FwHuXing { get; set; }
        public string FwMianji { get; set; }
        public string FwLoucheng { get; set; }
        public string FwZhuangxiu { get; set; }
        public string FwNianxian { get; set; }
        public string FwChaoxiang { get; set; }
        public string bak { get; set; }
        public Nullable<int> Id_count { get; set; }
        public string Laiyuan { get; set; }
        public string Image_str { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<short> delflag { get; set; }
        public Nullable<int> SumMoneyID { get; set; }
        public Nullable<int> MianjiID { get; set; }
        public Nullable<int> HuXingID { get; set; }
        public Nullable<decimal> Money_int { get; set; }
        public Nullable<decimal> Pingmi_int { get; set; }
        public Nullable<int> AddItemsUserID { get; set; }
        public Nullable<System.DateTime> AddUserTiem { get; set; }
    
        public virtual T_City T_City { get; set; }
        public virtual ICollection<T_SeeClickPhoto> T_SeeClickPhoto { get; set; }
        public virtual ICollection<T_ZhuaiJiaBak> T_ZhuaiJiaBak { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
