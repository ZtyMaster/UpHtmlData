using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ivony.Html.Parser;
using Ivony.Html;
using System.Reflection;
using mshtml;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Data;
using System.Net;
using System.IO;
using System.Data.Entity;
using AutoUpData.DAL;

namespace AutoUpData
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();            
        }

        List<newWORD.Class1> L_Class_Ganji = new List<newWORD.Class1>();
        List<newWORD.Class1> L_Class = new List<newWORD.Class1>();          
        public int ZantingI { get; set; }
        public DateTime MaxTime { get; set; }

        public int r { get; set; }
        string html = string.Empty;
        System.Windows.Forms.WebBrowser browser = new System.Windows.Forms.WebBrowser();
        public List<string> liImg = new List<string>();
        public int i { get; set; }

        #region 老版本自动更新
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using (var ctx = new oaEntities())
            {
                var T = ctx.T_FGJHtmlData.DefaultIfEmpty().ToList();
                for (int i = 0; i < T.Count(); i++)
                {
                    T_FGJHtmlData tf = T[i];
                    tf.SumMoneyID = GetMoney(tf.FwSumMoney);
                    tf.HuXingID = GetHuxing(tf.FwHuXing);
                    tf.MianjiID = GeiMinji(tf.FwMianji);                   
                    tf.Money_int = GetMoney_int(tf.FwSumMoney);
                    tf.Pingmi_int = GetPingmi_int(tf.FwMianji);          

                }
                ctx.SaveChanges();
            }
        }

        private decimal? GetPingmi_int(string pm)
        {
            pm = pm.IndexOf("㎡") > 0 ? pm.Substring(0, pm.IndexOf("㎡")) : "-1";
            return Convert.ToDecimal(pm.Trim());
        }

        private decimal? GetMoney_int(string mm)
        {   string m=mm.Trim();
            m = m.IndexOf("价") > 0 ? m.Substring(m.IndexOf("价") + 1) : m.IndexOf(" (") > 0 ? m.Substring(m.IndexOf(" (") + 2) : "-1";
            m = m == "-1" ? m : m.Substring(0, m.IndexOf("元"));
           return  Convert.ToDecimal(m.Trim());
        }



        #endregion
        CookieContainer ccr = new CookieContainer();
        CookieContainer ccrgj = new CookieContainer();
        public bool WhileBOOL1 { get; set; }
        public bool WhileBOOL2 { get; set; }

        public bool WhileBOOL3 { get; set; }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (UPURL58.Text.Trim().Length<=0|| UPURLganji.Text.Trim().Length <= 0)
            {

                MessageBox.Show("必须添加更新地址才可更新！");
                return;
            }
            Thread htmlHelper = new Thread(GoUpN);
            htmlHelper.SetApartmentState(ApartmentState.STA);
            htmlHelper.IsBackground = true;
            htmlHelper.Start();
            Thread htmlHelper1 = new Thread(GoUpN1);
            htmlHelper1.SetApartmentState(ApartmentState.STA);
            htmlHelper1.IsBackground = true;
            htmlHelper1.Start();
        }
        #region 新版本58更新系统
        private void GoUpN()
        {
            while (true)
            {
                L_Class.Clear();
                Thread htmlHelper = new Thread(NewHTMLhreper);
                htmlHelper.SetApartmentState(ApartmentState.STA);
                htmlHelper.IsBackground = true;
                htmlHelper.Start();
                WhileBOOL1 = true;
                // 使用多现场进行更新  
                while (WhileBOOL1)
                {
                    System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                }
                htmlHelper.Abort();
                Thread.Sleep(180000);
            }
        }
        private void NewHTMLhreper()
        {
            string url="";
             this.Dispatcher.Invoke(new Action(() =>
            {
                 url = UPURL58.Text;
            }));
            int cityItems = 0;
            this.Dispatcher.Invoke(new Action(() =>
            {
                cityItems = Convert.ToInt32(tongcheCombx.SelectedValue);
            }));
            try
            {
                string ALLhtml = GetHTMLstr(url);
                using (var ctx = new oaEntities())
                {
                    DateTime DBtime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
                    DateTime NOtime = DateTime.Now;
                    var drtme = ctx.T_FGJHtmlData.DefaultIfEmpty().Where(x => x.FbTime != DBtime && x.Laiyuan == "58" && x.CityID == cityItems && x.FbTime <= NOtime).FirstOrDefault();
                    MaxTime = drtme==null?DBtime.AddDays(-1): ctx.T_FGJHtmlData.DefaultIfEmpty().Where(x => x.FbTime != DBtime && x.Laiyuan == "58" && x.CityID == cityItems && x.FbTime <= NOtime).Max(x => x.FbTime);
                    #region 开始读取网站列表信息
                    IHtmlDocument document = new JumonyParser().Parse(ALLhtml);
                    IEnumerable<IHtmlElement> result = document.Find(".house-list-wrap");
                    IEnumerable<IHtmlElement> t = result.Find("li");
                    Dictionary<string, string> dir = new Dictionary<string, string>();
                    int ret = 0;
                  
                    foreach (var item in t)
                    {
                        #region MyRegion
                        newWORD.Class1 _class = new newWORD.Class1();

                        _class.TextName = MainWindow.GetN_value(item, ".title>a");
                        IEnumerable<IHtmlElement> adds = item.Find(".baseinfo");
                        IEnumerable<IHtmlElement> addsa = adds.Find("a");
                        string adess = "";
                        foreach (var addsaa in addsa)
                        {
                            adess += addsaa.InnerText();
                        }
                        _class.Address = adess;

                        string Timestr = MainWindow.GetN_value(item, ".time");
                        DateTime Dte = DateTime.Now;
                        _class.FbTime = Convert.ToDateTime(Dte.Year.ToString() + "-" + Dte.Month.ToString() + "-" + Dte.Day.ToString());
                        Timestr = Timestr.Replace("更新于", string.Empty);
                        Timestr = Timestr.Replace("前", string.Empty);
                        if (Timestr != "今天")
                        {
                            #region MyRegion
                            if (Timestr.IndexOf("分钟") > -1)
                            {
                                ret = IFthis_I(ret);
                                _class.FbTime = Dte.AddMinutes(-(Convert.ToInt32(Timestr.Replace("分钟", string.Empty))));
                            }
                            else if (Timestr.IndexOf("小时") > -1)
                            {
                                ret = IFthis_I(ret);
                                _class.FbTime = Dte.AddHours(-(Convert.ToInt32(Timestr.Replace("小时", string.Empty))));
                            }
                            else if (Timestr.IndexOf("天") > -1)
                            {
                                if (Timestr.Trim() == "昨天")
                                { _class.FbTime = Dte.AddDays(-1); }
                                else
                                {
                                    if (Timestr.Replace("天", string.Empty).Trim().Length <= 0)
                                    { continue; }
                                    _class.FbTime = Dte.AddDays(-3);
                                }                                
                            }
                            else
                            {
                                string[] strTime = Timestr.Split('-');
                                _class.FbTime = Convert.ToDateTime((_class.FbTime.Year-1) + "-" + strTime[0] + "-" + strTime[1]);
                            }

                            if (_class.FbTime <= MaxTime)
                            {

                                if (ret >=2)
                                {
                                    break;
                                }
                                ret++;
                            }
                            else
                            {
                                ret =0;
                            }
                            #endregion
                        }
                        else
                        {

                            var MaxT = ctx.T_FGJHtmlData.Where(x => x.HLName == _class.TextName && x.Address == _class.Address).DefaultIfEmpty().ToList();
                            if (MaxT[0] != null)
                            {
                                    var tm = MaxT.Max(x => x.FbTime);
                                    var vtm = MaxT.Where(x => x.FbTime == tm).First();
                                    //判断这条数据数 的时间是否是今天更新的 如果是 那么不进行操作
                                    if (vtm.FbTime == DBtime)
                                    {
                                        continue;
                                    }
                                                               
                            }                            
                            
                        }
                        _class.href = item.Exists(".title > a") ? item.FindFirst(".title>a").Attribute("href").Value() : string.Empty;
                        _class.Quyu = "同城";
                        _class.PersonName = MainWindow.GetN_value(item, ".jjrname-outer");
                        _class.Laiyuan = "58";
                        _class.datetime = "";
                        _class.Image_Count = MainWindow.GetInt_value(item, ".picNum");
                        _class.Image_str = _class.Image_Count > 0 ? "有" : string.Empty;
                        
                        if (ret == 0)
                        {
                            L_Class.Add(_class); }
                        r++;
                        #endregion
                    }
                    #endregion
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        SUMcount.Text = L_Class.Count.ToString();

                    }));
                    #region 查询完所有网站后执行 读取详细信息
                    for (i = 0; i < L_Class.Count; i++)
                    {
                        Thread.Sleep(6000);
                        if (L_Class.Count == 0)
                        {
                            continue;
                        }
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            ReadCount.Text = i.ToString();

                        }));
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            Yanzheng.Text = L_Class[i].href;
                        }));
                        string html = GetHTMLstr(L_Class[i].href);
                        IHtmlDocument document_1 = new JumonyParser().Parse(html);

                        IEnumerable<IHtmlElement> rl = document_1.Find("div");
                        IEnumerable<IHtmlElement> ss = rl.Find("p").Where(x => x.Identity() == "smallPicDescShow");
                        foreach (var rs in ss)
                        {
                            L_Class[i].bak = rs.InnerText();
                        }
                        IEnumerable<IHtmlElement> ul_il = document_1.Find("ul").Where(p => p.Identity() == "leftImg");
                        IEnumerable<IHtmlElement> li = ul_il.Find("li");
                        foreach (var img in li)
                        {
                            //liImg.Add();
                            L_Class[i].Image_str = L_Class[i].Image_str.Length > 0 ? L_Class[i].Image_str + "---" + img.FindFirst("img").Attribute("src").Value() : img.FindFirst("img").Attribute("src").Value();
                        }
                        IEnumerable<IHtmlElement> Phon = document_1.Find("div").Where(d => d.Identity() == "houseChatEntry");
                        IEnumerable<IHtmlElement> Phon_p = Phon.Find(".phone-num");
                        string phone = "";
                        foreach (var p in Phon_p)
                        {
                            phone = p.InnerText();
                        }
                        //获取概况信息
                        IEnumerable<IHtmlElement> GKelement = rl.Where(x => x.Identity() == "generalSituation").Find(".c_000");
                        int ElementI = 0;
                        foreach (var str in GKelement)
                        {
                            #region MyRegion
                            if (GKelement.Count() > 6)
                            {
                                switch (ElementI)
                                {
                                    case 0:
                                        L_Class[i].FwSumMoney = str.InnerText();
                                        break;
                                    case 1:
                                        L_Class[i].FwHuXing = str.InnerText();
                                        break;
                                    case 2:
                                        L_Class[i].FwMianji = str.InnerText();
                                        break;
                                    case 3:
                                        L_Class[i].FwChaoxiang = str.InnerText();
                                        break;
                                    case 4:
                                        L_Class[i].Loucheng = str.InnerText();
                                        break;
                                    case 5:
                                        L_Class[i].FwZhuangxiu = str.InnerText();
                                        break;
                                    case 6:
                                        L_Class[i].FwNianxian = str.InnerText();
                                        break;
                                    default:
                                        ;
                                        break;
                                }
                            }
                            else if (GKelement.Count() == 4)
                            {
                                switch (ElementI)
                                {
                                    case 0:
                                        L_Class[i].FwSumMoney = str.InnerText();
                                        break;
                                    case 1:
                                        L_Class[i].FwHuXing = str.InnerText();
                                        break;
                                    case 2:
                                        L_Class[i].FwMianji = str.InnerText();
                                        break;
                                    case 3:
                                        if (str.InnerText().IndexOf("层") > 0)
                                        {
                                            L_Class[i].FwLoucheng = str.InnerText();
                                        }
                                        else
                                        { L_Class[i].FwZhuangxiu = str.InnerText(); }

                                        break;
                                    default:
                                        ;
                                        break;
                                }
                            }
                            else
                            {
                                switch (ElementI)
                                {
                                    case 0:
                                        L_Class[i].FwSumMoney = str.InnerText();
                                        break;
                                    case 1:
                                        L_Class[i].FwHuXing = str.InnerText();
                                        break;
                                    case 2:
                                        L_Class[i].FwMianji = str.InnerText();
                                        break;
                                    case 3:
                                        L_Class[i].FwLoucheng = str.InnerText();
                                        break;
                                    case 4:
                                        L_Class[i].FwZhuangxiu = str.InnerText();
                                        break;
                                    case 5:
                                        L_Class[i].FwNianxian = str.InnerText();
                                        break;
                                    default:
                                        ;
                                        break;
                                }
                            }

                            #endregion
                            ElementI++;
                        }
                        L_Class[i].photo = phone;
                        if (i != 0)
                        {
                            List<newWORD.Class1> thisC = new List<newWORD.Class1>();
                            thisC.Add(L_Class[i - 1]);                            
                        }


                    }

                    #endregion
                    #region 读取完毕 开始数据库信息写入

                  
                    SaveDataHTML(ctx, L_Class, cityItems);
                     //  ctx.SaveChanges();
                    #endregion

                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    LISTbox.Text = "完成更新————" + DateTime.Now.ToString();

                }));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    LISTbox.Text = DateTime.Now.ToString() + ex.ToString();

                }));

            }
            WhileBOOL1 = false;
        }
        #endregion

        #region 新版本 赶集更新系统
        private void GoUpN1()
        {
            while (true)
            {
                // 使用多现场进行更新
                Thread htmlHelper = new Thread(NewHTMLhreperGJ);
                htmlHelper.SetApartmentState(ApartmentState.STA);
                htmlHelper.IsBackground = true;
                htmlHelper.Start();
                WhileBOOL2 = true;
                while (WhileBOOL2)
                {
                    System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                }
                htmlHelper.Abort();
                Thread.Sleep(180000);
            }
        }
        private void NewHTMLhreperGJ()
        {
            string url = "";
            this.Dispatcher.Invoke(new Action(() =>
            {
                 url = UPURLganji.Text;
            }));
            int cityItems = 0;
            this.Dispatcher.Invoke(new Action(() =>
            {
                cityItems = Convert.ToInt32(ganjicombc.SelectedValue);
            }));
            try
            {
                using (var ctx = new oaEntities())
                {
                    #region 加载首页
                    DateTime DBtime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
                    DateTime ds = DateTime.Now;
                    var ftdef = ctx.T_FGJHtmlData.DefaultIfEmpty().Where(x => x.FbTime != DBtime && x.Laiyuan == "赶集" && x.CityID == cityItems && x.FbTime <= ds).FirstOrDefault();

                    if (ftdef != null)
                    {
                        MaxTime = ctx.T_FGJHtmlData.DefaultIfEmpty().Where(x => x.FbTime != DBtime && x.Laiyuan == "赶集"&&x.CityID==cityItems&&x.FbTime<= ds).Max(x => x.FbTime);
                    }

                    string ALLhtml = GetHTMLstr_gj(url, "");
                    ALLhtml = ALLhtml.Replace("<!-- ", string.Empty);
                    ALLhtml = ALLhtml.Replace(" -->", string.Empty);
                    IHtmlDocument document = new JumonyParser().Parse(ALLhtml);
                    IEnumerable<IHtmlElement> result1 = document.Find(".list-items");
                    L_Class_Ganji.Clear();
                    foreach (var item in result1)
                    {
                        #region MyRegion
                        try
                        {
                            newWORD.Class1 GJclass = new newWORD.Class1();
                            IHtmlElement item_a = item.FindFirst("a");
                            //信息名称
                            GJclass.TextName = item.FindFirst(".house-name").InnerText().Trim();

                            #region 判断时间的方法 
                            string Timestr = item.FindFirst(".house-pulishtime").InnerText().Trim();
                            DateTime Dte = DateTime.Now;
                            GJclass.FbTime = Convert.ToDateTime(Dte.Year.ToString() + "-" + Dte.Month.ToString() + "-" + Dte.Day.ToString());
                            Timestr = Timestr.Replace("更新于", string.Empty);
                            Timestr = Timestr.Replace("前", string.Empty);
                           
                             if (Timestr != "今天")
                            {
                                if (Timestr.IndexOf("分钟") > -1)
                                {
                                    if (Timestr.Replace("分钟", string.Empty).Trim().Length <= 0)
                                    { continue; }
                                    GJclass.FbTime = Dte.AddMinutes(-(Convert.ToInt32(Timestr.Replace("分钟", string.Empty))));
                                }
                                else if (Timestr.IndexOf("小时") > -1)
                                {
                                    if (Timestr.Replace("小时", string.Empty).Trim().Length <= 0)
                                    { continue; }
                                    GJclass.FbTime = Dte.AddHours(-(Convert.ToInt32(Timestr.Replace("小时", string.Empty))));
                                }
                                else if (Timestr.IndexOf("天") > -1)
                                {
                                    if (Timestr.Trim() == "昨天")
                                    { GJclass.FbTime = Dte.AddDays(-1); }
                                    else if (Timestr.Trim() == "前天")
                                    {
                                        GJclass.FbTime = Dte.AddDays(-2);
                                    }
                                    else
                                    {
                                        if (Timestr.Replace("天", string.Empty).Trim().Length <= 0)
                                        { continue; }
                                        GJclass.FbTime = Dte.AddDays(-3);
                                    }
                                }
                                else if (Timestr.Trim() == "刚刚")
                                {
                                    GJclass.FbTime = DateTime.Now;
                                }
                                else
                                {
                                    string[] strTime_ = Timestr.Split('-');
                                    GJclass.FbTime = Convert.ToDateTime((GJclass.FbTime.Year - 1) + "-" + strTime_[0] + "-" + strTime_[1]);
                                }
                                if (GJclass.FbTime <= MaxTime)
                                {

                                    continue;
                                }
                            }
                            #endregion

                            //连接
                            GJclass.href = item_a.Attribute("href").Value().IndexOf("https") > -1 ? item_a.Attribute("href").Value() : "https://3g.ganji.com" + item_a.Attribute("href").Value();
                            //地址
                            GJclass.Address = item.FindFirst(".house-addr").FindFirst(".house-area").InnerText();
                            L_Class_Ganji.Add(GJclass);
                        }
                        catch (Exception ex)
                        {

                        }
                        #endregion
                        
                    }
                    #endregion
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        GJSUMcount.Text = L_Class_Ganji.Count().ToString();
                    }));
                    #region 加载分页
                    for (int i = 0; i < L_Class_Ganji.Count; i++)
                    {
                        Thread.Sleep(3000);
                        try {

                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                GJReadCount.Text = i.ToString();
                            }));
                            string this_html = GetHTMLstr_gj(L_Class_Ganji[i].href, "");
                            if (this_html.Trim().Length <= 0)
                            {
                                continue;
                            }
                            IHtmlDocument document_this = new JumonyParser().Parse(this_html);
                            //金额
                            L_Class_Ganji[i].FwSumMoney = document_this.Exists(".house-price") ? document_this.FindFirst(".house-price").InnerText().Replace("算房贷", string.Empty).Trim() : string.Empty;
                            var this_Image = document_this.Find(".slide-area>li>img");

                            L_Class_Ganji[i].Image_str = string.Empty;
                            foreach (var img in this_Image)
                            {
                                L_Class_Ganji[i].Image_str = L_Class_Ganji[i].Image_str.Length > 0 ? L_Class_Ganji[i].Image_str + "---" + img.Attribute("data-big-image").Value() : "有---" + img.Attribute("data-big-image").Value();

                            }
                            var list = document_this.Find(".house-type>span");
                            //电话
                            var php = document_this.Find(".tel");
                            foreach (var item in php)
                            {
                                if (item.Attribute("href").Value().Trim().Length > 0)
                                {
                                    L_Class_Ganji[i].photo = item.Attribute("href").Value().Trim().Replace("tel:", string.Empty);
                                }
                            }
                            //联系人


                            L_Class_Ganji[i].Laiyuan = "赶集";

                            L_Class_Ganji[i].PersonName = document_this.Exists(".broker") ? document_this.FindFirst(".broker").FindFirst("span").InnerText().Replace("(个人)", string.Empty) : string.Empty;



                            foreach (var item in list)
                            {
                                if (item.InnerText().IndexOf("朝") > -1)
                                {
                                    L_Class_Ganji[i].FwChaoxiang = PFUANDtext(item, "朝");
                                }
                                else if (item.InnerText().IndexOf("室") > -1)
                                { L_Class_Ganji[i].FwHuXing = PFUANDtext(item, "室"); }
                                else if (item.InnerText().IndexOf("层") > -1)
                                { L_Class_Ganji[i].FwLoucheng = PFUANDtext(item, "层"); }
                                else if (item.InnerText().IndexOf("㎡") > -1)
                                { L_Class_Ganji[i].FwMianji = PFUANDtext(item, "㎡"); }
                                else if (item.InnerText().IndexOf("产权") > -1)
                                { L_Class_Ganji[i].FwNianxian = PFUANDtext(item, "产权"); }
                                else if (item.InnerText().IndexOf("毛") > -1)
                                { L_Class_Ganji[i].FwZhuangxiu = PFUANDtext(item, "毛"); }
                                else if (item.InnerText().IndexOf("装修") > -1)
                                { L_Class_Ganji[i].FwZhuangxiu = PFUANDtext(item, "装修"); }
                            }
                        }
                        catch (Exception e) {

                        }
                        
                    }
                    #endregion
                    #region 进行数据存储
                  
                    SaveDataHTML(ctx, L_Class_Ganji,cityItems);

                    ctx.SaveChanges();
                    #endregion
                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    GJLISTbox.Text = "完成更新————" + DateTime.Now.ToString();
                }));

            }
            catch (Exception e)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Ertext.Text = DateTime.Now.ToString() + e.ToString();

                }));
                WhileBOOL2 = false;
            }
            WhileBOOL2 = false;

        }
        #endregion
        private static int IFthis_I(int ret)
        {
            ret = ret > 0 ? ret-- : ret;
            return ret;
        }
        private string GetHTMLstr(string url)
        {
            string ALLhtml;
            HttpHeader header = new HttpHeader();
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "GET";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;

            ccr = ccr.Count < 0 ? HTMLHelper.GetCooKie(url, "PGTID=0d30000c-007f-6cea-00fc-a261cfc9171c&ClickID=1", header) : ccr;
            ALLhtml = HTMLHelper.GetHtml(url, ref ccr, header);
            return ALLhtml;
        }
        private string GetHTMLstr_gj(string url, string str)
        {
            string ALLhtml;
            HttpHeader header = new HttpHeader();
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "GET";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;

            ccrgj = ccrgj.Count < 0 ? HTMLHelper.GetCooKie(url, str, header) : ccrgj;
            ALLhtml = HTMLHelper.GetHtml(url, ref ccrgj, header);
            return ALLhtml;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            while (true)
            {
                // 使用多现场进行更新
                Thread htmlHelper = new Thread(NewHTMLhreperGJ);
                htmlHelper.SetApartmentState(ApartmentState.STA);
                htmlHelper.IsBackground = true;
                htmlHelper.Start();
                WhileBOOL2 = true;
                while (WhileBOOL2)
                {
                    System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                }
                htmlHelper.Abort();
                Thread.Sleep(180000);
            }

        }

        private void SaveDataHTML(oaEntities ctx, List<newWORD.Class1> L_Class_,int p)
        {            
            DAL.T_FGJHtmlDataDal Dti = new DAL.T_FGJHtmlDataDal();
            foreach (var da in L_Class_)
            {
                T_FGJHtmlData tf = new T_FGJHtmlData();

                tf.HLName = da.TextName;
                tf.HLhref = da.href;
                tf.PersonName = da.PersonName;
                tf.Address = da.Address;
                tf.photo = da.photo;
                tf.FbTime = da.FbTime;
                tf.FbTime = tf.FbTime > DateTime.Now ? new DateTime(2017,tf.FbTime.Month,tf.FbTime.Day,tf.FbTime.Hour,tf.FbTime.Minute,tf.FbTime.Second):tf.FbTime;
                if (da.FwSumMoney == null)
                {
                    continue;
                }
                else
                {
                    if (da.FwSumMoney.Trim().Length <= 0)
                    {
                        continue;
                    }
                }
                tf.FwSumMoney = da.FwSumMoney;
                tf.FwHuXing = da.FwHuXing;
                tf.FwMianji = da.FwMianji;
                tf.FwLoucheng = da.FwLoucheng;
                tf.FwZhuangxiu = da.FwZhuangxiu;
                tf.FwNianxian = da.FwNianxian;
                tf.FwChaoxiang = da.FwChaoxiang;
                tf.bak = da.bak;
                tf.Id_count = int.Parse(da.Id_count == null ? "0" : da.Id_count);
                tf.Laiyuan = da.Laiyuan;
                tf.Image_str = da.Image_str;
                tf.CityID = p;
                if (da.FwMianji != null)
                {
                    tf.MianjiID = GeiMinji(da.FwMianji);
                    tf.Pingmi_int = GetPingmi_int(da.FwMianji);
                }
                if (da.FwSumMoney != null)
                {
                    tf.SumMoneyID = GetMoney(da.FwSumMoney);
                    tf.Money_int = GetMoney_int(da.FwSumMoney);
                }
                if (da.FwHuXing != null)
                {
                    tf.HuXingID = GetHuxing(da.FwHuXing);
                }

                //如果数据库中出现该名称并且 时间在当前时间那么  该信息不写入数据库
                //如果有该数据那么修改数据库中的数据信息


               
                var isnull = Dti.LoadEntities(x => x.FwMianji == tf.FwMianji && x.photo == tf.photo).DefaultIfEmpty();
                
                if (isnull.ToList()[0] != null)
                {
                    var tm = isnull.Max(x => x.FbTime);
                    var vtm = isnull.Where(x => x.FbTime == tm).First();
                    vtm.FbTime = tf.FbTime;
                    vtm.Money_int = tf.Money_int;
                    vtm.FwSumMoney = tf.FwSumMoney;
                    vtm.SumMoneyID = tf.SumMoneyID;
                    Dti.EditEntity(vtm);
                                          
                }
                else 
                {
                    Dti.AddEntity(tf);
                }
            }
            Dti.SaveChanges();
        }
        private static int GetHuxing(string da)
        {
            if (da.IndexOf("1室") >= 0)
            {
                return 1;
            }
            else if (da.IndexOf("2室") >= 0)
            { return 2; }
            else if (da.IndexOf("3室") >= 0)
            { return 3; }
            else if (da.IndexOf("4室") >= 0)
            { return 4; }
            else
            {
                return 5;
            }
        }
        private static int GetMoney(string da)
        {
            if (da.IndexOf("万") < 0)
            {
                return 0;
            }
            string str = da.Remove(da.IndexOf("万"));
            str = str.Replace("万", string.Empty);
            str = str.Replace("以下", string.Empty);
            str = str.Replace("以上", string.Empty);
            double mj = Convert.ToDouble(str);
            if (mj < 30)
            {
                return 1;
            }
            else if (mj >= 30 && mj < 40)
            {
                return 2;
            }
            else if (mj >= 40 && mj < 50)
            {
                return 3;
            }
            else if (mj >= 50 && mj < 60)
            {
                return 4;
            }
            else if (mj >= 60 && mj < 80)
            {
                return 5;
            }
            else if (mj >= 80 && mj < 100)
            {
                return 6;
            }
            else if (mj >= 100 && mj < 120)
            {
                return 7;
            }
            else if (mj >= 120 && mj < 160)
            {
                return 8;
            }
            else if (mj >= 160 && mj < 200)
            {
                return 9;
            }
            else
            {
                return 10;
            }
        }
        private static int GeiMinji(string da)
        {
            if (da.IndexOf("㎡") < 0)
            {
                return 0;
            }
            string str = da.Replace("㎡", string.Empty);
            str = str.Replace("以下", string.Empty);
            str = str.Replace("以上", string.Empty);

            double mj = Convert.ToDouble(str);
            if (mj < 50)
            {
                return 1;
            }
            else if (mj >= 50 && mj < 70)
            {
                return 2;
            }
            else if (mj >= 70 && mj < 90)
            {
                return 3;
            }
            else if (mj >= 90 && mj < 110)
            {
                return 4;
            }
            else if (mj >= 110 && mj < 130)
            {
                return 5;
            }
            else if (mj >= 130 && mj < 150)
            {
                return 6;
            }
            else if (mj >= 150 && mj < 200)
            {
                return 7;
            }
            else if (mj >= 200 && mj < 300)
            {
                return 8;
            }
            else if (mj >= 300 && mj < 500)
            {
                return 9;
            }
            else
            {
                return 10;
            }
        }

        private string PFUANDtext(IHtmlElement item, string str)
        {
            return item.InnerText().IndexOf(str) > -1 ? item.InnerText() : string.Empty;
        }
        public static string GetN_value(IHtmlElement item, string str)
        {
            return item.Exists(str) ? item.FindFirst(str).InnerText().Trim() : string.Empty;
        }
        public static int GetInt_value(IHtmlElement item, string str)
        {
            return item.Exists(str) ? Convert.ToInt32(item.FindFirst(str).InnerText().Trim().Replace("图", "")) : 0;
        }

        #region WPF 设置WebBrowser控件不弹脚本错误提示框
        void WebBrowserMain_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SetWebBrowserSilent(sender as System.Windows.Controls.WebBrowser, true);
        }
        /// <summary>  
        /// 设置浏览器静默，不弹错误提示框  
        /// </summary>  
        /// <param name="webBrowser">要设置的WebBrowser控件浏览器</param>  
        /// <param name="silent">是否静默</param>  
        private void SetWebBrowserSilent(System.Windows.Controls.WebBrowser webBrowser, bool silent)
        {
            FieldInfo fi = typeof(System.Windows.Controls.WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                object browser = fi.GetValue(webBrowser);
                if (browser != null)
                    browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
            }
        }
        #endregion
        System.Timers.Timer aTimer = new System.Timers.Timer(1000 * 60);
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            using (var City = new oaEntities())
            {
                var prot = City.T_province.Where(x => x.ID > 0);               
                tongchePro.ItemsSource = prot.ToList();
                tongchePro.SelectedValuePath = "ID";
                tongchePro.DisplayMemberPath = "str";

                procombc.ItemsSource = prot.ToList();
                procombc.SelectedValuePath = "ID";
                procombc.DisplayMemberPath = "str";

                var T_BoolItemService = City.T_BoolItem;
                var tbb = T_BoolItemService.Where(x => x.ID == 2).FirstOrDefault();
                TiaDay.Text = tbb.@int.ToString();


            }
        }
        void OnTimedEvent()
        {
            using (var ctx = new oaEntities())
            {

                //——————————————————————更新 时间大于

                var ftimes = DateTime.Now;
                var catemp = ctx.T_FGJHtmlData.Where(x => x.FbTime > ftimes).DefaultIfEmpty();
                if (catemp != null)
                {
                    if (catemp.Count() > 0)
                    {
                        foreach (var tp in catemp)
                        {
                            long id = tp.ID;
                            using (var Uptimes = new oaEntities())
                            {
                                var tps = Uptimes.T_FGJHtmlData.FirstOrDefault(z => z.ID == id);
                                tps.delflag = 1;
                                Uptimes.SaveChanges();
                            }
                        }
                    }
                }
                //————————————————————————————————————
                int Gint = 0;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Gint = Convert.ToInt32(TiaDay.Text);

                }));
                var Zuijia = ctx.T_ZhuaiJiaBak.Where(x => x.DEL == 0).ToList();
                List<long> lit = new List<long>();
                foreach (var a in Zuijia) {
                    lit.Add(Convert.ToInt64( a.FGJHTML_id));
                }
                var T_saverHtml = ctx.T_SaveHtmlData.Where(x => x.DelFlag == 0 && x.GongGong == 0&&!lit.Contains(x.HtmldataID)).ToList(); 
                for (int i = 0; i < T_saverHtml.Count(); i++)
                {
                    if (T_saverHtml[i].SaveTime.AddDays(Gint) < DateTime.Now)
                    {
                        long id = T_saverHtml[i].ID;
                        using (var newoa = new oaEntities())
                        {
                            var temp = newoa.T_SaveHtmlData.FirstOrDefault(x => x.ID == id);
                            temp.GongGong = 1;
                          //  newoa.Entry<T_SaveHtmlData>(temp).State = System.Data.EntityState.Modified;
                            newoa.SaveChanges();
                        }
                    }
                }               
                
            }
        }
   
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            UPdemo.Text = "数据库公共信息更新开启";
            while (true)
            {
                Thread htmlHelper1 = new Thread(OnTimedEvent);
                htmlHelper1.SetApartmentState(ApartmentState.STA);
                htmlHelper1.IsBackground = true;
                htmlHelper1.Start();
                // 使用多现场进行更新  
                WhileBOOL3 = true;
                // 使用多现场进行更新  
                while (WhileBOOL3)
                {
                    System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                }
                htmlHelper1.Abort();
                Thread.Sleep(180000);
            }

          
        }

        private void tongchePro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int strid = Convert.ToInt32( ((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
            using (var City = new oaEntities())
            {
                var Mcity = City.T_City.Where(x => x.DelFlag == 0&&x.province_id==strid);
                tongcheCombx.ItemsSource = Mcity.ToList();
                tongcheCombx.SelectedValuePath = "ID";
                tongcheCombx.DisplayMemberPath = "City";
                tongcheCombx.SelectedValue = 1;
                int id = Mcity.ToList()[0].ID;
                var href= City.T_UpHerfCity.Where(x => x.Items == "58更新"&&x.City_ID== id).FirstOrDefault();
                UPURL58.Text = href.Href;
            }

        }

        private void tongcheCombx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
                using (var City = new oaEntities())
                {
                    var href = City.T_UpHerfCity.Where(x => x.Items == "58更新" && x.City_ID == strid).FirstOrDefault();
                    UPURL58.Text = href == null ? "" : href.Href;
                }
            }
            catch (Exception err)
            {
               string ss= err.ToString();
            }
          
        }

        private void procombc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
            using (var City = new oaEntities())
            {
                var Mcity = City.T_City.Where(x => x.DelFlag == 0 && x.province_id == strid);
                ganjicombc.ItemsSource = Mcity.ToList();
                ganjicombc.SelectedValuePath = "ID";
                ganjicombc.DisplayMemberPath = "City";
                ganjicombc.SelectedValue = 1;
                int id = Mcity.ToList()[0].ID;
                var href = City.T_UpHerfCity.Where(x => x.Items == "赶集更新" && x.City_ID == id).FirstOrDefault();
                UPURLganji.Text = href.Href;
            }
        }

        private void ganjicombc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
                using (var City = new oaEntities())
                {
                    var href = City.T_UpHerfCity.Where(x => x.Items == "赶集更新" && x.City_ID == strid).FirstOrDefault();
                    UPURLganji.Text = href == null ? "" : href.Href;
                }
            }
            catch (Exception err)
            {
                string ss = err.ToString();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            DAL.T_FGJHtmlDataDal Dti = new DAL.T_FGJHtmlDataDal();

            var Alltfg = Dti.LoadEntities(x => x.delflag != 1 || x.delflag == null).DefaultIfEmpty();
            var c = Alltfg.Count();         
            var t = Alltfg.GroupBy(x => new { x.photo, x.FwMianji }).Where(g => g.Count() > 1);
            var tc = t.Count();
           
            var ttc = t.Count();
            int i = 0;
            foreach (var a in t)
            {
                
                var temp1 = Dti.LoadEntities(x => x.photo == a.Key.photo).FirstOrDefault();
                if (temp1 != null)
                {
                    var del1 = Dti.LoadEntities(x => x.photo == a.Key.photo).DefaultIfEmpty();
                    DateTime dt = del1.Max(x => x.FbTime);                   
                    foreach (var d in del1)
                    {
                        if (d.FbTime != dt)
                        {
                            d.delflag = 1;
                            Dti.EditEntity(d);
                        }
                    }
                }
                i++;
            }
            Dti.SaveChanges();
            
        }
    }
}
