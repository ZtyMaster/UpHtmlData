using Ivony.Html;
using Ivony.Html.Parser;
using mshtml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AutoUpData
{
    /// <summary>
    /// Up1.xaml 的交互逻辑
    /// </summary>
    /// 
    public delegate void ChangeTextHandler(string text);
    public partial class Up1 : Window
    {
        public event ChangeTextHandler ChangeTextEvent;
        public Up1()
        {
            InitializeComponent();
            
            web1.Navigating += WebBrowserMain_Navigating;
        }
        bool loadingS = true;    // 该变量表示网页是否正在加载.
        bool loading = true;    // 该变量表示网页是否正在加载.  
        public DateTime MaxTime { get; set; }
        IQueryable<T_FGJHtmlData> iqdata = null;
        List<newWORD.Class1> L_Class = new List<newWORD.Class1>();
       
        public int ZantingI { get; set; }
        public int r { get; set; }
        public int i { get; set; }
        public  void Updata()
        {
            #region MyRegion
            using (var ctx = new oaEntities())
            {
                var obj = (ctx as IObjectContextAdapter).ObjectContext;
                DateTime DBtime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
                MaxTime = ctx.T_FGJHtmlData.DefaultIfEmpty().Where(x => x.FbTime != DBtime).Max(x => x.FbTime);
              
                // var tfdata = ctx.T_FGJHtmlData.DefaultIfEmpty<T_FGJHtmlData>().Where(x=>x.FbTime==DBtime);

                iqdata = ctx.T_FGJHtmlData.DefaultIfEmpty<T_FGJHtmlData>().Where(x => x.FbTime == DBtime);

                web1.LoadCompleted += new LoadCompletedEventHandler(webbrowserUpload);
                loadingS = true;   // 表示正在加载 
                web1.Navigate("http://liaoyang.58.com/ershoufang/0");


                while (loadingS)
                {
                    System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                }
                
                #region 查询完所有网站后执行
                for (i = 0; i < L_Class.Count; i++)
                {
                    web1.LoadCompleted += new LoadCompletedEventHandler(web2_Navigated); ;

                    loading = true;   // 表示正在加载 
                    web1.Navigate(L_Class[i].href);
                    ChangeTextEvent("支目录条数已读取" + i + "条" + "-----主目录条数" + L_Class.Count());
                    while (loading)
                    {
                        System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                    }

                }
                //DPdg.ItemsSource = L_Class;
                #endregion

                foreach (var da in L_Class)
                {
                    T_FGJHtmlData tf = new T_FGJHtmlData();
                    //如果数据库中出现该名称并且 时间在当前时间那么  该信息不写入数据库

                    if (ctx.T_FGJHtmlData.FirstOrDefault(x => x.HLName == da.TextName) != null)
                    {
                        continue;
                    }
                    tf.HLName = da.TextName;
                    tf.HLhref = da.href;
                    tf.PersonName = da.PersonName;
                    tf.Address = da.Address;
                    tf.photo = da.photo;
                    tf.FbTime = da.FbTime;
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

                    ctx.T_FGJHtmlData.Add(tf);
                }
                ctx.SaveChanges();
                ChangeTextEvent("完成更新————" + DateTime.Now.ToString() + ".OK");


            }
            #endregion

            this.Close();
        }

        #region 主菜单LOAD 方法
        void webbrowserUpload(object sender, NavigationEventArgs e)
        {

            mshtml.HTMLDocument mhtml = (mshtml.HTMLDocument)web1.Document;
            string html = mhtml.body.innerHTML;
            IHtmlDocument document = new JumonyParser().Parse(html);

            IEnumerable<IHtmlElement> result = document.Find(".house-list-wrap");
            IEnumerable<IHtmlElement> t = result.Find("li");
            Dictionary<string, string> dir = new Dictionary<string, string>();
            bool ToNotDown = true;
            int ret = 0;
            foreach (var item in t)
            {
                #region MyRegion
                newWORD.Class1 _class = new newWORD.Class1();

                _class.TextName = MainWindow.GetN_value(item, ".title>a");
                _class.href = item.Exists(".title > a") ? item.FindFirst(".title>a").Attribute("href").Value() : string.Empty;
                _class.Quyu = "同城";
                _class.PersonName = MainWindow.GetN_value(item, ".jjrname-outer");
                _class.Laiyuan = "58";
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
                if (Timestr != "今天")
                {
                    if (Timestr.IndexOf("分钟") > -1)
                    {
                        _class.FbTime = Dte.AddMinutes(-(Convert.ToInt32(Timestr.Replace("分钟", string.Empty))));
                    }
                    else if (Timestr.IndexOf("小时") > -1)
                    {
                        _class.FbTime = Dte.AddHours(-(Convert.ToInt32(Timestr.Replace("小时", string.Empty))));
                    }
                    else
                    {
                        string[] strTime = Timestr.Split('-');
                        _class.FbTime = Convert.ToDateTime(_class.FbTime.Year + "-" + strTime[0] + "-" + strTime[1]);
                    }

                    if (_class.FbTime <= MaxTime)
                    {

                        if (ret >= 2)
                        {
                            ToNotDown = false;
                            break;
                        }
                        ret++;
                    }
                }
                else
                {
                    var datalist = iqdata.ToList();
                    if (iqdata.FirstOrDefault(x => x.HLName == _class.TextName && x.Address == _class.Address) != null)
                    {
                        continue;
                    }

                }


                _class.datetime = "";
                _class.Image_Count = MainWindow.GetInt_value(item, ".picNum");
                _class.Image_str = _class.Image_Count > 0 ? "有" : string.Empty;
                if (ret == 0)
                { L_Class.Add(_class); }
                r++;
                #endregion
            }
            int nextI = 0;
            #region 跳转倒下一页
            if (ToNotDown)
            {

                //mshtml.IHTMLDocument2 doc2 = (mshtml.IHTMLDocument2)web1.Document;
                //foreach (IHTMLElement ele in doc2.all)
                //{
                //    if (ele.tagName.ToLower().Equals("a"))
                //    {
                //        IHTMLElement aa = (IHTMLElement)ele;
                //        if (ZantingI >= 1)
                //        {
                //            break;
                //        }
                //        if (aa.innerText == "下一页")
                //        {
                //            ZantingI++;
                //            nextI++;
                //            aa.click();
                //            retStr = "读取主目录" + ZantingI;
                //        }

                //    }
                //}

            }
            #endregion
            loadingS = nextI > 0 ? true : false;
           
        }
        #endregion
        #region 子菜单LOAD方法
        private void web2_Navigated(object sender, NavigationEventArgs e)
        {
            mshtml.HTMLDocument mhtml = (mshtml.HTMLDocument)web1.Document;
            string html = mhtml.body.innerHTML;
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

            loading = false; // 在加载完成后,将该变量置为false,下一次循环随即开始执行. 
        }
        #endregion

        private void web1_Loaded(object sender, RoutedEventArgs e)
        {
            
           
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

        
    }
}
