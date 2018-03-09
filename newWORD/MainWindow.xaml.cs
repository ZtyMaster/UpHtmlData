using Ivony.Html;
using Ivony.Html.Parser;
using mshtml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Xml;

namespace newWORD
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer Dtimer = new DispatcherTimer();
        private int r { get; set; }
        private int page { get; set; }
        private string Id { get; set; }
        private string URL { get; set; }
        private mshtml.HTMLDocument msl { get; set; }
        List<URL_list> URL_List_ = new List<URL_list>();
        List<Class1> L_Class = new List<Class1>();
        bool fristLoad = true, fristLoad_1=true;
        List<Quyu> Quyulist_58= new List<Quyu>();
        List<Quyu> Quyulist_ganji = new List<Quyu>();



        public class LocationRoad
        {
            public int ID { set; get; }
            public string Code { set; get; }
            public string Info { set; get; }
        }
        public MainWindow()
        {
            InitializeComponent();
            Webbrowser1.Navigating += WebBrowserMain_Navigating;
            this.Webbrowser1.LoadCompleted+=new LoadCompletedEventHandler(webbrowser1_LoadCompleted);

            this.Webbrowser2.LoadCompleted += new LoadCompletedEventHandler(Webbrowser2_LoadCompleted);
            lobr.Visibility = Visibility.Hidden;
            lobr_text.Visibility = Visibility.Hidden;
            page = 1;
            //在本地文件中获取网站地址
            GetURL_lst(URL_List_);
            Webbrowser1.Visibility = Visibility.Hidden;
           
        }

       
        #region 打开程序后执行
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            zc zcwm = new zc();
            zcwm.ShowDialog();
            URL = URL_List_[0].Href;
            newvoid();
        }
        #endregion


        #region WPF 设置WebBrowser控件不弹脚本错误提示框
        void WebBrowserMain_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SetWebBrowserSilent(sender as WebBrowser, true);
        }
        /// <summary>  
        /// 设置浏览器静默，不弹错误提示框  
        /// </summary>  
        /// <param name="webBrowser">要设置的WebBrowser控件浏览器</param>  
        /// <param name="silent">是否静默</param>  
        private void SetWebBrowserSilent(WebBrowser webBrowser, bool silent)
        {
            FieldInfo fi = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                object browser = fi.GetValue(webBrowser);
                if (browser != null)
                    browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
            }
        }
        #endregion

        #region webbrowser加载完成后触发
        void webbrowser1_LoadCompleted(object sender, NavigationEventArgs e)
        {

            mshtml.HTMLDocument mhtml = (mshtml.HTMLDocument)Webbrowser1.Document;
            string html = mhtml.body.innerHTML;
            NewMethod(html, ref fristLoad,ref L_Class);

            if (shaixuan.Text.Trim().Length > 0)
            {
                string str = shaixuan.Text.Trim();
                L_Class = L_Class.Where(x => x.Address.Contains(str)).ToList<Class1>();

            }
            DataGrid.ItemsSource = L_Class;
            CB_qy.ItemsSource = Quyulist_58;
            CB_qy.DisplayMemberPath = "Name";

            CB_qy.SelectedIndex = CB_qy.SelectedIndex == -1 ? 0 : CB_qy.SelectedIndex;

            #region 老版本方法

            //mshtml.HTMLDocument htmlt =(mshtml.HTMLDocument)Webbrowser1.Document;
            //if (htmlt.getElementById("verify_code") == null)
            //{
            //    page = 1;
            //    Webbrowser1.Visibility = Visibility.Hidden;
            //    DataGrid.Visibility = Visibility.Visible;
            //    RoutedEventArgs es = new RoutedEventArgs();
            //    Button_Click(sender, es);
            //}
            //else
            //{
            //    var thist = htmlt.getElementById("verify_code").getAttribute("placeholder");
            //    if ((string)thist != "请输入验证码")
            //    {
            //        page = 1;
            //        Webbrowser1.Visibility = Visibility.Hidden;
            //        DataGrid.Visibility = Visibility.Visible;
            //        RoutedEventArgs es = new RoutedEventArgs();
            //        Button_Click(sender, es);
            //    }
            //}

            // msl = (mshtml.HTMLDocument)Webbrowser1.Document;
            //if (Webbrowser1.Source.ToString() != "http://liaoyang.58.com/ershoufang/0")
            //{
            //    URL = Webbrowser1.Source.ToString();
            //    List<Class1> L_Class = new List<Class1>();
            //    if (GO_58com(L_Class))
            //    {
            //        return;
            //    }
            //    DataGrid.ItemsSource = L_Class;
            //}
            #endregion
        }

        public void NewMethod(string html,ref bool fristLoads, ref List<Class1> L_Class)
        {
            IHtmlDocument document = new JumonyParser().Parse(html);
            //网站第一次加载后读取区域地址
            if (fristLoads)
            {
                GetSelectQuYu(document);
                fristLoads = false;
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

        void Webbrowser2_LoadCompleted(object sender, NavigationEventArgs e)
        {
            mshtml.HTMLDocument mhtml = (mshtml.HTMLDocument)Webbrowser2.Document;
            string html = mhtml.body.innerHTML;
            IHtmlDocument document = new JumonyParser().Parse(html);
            IEnumerable<IHtmlElement> result= document.Find("ul").Where(x => x.Identity() == "houselist-mod-new");
            IEnumerable<IHtmlElement> result_li = result.Find("li");
            IEnumerable<IHtmlElement> result_li1 = document.Find("li>.list-item");

        }
        #region 搜索百姓的方法
        //mshtml.HTMLDocument mhtml = (mshtml.HTMLDocument)Webbrowser2.Document;
        //string html = mhtml.body.innerHTML;
        //IHtmlDocument document = new JumonyParser().Parse(html);
        //IEnumerable<IHtmlElement> result = document.Find("div").Where(i => i.Identity() == "all-list");
        //result = result.Find("li");
        //    foreach (var li in result)
        //    {
        //        Class1 _class = new Class1();

        //_class.TextName = GetN_value(li, ".media-body-title>a");
        //_class.href = li.FindFirst(".media-body-title>a").Attribute("href").Value();
        //_class.Quyu = "百姓";               
        //        _class.Laiyuan = "百姓";
        //        _class.Address = GetN_value(li, ".typo-small");

        //string this_spk = GetN_value(li, ".typo-small");
        //_class.SumMoney = GetN_value(li, ".highlight");
        //string[] thip = this_spk.Split('/');
        //_class.PingMoney = thip.Length>1? thip[1]:"";
        //        _class.Allpm = "";

        //        _class.datetime = GetN_value(li, ".pull-right");
        //        if (li.Exists("img"))
        //        {
        //            if (li.FindFirst("img").Attribute("src").Value().IndexOf("http") < 0)
        //            {
        //                _class.Image_str = string.Empty;
        //            }
        //            else
        //            {
        //                _class.Image_str = "有";
        //            }
        //        }
        //        else if (li.Exists(".img"))
        //        {
        //            if (li.FindFirst(".img").Attribute("src").Value().IndexOf("http") < 0)
        //            {
        //                _class.Image_str = string.Empty;
        //            }
        //            else
        //            {
        //                _class.Image_str = "有";
        //            }
        //        }
        //        else
        //        {
        //            _class.Image_str = string.Empty;
        //        }
                

        //        L_Class.Add(_class);
        //    }

        #endregion
       
        #endregion

        #region 读取网站区域
        private void GetSelectQuYu(IHtmlDocument document)
        {
            IEnumerable<IHtmlElement> result = document.Find("div").Where(i => i.Identity() == "qySelectFirst").Find("a");

            List<URL_list> llt = new List<URL_list>();
            foreach (var rt in result)
            {
                Quyu urt = new Quyu();
                urt.ID = Id;
                urt.Name = rt.InnerText();
                urt.Href = rt.Attribute("href").Value();
                Quyulist_58.Add(urt);
            }
        }
        #endregion


        private void ThreadProcess()
        {

            this.Dispatcher.Invoke(new Action(() =>
            {
                lobr.Maximum = 60;
                lobr.Value = 0;
                lobr.Visibility = Visibility.Visible;
                lobr_text.Visibility = Visibility.Visible;
            }));
            Dtimer.Tick += new EventHandler(Dtimer_Tick);
            Dtimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Dtimer.Start();


        }
        public void Dtimer_Tick(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {

                lobr.Value = r;
                lobr_text.Text = (r / (120 / 100)).ToString();
            }));

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region 原多线程程序
            //Thread NetServer = new Thread(new ThreadStart(ThreadProcess));
            /////UI必要语句
            //NetServer.SetApartmentState(ApartmentState.STA);
            //NetServer.IsBackground = true;
            //NetServer.Start();

            //Thread.Sleep(500);
            //Thread NetServer1 = new Thread(new ThreadStart(newvoid));
            /////UI必要语句
            //NetServer1.SetApartmentState(ApartmentState.STA);
            //NetServer1.IsBackground = true;
            //NetServer1.Start();
            #endregion
            newvoid();
        }
        #region 执行搜索方法
        private void newvoid()
        {
            L_Class.Clear();
            DataGrid.ItemsSource = null;
            r = 0;
            this.Dispatcher.Invoke(new Action(() =>
            {
                page_text.Text = "当前第" + page + "页";
                Page_text.Text = page.ToString();
            }));

            #region 网址重组加载数据


            //-----------------------------赶集网开始读取-----------------------------------------//

            URL = URL_List_[1].Href;
            Id = URL_List_[1].ID;

            URL = page == 1 ? URL : URL + "o" + page.ToString() + "/";
            //对感觉进行抓取
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string html = client.DownloadString(URL);
            IHtmlDocument document = new JumonyParser().Parse(html);
            //首次进入 读取区域数据
            if (fristLoad_1)
            {
                #region MyRegion
                IEnumerable<IHtmlElement> result = document.Find(".warpper").Where(c => c.Attribute("data-key").Value() == "street");
                result = result.Find("a");
                foreach (var fh in result)
                {
                    Quyu qy = new Quyu();
                    qy.Href = fh.Attribute("href").Value();
                    qy.Name = fh.InnerText().Replace("全", "");
                    qy.ID = Id;
                    Quyulist_ganji.Add(qy);
                }
                fristLoad_1 = false;
                #endregion
            }
            GetUrlText_2(document, L_Class);
            //-----------------------------赶集网读取结束-----------------------------------------//
            //-------------------------------读取58开始---------------------------------//

            URL = URL_List_[0].Href;
            Id = URL_List_[0].ID;
            URL = page == 1 ? URL : URL + "/pn" + page.ToString() + "/";


            //58网站对区域进行搜索进行二次修改网站地址
            if (!fristLoad)
            {
                string name = ((newWORD.Quyu)CB_qy.SelectedValue).Href;
                name = name.Remove(name.IndexOf("/0"));
                URL = URL.Replace("/ershoufang", name);
            }
            //对58进行加载
            GO_58com_new();
            //-------------------------------读取58结束---------------------------------//



            //URL = URL_List_[2].Href;
            //Id = URL_List_[2].ID;
            //URL = page == 1 ? URL : URL + "&page=" + page.ToString() ;
            //Webbrowser2.Navigate(URL);

            //-------------------------------读取安居客开始---------------------------------//
            URL = URL_List_[3].Href;
            Id = URL_List_[3].ID;
            URL = page == 1 ? URL : URL + "&page=" + page.ToString() ;
            Webbrowser2.Navigate(URL);
            //-------------------------------读取安居客结束---------------------------------//
            #endregion
            //往前台赋值
            this.Dispatcher.Invoke(new Action(() =>
            {

            }));
            Dtimer.Stop();
            this.Dispatcher.Invoke(new Action(() =>
            {
                lobr.Visibility = Visibility.Hidden;
                lobr_text.Visibility = Visibility.Hidden;
            }));

        }
        #endregion

        private void GO_58com_new()
        {

            Webbrowser1.Navigate(URL);
            
        }
        private bool GO_58com(List<Class1> L_Class)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string html = client.DownloadString(URL);
            
            IHtmlDocument document = new JumonyParser().Parse(html);
            if (document.FindFirst("title").InnerText().Trim()== "请输入验证码")
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    DataGrid.Visibility = Visibility.Collapsed;
                    Webbrowser1.Visibility = Visibility.Visible;
                    Webbrowser1.Navigate(URL);
                }));
                //弹出网站
                //System.Diagnostics.Process.Start(URL);
                return true;

            }
            else
            {
               
                IEnumerable<IHtmlElement> result = document.Find(".house-list-wrap");
                IEnumerable<IHtmlElement> t = result.Find("li");
                Dictionary<string, string> dir = new Dictionary<string, string>();
                foreach (var item in t)
                {
                    #region MyRegion
                    Class1 _class = new Class1();
                   
                    _class.TextName = GetN_value(item, ".title>a");
                    _class.href =item.Exists(".title > a") ? item.FindFirst(".title>a").Attribute("href").Value():string.Empty;
                    _class.Quyu = "同城";
                    _class.PersonName = GetN_value(item, ".jjrname-outer");
                    _class.Laiyuan = "58";
                    IEnumerable<IHtmlElement> adds = item.Find(".baseinfo");
                    IEnumerable<IHtmlElement> addsa = adds.Find("a");
                    string adess = "";
                    foreach (var addsaa in addsa)
                    {
                       adess+= addsaa.InnerText();
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
                return false;
            }
        }
        
        private void GetUrlText_2(IHtmlDocument document, List<Class1> L_Class)
        {
            
            IEnumerable<IHtmlElement> result1 = document.Find(".list-items");            
            foreach (var item in result1)
            {
                #region MyRegion
                Class1 _class = new Class1();
                IHtmlElement item_a = item.FindFirst("a");
                string img_str = item.Exists("img") ? item.FindFirst("img").Attribute("src").Value() : "";
                _class.Image_Count = img_str.Length > 0 ? img_str.IndexOf("default.jpg")>0?0:1:0;
                IEnumerable<IHtmlElement> div = item.Find("div");
                List<string> ls = new List<string>();
                foreach (var d in div)
                {
                    ls.Add(d.InnerText());
                }
                _class.TextName = ls[2];
                _class.SumMoney = ls[4];
                _class.Quyu = "赶集";
                _class.Allpm = ls[1];
                _class.Address = ls[0];
                _class.href = item_a.Attribute("href").Value().Trim();
               
                IEnumerable<IHtmlElement> ssa = item_a.Find("span");

                string item_aa = item_a.ToString().Replace("<!--", "stu1").Replace("-->", "stp2");
                item_aa = item_aa.Substring(item_aa.IndexOf("stu1") + 4);
                item_aa = item_aa.Substring(0, item_aa.IndexOf("stp2"));
                item_aa = item_aa.Substring(item_aa.IndexOf(">") + 1);
                item_aa = item_aa.Substring(0, item_aa.IndexOf("<"));
                _class.datetime = item_aa;
                string[] pm = _class.Allpm.Split(' ');

                double pm_int = Convert.ToDouble(pm[6].Replace('㎡', ' ').Trim().Length <= 0 ? pm[5].Replace('㎡', ' ').Trim() : pm[6].Replace('㎡', ' ').Trim());
                double ss = ((Convert.ToDouble(_class.SumMoney.Replace("万元", "").Trim()) / pm_int));
                _class.PingMoney = "≈" + Convert.ToInt32(ss * 10000).ToString();
                _class.Image_str = _class.Image_Count > 0 ? "有" : string.Empty;
                
                L_Class.Add(_class);
                r++;
                #endregion
            }
        }
        private static string G_mtdVoid(string huxing, IHtmlElement spn, string p)
        {
            return spn.Exists(p) ? spn.FindFirst(p).InnerText() : huxing;
        }
        private static List<string> LIST_G(string[] str_)
        {
            //string[] ist = null;
            List<string> ist = new List<string>();

            foreach (var s in str_)
            {
                if (s != "")
                { ist.Add(s); }
            }
            return ist;
        }

        private static string GetN_value(IHtmlElement item, string str)
        {
            return item.Exists(str) ? item.FindFirst(str).InnerText().Trim() : string.Empty;
        }
        private static int GetInt_value(IHtmlElement item, string str)
        {
            return item.Exists(str) ? Convert.ToInt32(item.FindFirst(str).InnerText().Trim().Replace("图","") ):0;
        }

        private string NewMethodhref(IEnumerable<IHtmlElement> item_a)
        {
            string ret = string.Empty;
            foreach (var f in item_a)
            {
                ret = f.Attribute("href").Value();
            }
            return ret;
        }

        private string NewMethodtext(IEnumerable<IHtmlElement> item_a)
        {
            string ret = string.Empty;
            foreach (var f in item_a)
            {
                ret = f.InnerText().Trim();
            }
            return ret;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Class1 class1 = DataGrid.SelectedItem as Class1;
            if (class1!=null)
            {
                class1.ID = class1.Quyu == "百姓" ? "3" : (class1.Quyu == "同城" ? "1" : "2");
                Show show = new newWORD.Show();
                show.Href = class1.href;
                show.class1 = class1;
                show.ShowDialog();
            }
            

        }
       
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1();
            w.Show();
        }

        public static void GetURL_lst(List<URL_list> URL_List_)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(@"..\..\XMLFile1.xml", settings);

            xmlDoc.Load(reader);
            reader.Close();
            // 得到根节点bookstore
            XmlNode xn = xmlDoc.SelectSingleNode("bookstore");
            // 得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;

            foreach (XmlNode xn1 in xnl)
            {
                #region MyRegion
                URL_list uls = new URL_list();
                // 将节点转换为元素，便于得到节点的属性值
                XmlElement xe = (XmlElement)xn1;
                // 得到Type和ISBN两个属性的属性值
                uls.ID = xe.GetAttribute("ID").ToString();
                uls.Name = xe.GetAttribute("Type").ToString();
                // 得到Book节点的所有子节点
                XmlNodeList xnl0 = xe.ChildNodes;
                uls.Href = xnl0.Item(0).InnerText;
                URL_List_.Add(uls);
                #endregion
            }

           
            xmlDoc.Clone();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SaveDate sd = new SaveDate();
            sd.Show();
        }

       
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            log l = new log();
            l.Show();
        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            msl = (mshtml.HTMLDocument)Webbrowser1.Document;
            msl.getElementById("keyword1").setAttribute("value", SelectSTR.Text);
            Webbrowser1.InvokeScript("eval", "document.getElementById('searchbtn1').click()");
        }


        #region 页面跳转
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                page = Convert.ToInt32(Page_text.Text);
                if (page > 0)
                {
                    this.Button_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("非法页数！");
                }
            }
            catch
            { MessageBox.Show("非法页数！"); }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            page = 1;

            this.Button_Click(sender, e);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            page++;
            this.Button_Click(sender, e);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            page--;
            this.Button_Click(sender, e);
        }
        #endregion
    }

}
