using Ivony.Html;
using Ivony.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace newWORD
{

    public class Class1
    {
        public string ID { get; set; }
        public string TextName { get; set; }
        public string PersonName { get; set; }
        public int Image_Count { get; set; }
        public string Image_str { get; set; }
        public string Url { get; set; }
        public string Laiyuan { get; set; }
        public string FwSumMoney { get; set; }
        public string FwHuXing { get; set; }
        public string FwMianji { get; set; }
        public string FwLoucheng { get; set; }
        public string FwZhuangxiu { get; set; }
        public string FwNianxian { get; set; }
        public string FwChaoxiang { get; set; }
        public string SumMoney { get; set; }
        public string PingMoney { get; set; }
        public string datetime { get; set; }
        public string Allpm { get; set; }
        public string Address { get; set; }
        public string Quyu { get; set; }
        public string Huxing { get; set; }
        public string Mianji { get; set; }
        public string Loucheng { get; set; }
        public string Chaoxiang { get; set; }

        public string href { get; set; }
        public string photo { get; set; }
        public List<string> lst_ { get; set; }
        public string bak { get; set; }
        public DateTime time { get; set; }
        public string Id_count { get; set; }
        private string Id { get; set; }
       


        public DateTime FbTime { get; set; }
        public void NewMethod(string html, ref bool fristLoads, ref List<Class1> L_Class, ref int r)
        {
            IHtmlDocument document = new JumonyParser().Parse(html);
            //网站第一次加载后读取区域地址
            if (fristLoads)
            {
                // GetSelectQuYu(document);
                //  fristLoads = false;
            }
            IEnumerable<IHtmlElement> result = document.Find(".house-list-wrap");
            IEnumerable<IHtmlElement> t = result.Find("li");
            Dictionary<string, string> dir = new Dictionary<string, string>();
            foreach (var item in t)
            {
                #region MyRegion
                Class1 _class = new Class1();

                _class.TextName = GetN_value(item, ".title>a");
                _class.href = item.Exists(".title > a") ? item.FindFirst(".title>a").Attribute("href").Value() : string.Empty;
                _class.Quyu = "同城";
                _class.PersonName = GetN_value(item, ".jjrname-outer");
                _class.Laiyuan = "58";
                IEnumerable<IHtmlElement> adds = item.Find(".baseinfo");
                IEnumerable<IHtmlElement> addsa = adds.Find("a");
                string adess = "";
                foreach (var addsaa in addsa)
                {
                    adess += addsaa.InnerText();
                }
                _class.Address = adess;
                adds.Find("a");
                string[] ssp = GetN_value(item, ".qj-listright").Split(' ');
                int j = ssp.Length == 10 ? 0 : 10 - ssp.Length;
                _class.SumMoney = GetN_value(item, ".sum");
                _class.PingMoney = GetN_value(item, ".unit");
                _class.Allpm = GetN_value(item, ".baseinfo");

                _class.datetime = GetN_value(item, ".time");
                _class.Image_Count = GetInt_value(item, ".picNum");
                _class.Image_str = _class.Image_Count > 0 ? "有" : string.Empty;
                L_Class.Add(_class);
                r++;
                #endregion
            }
        }

        public static string GetN_value(IHtmlElement item, string str)
        {
            return item.Exists(str) ? item.FindFirst(str).InnerText().Trim() : string.Empty;
        }
        public static int GetInt_value(IHtmlElement item, string str)
        {
            return item.Exists(str) ? Convert.ToInt32(item.FindFirst(str).InnerText().Trim().Replace("图", "")) : 0;
        }
    }
    public class URL_list {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
    }
    public class Quyu {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
    }

   



}
