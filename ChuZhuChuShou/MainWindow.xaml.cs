using Ivony.Html;
using Ivony.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace ChuZhuChuShou
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
        List<Class1> L_Class = new List<Class1>();
        public bool WhileBOOL1 { get; set; }
        public bool WhileBOOL58 { get; set; }
        public bool BXWhileBOOL1 { get; set; }
        public bool CJTXWhileBOOL1 { get; set; }
        CookieContainer ccr = new CookieContainer();
        public DateTime MaxTime { get; set; }
        public int r { get; set; }
        public int i { get; set; }
        public int smi { get; set; }
        public int BXsmi { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (QiuZhuURL.Text.Trim().Length <= 0 )
            {

                MessageBox.Show("必须添加更新地址才可更新！");
                return;
            }
            //同城
            Thread htmlHelper = new Thread(GoUpN);
            htmlHelper.SetApartmentState(ApartmentState.STA);
            htmlHelper.IsBackground = true;
            htmlHelper.Start();
        }

        #region 百姓网求组更新
        private void BaixinGO()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                BXupgengxin.Text = "百姓求租更新开启！";

            }));

            while (true)
            {
                BXsmi++;
                L_Class.Clear();
                Thread bxhtmlHelper = new Thread(BaiXingNewHTMLhreper);
                bxhtmlHelper.SetApartmentState(ApartmentState.STA);
                bxhtmlHelper.IsBackground = true;
                bxhtmlHelper.Start();
                BXWhileBOOL1 = true;
                // 使用多现场进行更新  
                while (BXWhileBOOL1)
                {
                    System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                }
                bxhtmlHelper.Abort();
                this.Dispatcher.Invoke(new Action(() =>
                {
                    BXupgengxin.Text = "等待地" + (BXsmi + 1) + "次开启！";

                }));

                Thread.Sleep(180000);
            }
        }
        #endregion
        #region 求租更新
        private void GoUpN()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                upgengxin.Text = "求租更新开启！";

            }));
          
            while (true)
            {
                smi++;
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
                this.Dispatcher.Invoke(new Action(() =>
                {
                    upgengxin.Text = "等待地" + (smi + 1) + "次开启！";

                }));
              
                Thread.Sleep(180000);
            }
        }
        public static string GetN_value(IHtmlElement item, string str)
        {
            return item.Exists(str) ? item.FindFirst(str).InnerText().Trim() : string.Empty;
        }
        private static int IFthis_I(int ret)
        {
            ret = ret > 0 ? ret-- : ret;
            return ret;
        }
        public static int GetInt_value(IHtmlElement item, string str)
        {
            return item.Exists(str) ? Convert.ToInt32(item.FindFirst(str).InnerText().Trim().Replace("图", "")) : 0;
        }
        private void BaiXingNewHTMLhreper()
        {
            string url = "http://liaoyang.baixing.com/qiufang/";
            string ThisHtml = BXGetHTMLstr(url);
            using (var ctx = new oaEntities())
            {
                DateTime DBtime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());

                IHtmlDocument document = new JumonyParser().Parse(ThisHtml);
                IEnumerable<IHtmlElement> result = document.Find(".media-body-title");
                List<Class1> Ncss = new List<Class1>();       
                foreach (var item in result)
                {
                    Class1 Class1 = new Class1();
                    item.FindFirst("a");
                    Class1.href = item.Exists("a") ? item.FindFirst("a").Attribute("href").Value() : string.Empty;
                    Class1.TextName = MainWindow.GetN_value(item, "a");
                    //开始读取子连接
                    #region 读取子连接
                    string ThisZ = BXGetHTMLstr(Class1.href);
                    IHtmlDocument document_1 = new JumonyParser().Parse(ThisZ); 
                    IEnumerable<IHtmlElement> restime = document_1.Find("div>.viewad-topMeta");
                    foreach (var tm in restime)
                    {

                    }
                        #endregion
                  Ncss.Add(Class1);
                }
               
            }
            //media - body - title
        }
        private void NewHTMLhreper()
        {
          
            try
            {
                string url = "";
                this.Dispatcher.Invoke(new Action(() =>
                {
                    url = QiuZhuURL.Text;

                }));

                string ALLhtml = GetHTMLstr(url);
                using (var ctx = new oaEntities())
                {
                    DateTime DBtime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
                    var QZQtime = ctx.T_QiuZhuQiuGou.Count();
                    if (QZQtime <= 0)
                    {

                    }
                    MaxTime =QZQtime<=0? new DateTime(2016, 12, 12): ctx.T_QiuZhuQiuGou.DefaultIfEmpty().Max(x => x.FBtime);
                    #region 开始读取网站列表信息
                    IHtmlDocument document = new JumonyParser().Parse(ALLhtml);
                    IEnumerable<IHtmlElement> result = document.Find(".tblist");
                    IEnumerable<IHtmlElement> t = result.Find("tr");
                    //Dictionary<string, string> dir = new Dictionary<string, string>();                  
                    foreach (var item in t)
                    {
                       
                        #region MyRegion
                        Class1 _class = new Class1();
             
                        _class.href = item.Exists(".t > a") ? item.FindFirst(".t>a").Attribute("href").Value() : string.Empty;
                        _class.TextName = MainWindow.GetN_value(item, ".t");

                      

                        _class.Quyu = MainWindow.GetN_value(item, ".c_58"); 
                        _class.PingMoney = MainWindow.GetN_value(item, ".pri");

                        _class.PersonName = MainWindow.GetN_value(item, ".jjrname-outer");
                        _class.Laiyuan = "58"; 
                        string  Timestr;
                        IEnumerable<IHtmlElement> adds = item.Find(".tc");
                        int abc_i = 0;
                        foreach (var abc in adds)
                        {
                            if (abc_i == 1)
                            {
                                _class.FwHuXing = abc.InnerText();
                            }
                            else if (abc_i == 2)
                            {
                                Timestr = abc.InnerText();
                            }
                            abc_i++;
                        }
                        #region 进入子页面
                        string html = GetHTMLstr(_class.href);
                        IHtmlDocument document_1 = new JumonyParser().Parse(html);
                        IEnumerable<IHtmlElement> result1 = document_1.Find(".user");
                        IEnumerable<IHtmlElement> restime = document_1.Find(".other");
                        foreach (var tm in restime)
                        {
                            var tttm = tm.InnerText();
                            _class.time = Convert.ToDateTime(tttm.Substring(tttm.IndexOf("：") + 1).Substring(0, tttm.Substring(tttm.IndexOf("：") + 1).IndexOf("浏览")).Trim());

                        }

                        if (QZQtime > 0)
                        {
                            var listc = ctx.T_QiuZhuQiuGou.Where(u => u.Hname == _class.TextName && u.FBtime==_class.time);
                            if (listc.Count() >= 1)
                            {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    LISTbox.Text = "重复信息退出更新等待下一次更新———" + _class.TextName + "———" + DateTime.Now.ToString();

                                }));
                                WhileBOOL1 = false;
                                return;
                            }

                        }
                        foreach (var ts in result1)
                        {
                            _class.PersonName = ts.FindLast(".tx").InnerText();
                            string strimage = ts.FindFirst(".phone").InnerText();
                            strimage = strimage.Substring(strimage.IndexOf("'")+1);
                            _class.Image_str = strimage.Substring(0, strimage.IndexOf("'"));
                            _class.bak = ts.FindFirst(".belong").InnerText();                           
                        }

                        #endregion

                        #endregion

                        #region 写入系统
                        T_QiuZhuQiuGou tqzq = new T_QiuZhuQiuGou();
                        tqzq.Hname = _class.TextName;
                        tqzq.QuYu = _class.Quyu;
                        tqzq.JuShi = _class.FwHuXing;
                        tqzq.Money = _class.PingMoney;
                        tqzq.FBtime = _class.time;
                        tqzq.Hperson = _class.PersonName;
                        tqzq.Photo = _class.Image_str;
                        tqzq.GuiShuDi = _class.bak;
                       
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            tqzq.CityID = Convert.ToInt32(QiuZhuCombx1.SelectedValue);

                        }));

                        ctx.T_QiuZhuQiuGou.Add(tqzq);
                        #endregion
                      //  L_Class.Add(_class);
                        r++;
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            SUMcount.Text = r.ToString();

                        }));
                        ctx.SaveChanges();
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            LISTbox.Text = "更新一条数据————"+ _class.TextName+ "———" + DateTime.Now.ToString();

                        }));
                    }
                    #endregion
                }
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
        private void SaveDataHTML(oaEntities ctx, List<newWORD.Class1> L_Class_)
        {
            foreach (var da in L_Class_)
            {
                T_FGJHtmlData tf = new T_FGJHtmlData();

                tf.HLName = da.TextName;
                tf.HLhref = da.href;
                tf.PersonName = da.PersonName;
                tf.Address = da.Address;
                tf.photo = da.photo;
                tf.FbTime = da.FbTime;
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
                tf.CityID = 1;
                if (da.FwMianji != null)
                {
                    tf.MianjiID = GeiMinji(da.FwMianji);
                }
                if (da.FwSumMoney != null)
                {
                    tf.SumMoneyID = GetMoney(da.FwSumMoney);
                }
                if (da.FwHuXing != null)
                {
                    tf.HuXingID = GetHuxing(da.FwHuXing);
                }
                //如果数据库中出现该名称并且 时间在当前时间那么  该信息不写入数据库
                //如果有该数据那么修改数据库中的数据信息
                if (ctx.T_FGJHtmlData.FirstOrDefault(x => x.HLName == da.TextName) != null)
                {
                    var thiseti = ctx.T_FGJHtmlData.FirstOrDefault(x => x.HLName == da.TextName);
                    using (var newoa = new oaEntities())
                    {
                        var temp = newoa.T_FGJHtmlData.FirstOrDefault(x => x.ID == thiseti.ID);
                        temp.FbTime = da.FbTime;
                        temp.FwSumMoney = da.FwSumMoney;
                        newoa.Entry<T_FGJHtmlData>(temp).State = System.Data.EntityState.Modified;
                        newoa.SaveChanges();
                    }
                }
                else
                {
                    ctx.T_FGJHtmlData.Add(tf);
                }
            }
        }     
        private string GetHTMLstr(string url)
        {
            string ALLhtml;
            AutoUpData.HttpHeader header = new AutoUpData.HttpHeader();            
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "GET";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;

            ccr = ccr.Count < 0 ? AutoUpData.HTMLHelper.GetCooKie(url, "PGTID=0d30000c-007f-6cea-00fc-a261cfc9171c&ClickID=1", header) : ccr;
            ALLhtml = AutoUpData.HTMLHelper.GetHtml(url, ref ccr, header);
            return ALLhtml;
        }
        private string BXGetHTMLstr(string url)
        {
            string ALLhtml;
            AutoUpData.HttpHeader header = new AutoUpData.HttpHeader();
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "GET";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;

            ccr = ccr.Count < 0 ? AutoUpData.HTMLHelper.GetCooKie(url, "", header) : ccr;
            ALLhtml = AutoUpData.HTMLHelper.GetHtml(url, ref ccr, header);
            return ALLhtml;
        }     
        #endregion
        //更新出租信息
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (FTXtexturl.Text.Trim().Length <= 0)
            {

                MessageBox.Show("必须添加更新地址才可更新！");
                return;
            }
            //房天下更新
            Thread htmlHelper = new Thread(FangTianXia);
            htmlHelper.SetApartmentState(ApartmentState.STA);
            htmlHelper.IsBackground = true;
            htmlHelper.Start();
        }
        private void FangTianXia()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                FtxiaZT.Text = "城际分类更新开启！";

            }));

            while (true)
            {
                smi++;
                L_Class.Clear();
                Thread htmlHelper = new Thread(CJTXNewHTMLhreper);
                htmlHelper.SetApartmentState(ApartmentState.STA);
                htmlHelper.IsBackground = true;
                htmlHelper.Start();
                CJTXWhileBOOL1 = true;
                // 使用多现场进行更新  
                while (CJTXWhileBOOL1)
                {
                    System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                }
                htmlHelper.Abort();
                this.Dispatcher.Invoke(new Action(() =>
                {
                    uPCount.Text = "等待地" + (smi + 1) + "次开启！";

                }));

                Thread.Sleep(180000);
            }
        }
        
        private void CJTXNewHTMLhreper()
        {
            
            try
            {
                string url = "";
                this.Dispatcher.Invoke(new Action(() =>
                {
                    url = FTXtexturl.Text;

                }));
                string ALLhtml = BXGetHTMLstr(url);
                using (var ctx = new oaEntities())
                {
                    //获取当前时间
                    DateTime DBtime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
                    var QZQtime = ctx.T_ChuZhuInfo.Count();
                    if (QZQtime <= 0)
                    {

                    }
                    MaxTime = QZQtime <= 0 ? new DateTime(2016, 12, 12) : Convert.ToDateTime(ctx.T_ChuZhuInfo.DefaultIfEmpty().Max(x => x.FbTime));

                    #region 开始读取网站列表信息
                    IHtmlDocument document = new JumonyParser().Parse(ALLhtml);
                    IEnumerable<IHtmlElement> result = document.Find(".hpic");

                    foreach (var item in result)
                    {

                        #region MyRegion
                        Class1 _class = new Class1();

                        _class.href = item.Exists(".pic_box") ? item.FindFirst(".pic_box").Attribute("href").Value() : string.Empty;
                        _class.TextName = item.Exists(".pic_box") ? item.FindFirst(".pic_box").Attribute("title").Value() : string.Empty;
                        

                        if (QZQtime > 0)
                        {
                            var listc = ctx.T_ChuZhuInfo.Where(u => u.ChuZhuName == _class.TextName && u.ChuZhuHref == _class.href);
                            if (listc.Count() >= 1)
                            {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    ltextd.Text = "重复信息退出更新等待下一次更新———" + _class.TextName + "———" + DateTime.Now.ToString();

                                }));
                                CJTXWhileBOOL1 = false;
                                return;
                            }

                        }

                        _class.Address = MainWindow.GetN_value(item, ".addr");
                        _class.SumMoney = MainWindow.GetN_value(item, ".pr");
                        _class.Laiyuan = "cjfl";

                        #region 进入子页面
                        string html = GetHTMLstr(_class.href);
                        IHtmlDocument document_1 = new JumonyParser().Parse(html);
                        IEnumerable<IHtmlElement> result1 = document_1.Find(".viewad_box>.viewad-meta > li");
                        IEnumerable<IHtmlElement> result2 = document_1.Find(".view_box>p");
                        IEnumerable<IHtmlElement> result3 = document_1.Find(".viewad-actions");
                        IEnumerable<IHtmlElement> image = document_1.Find(".container>.pic_box");

                        int i = 0;
                        foreach (var tm in result1)
                        {
                            if (i == 2)
                            {
                                _class.Mianji = MainWindow.GetN_value(tm, "span");
                            }
                            if (i == 4)
                            {
                                _class.PersonName = MainWindow.GetN_value(tm, "span");
                            }
                            if (i == 5)
                            {
                                _class.photo = MainWindow.GetN_value(tm, "span");
                            }
                            i++;

                        }
                        i = 0;
                        foreach (var ts in result2)
                        {
                            if (i == 1)
                            { _class.bak = ts.InnerHtml(); }
                            i++;

                        }
                        string imagerui = "";
                        foreach (var tm in image)
                        {
                            imagerui = imagerui + "---" + tm.FindFirst("img ").Attribute("src").Value();

                        }
                        _class.Image_str = imagerui;
                        foreach (var ts in result3)
                        {
                            _class.FbTime = Convert.ToDateTime(ts.FindFirst("span").InnerHtml());

                        }

                        #endregion

                        #endregion

                        #region 写入系统

                        if (item.Exists("a"))
                        {
                            T_ChuZhuInfo tqzq = new T_ChuZhuInfo();
                            tqzq.ChuZhuName = _class.TextName;
                            tqzq.Addess = _class.Address;
                            tqzq.Money = Convert.ToDecimal(_class.SumMoney);
                            tqzq.LaiYuan = _class.Laiyuan;
                            tqzq.Images = _class.Image_str;
                            tqzq.FbTime = _class.FbTime;
                            tqzq.PingMi = Convert.ToDecimal(_class.Mianji.Replace("㎡", ""));
                            tqzq.LianXiPerson = _class.PersonName;
                            tqzq.LianXiPhoto = _class.photo;
                            tqzq.Bak = _class.bak;
                            tqzq.ChuZhuHref = _class.href;

                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                tqzq.CityID = Convert.ToInt32(ftxCombx.SelectedValue);

                            }));
                            ctx.T_ChuZhuInfo.Add(tqzq);
                            //  L_Class.Add(_class);
                            r++;
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                ADDC.Text = r.ToString();

                            }));
                            ctx.SaveChanges();
                        }

                        #endregion

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            ltextd.Text = "更新一条数据————" + _class.TextName + "———" + DateTime.Now.ToString();

                        }));
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    ltextd.Text = DateTime.Now.ToString() + ex.ToString();

                }));

            }
            WhileBOOL1 = false;
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

        private void lo_Loaded(object sender, RoutedEventArgs e)
        {

            using (var City = new oaEntities())
            {
                var prot = City.T_province.Where(x => x.ID > 0);
                ProtCmb.ItemsSource = prot.ToList();
                ProtCmb.SelectedValuePath = "ID";
                ProtCmb.DisplayMemberPath = "str";
                ProtCmb.SelectedIndex = 0;
                protcb2.ItemsSource = prot.ToList();
                protcb2.SelectedValuePath = "ID";
                protcb2.DisplayMemberPath = "str";
                protcb2.SelectedIndex = 0;

                up58name.ItemsSource = prot.ToList();
                up58name.SelectedValuePath = "ID";
                up58name.DisplayMemberPath = "str";
                up58name.SelectedIndex = 0;
            }
           
        }

        private void ProtCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
            using (var City = new oaEntities())
            {
                var Mcity = City.T_City.Where(x => x.DelFlag == 0 && x.province_id == strid);
                QiuZhuCombx1.ItemsSource = Mcity.ToList();
                QiuZhuCombx1.SelectedValuePath = "ID";
                QiuZhuCombx1.DisplayMemberPath = "City";
                QiuZhuCombx1.SelectedValue = 1;
                if (Mcity.ToList().Count == 0)
                {
                    return;
                }
                int id = Mcity.ToList()[0].ID;
                var href = City.T_UpHerfCity.Where(x => x.Items == "58求租" && x.City_ID == id).FirstOrDefault();
                if(href == null) {
                    return;
                }
                QiuZhuURL.Text = href.Href;
            }
        }

        private void QiuZhuCombx1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
                using (var City = new oaEntities())
                {
                    var href = City.T_UpHerfCity.Where(x => x.Items == "58求租" && x.City_ID == strid).FirstOrDefault();
                    QiuZhuURL.Text = href == null ? "" : href.Href;
                }
            }
            catch (Exception err)
            {
                string ss = err.ToString();
            }
        }
        private void protcb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
            using (var City = new oaEntities())
            {
                var Mcity = City.T_City.Where(x => x.DelFlag == 0 && x.province_id == strid);
                ftxCombx.ItemsSource = Mcity.ToList();
                ftxCombx.SelectedValuePath = "ID";
                ftxCombx.DisplayMemberPath = "City";
                ftxCombx.SelectedValue = 1;
                if (Mcity.ToList().Count == 0)
                {
                    return;
                }
                int id = Mcity.ToList()[0].ID;
                var href = City.T_UpHerfCity.Where(x => x.Items == "城际分类出租" && x.City_ID == id).FirstOrDefault();
                if(href == null) {
                    return;
                }
                FTXtexturl.Text = href.Href;
            }
        }

        private void up58name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
            using (var City = new oaEntities())
            {
                var Mcity = City.T_City.Where(x => x.DelFlag == 0 && x.province_id == strid);
                up58city.ItemsSource = Mcity.ToList();
                up58city.SelectedValuePath = "ID";
                up58city.DisplayMemberPath = "City";
                up58city.SelectedValue = 1;
                if (Mcity.ToList().Count == 0)
                {
                    return;
                }
                int id = Mcity.ToList()[0].ID;
                var href = City.T_UpHerfCity.Where(x => x.Items == "58出租" && x.City_ID == id).FirstOrDefault();
                if (href == null) {
                    return;
                }
                up58nameurl.Text = href.Href;
            }
        }

        private void ftxCombx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
                using (var City = new oaEntities())
                {
                    var href = City.T_UpHerfCity.Where(x => x.Items == "城际分类出租" && x.City_ID == strid).FirstOrDefault();
                    FTXtexturl.Text = href == null ? "" : href.Href;
                }
            }
            catch (Exception err)
            {
                string ss = err.ToString();
            }
        }
        private void up58city_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int strid = Convert.ToInt32(((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString());
                using (var City = new oaEntities())
                {
                    var href = City.T_UpHerfCity.Where(x => x.Items == "58出租" && x.City_ID == strid).FirstOrDefault();
                    up58nameurl.Text = href == null ? "" : href.Href;
                }
            }
            catch (Exception err)
            {
                string ss = err.ToString();
            }
        }
        //58出租信息
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (up58nameurl.Text.Trim().Length <= 0)
            {

                MessageBox.Show("必须添加更新地址才可更新！");
                return;
            }
            //同城
            Thread htmlHelper = new Thread(GoUp58chuzhu);
            htmlHelper.SetApartmentState(ApartmentState.STA);
            htmlHelper.IsBackground = true;
            htmlHelper.Start();
        }

        private void GoUp58chuzhu()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                upgengxin58.Text = "58出租更新开启！";

            }));

            while (true)
            {
                smi++;
                L_Class.Clear();
                Thread htmlHelper = new Thread(NewHTMLhreper58);
                htmlHelper.SetApartmentState(ApartmentState.STA);
                htmlHelper.IsBackground = true;
                htmlHelper.Start();
                WhileBOOL58 = true;
                // 使用多现场进行更新  
                while (WhileBOOL58)
                {
                    System.Windows.Forms.Application.DoEvents(); // 等待本次加载完毕才执行下次循环. 
                }
                htmlHelper.Abort();
                this.Dispatcher.Invoke(new Action(() =>
                {
                    upgengxin58.Text = "等待地" + (smi + 1) + "次开启！";

                }));

                Thread.Sleep(180000);
            }
        }
        private void NewHTMLhreper58()
        {

           
                string url = "";
                this.Dispatcher.Invoke(new Action(() =>
                {
                    url = up58nameurl.Text;

                }));

                string ALLhtml = GetHTMLstr(url);
                int cityItems = 0;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    cityItems = Convert.ToInt32(up58city.SelectedValue);
                }));
                using (var ctx = new oaEntities())
                {
                    DateTime DBtime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
                    var QZQtime = ctx.T_ChuZhuInfo.Count();
                    if (QZQtime <= 0)
                    {

                    }
                    var timesdata = ctx.T_ChuZhuInfo.Where(x => x.LaiYuan == "58").DefaultIfEmpty();
                    var MaxTime = timesdata.First()!=null?(DateTime) timesdata.Max(x => x.FbTime) : new DateTime(2016, 12, 12);

                   // MaxTime = QZQtime <= 0 ? new DateTime(2016, 12, 12) : times ;
                    #region 开始读取网站列表信息
                    IHtmlDocument document = new JumonyParser().Parse(ALLhtml);
                    IEnumerable<IHtmlElement> result = document.Find(".listBox");
                    IEnumerable<IHtmlElement> t = result.Find("li");
                    DateTime Ttdtime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1");
                    var TTimeData = ctx.T_ChuZhuInfo.Where(x => x.FbTime >= Ttdtime).DefaultIfEmpty();
                    //Dictionary<string, string> dir = new Dictionary<string, string>();                  
                    foreach (var item in t)
                    {
                      //  DateTime uptime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString());
                     //   DateTime NOtime = DateTime.Now;
                      //  var drtme = ctx.T_FGJHtmlData.DefaultIfEmpty().Where(x => x.FbTime != uptime && x.Laiyuan == "58" && x.CityID == cityItems && x.FbTime <= NOtime).FirstOrDefault();
                      //  MaxTime = drtme == null ? uptime.AddDays(-1) : ctx.T_FGJHtmlData.DefaultIfEmpty().Where(x => x.FbTime != uptime && x.Laiyuan == "58" && x.CityID == cityItems && x.FbTime <= NOtime).Max(x => x.FbTime);
                        #region 开始读取网站列表信息
                       // IHtmlDocument document58 = new JumonyParser().Parse(ALLhtml);
                       // IEnumerable<IHtmlElement> result58 = document58.Find(".house-list-wrap");
                      //  IEnumerable<IHtmlElement> t58 = result.Find("li");
                        Dictionary<string, string> dir = new Dictionary<string, string>();
                        int ret = 0;
                        #region MyRegion
                        newWORD.Class1 _class = new newWORD.Class1();

                        IEnumerable<IHtmlElement> sendTime = item.Find(".sendTime");
                        string Timestr="";
                        foreach (var h1 in sendTime)
                        {
                            Timestr = h1.InnerText();
                        }
                        DateTime Dte = DateTime.Now;
                        _class.FbTime = Convert.ToDateTime(Dte.Year.ToString() + "-" + Dte.Month.ToString() + "-" + Dte.Day.ToString());
                        string timerss = Timestr.Replace(" ", "");
                        if (timerss.Length > 0)
                        {
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
                                    _class.FbTime = Convert.ToDateTime((_class.FbTime.Year - 1) + "-" + strTime[0] + "-" + strTime[1]);
                                }

                                if (_class.FbTime <= MaxTime)
                                {

                                    if (ret >= 2)
                                    {
                                        break;
                                    }
                                    ret++;
                                }
                                else
                                {
                                    ret = 0;
                                }
                                #endregion
                            }
                        }
                        else {
                            _class.FbTime = _class.FbTime.AddDays(-1);
                        }


                        IEnumerable<IHtmlElement> hname = item.Find("h2");
                        foreach (var h1 in hname)
                        {
                           _class.TextName= h1.InnerText();
                        }
                    if (_class.TextName == null)
                    {
                        continue;
                    }
                        IEnumerable<IHtmlElement> tingshi = item.Find(".room");
                        foreach (var h1 in tingshi)
                        {
                            _class.FwHuXing = h1.InnerText();
                        }
                        string pinx = _class.FwHuXing;
                       
                        pinx = pinx.Remove(0, pinx.IndexOf("卫") + 1).Replace("㎡", "").Replace(" ","");
                        _class.FwHuXing = _class.FwHuXing.Replace(pinx,"");

                        _class.FwMianji = pinx.Remove(0, pinx.IndexOf("  ") + 1).Replace("  ", "");
                        IEnumerable<IHtmlElement> addess = item.Find(".add");
                        foreach (var h1 in addess)
                        {
                            _class.Address = h1.InnerText();
                        }

                        IEnumerable<IHtmlElement> person = item.Find(".geren");
                        foreach (var h1 in person)
                        {
                            _class.PersonName = h1.InnerText();
                        }
                        _class.PersonName = _class.PersonName.Replace("来自个人房源：", "");
                        IEnumerable<IHtmlElement> images = item.Find("img");
                        foreach (var h1 in images)
                        {
                            _class.Image_str= "---" + h1.Attribute("lazy_src").Value();                          
                        }

                        IEnumerable<IHtmlElement> money = item.Find(".money>b");
                        foreach (var h1 in money)
                        {
                            _class.SumMoney = h1.InnerText();
                        }
                        if (item.Exists("h2 > a")) {
                            IEnumerable<IHtmlElement> href = item.Find("h2>a");
                            foreach (var h1 in href)
                            {
                                _class.href = h1.Attribute("href").Value();
                            }
                        }
                        _class.Laiyuan = "58";
                        r++;
                        #endregion
                        T_ChuZhuInfo tqzq = new T_ChuZhuInfo();
                        tqzq.ChuZhuName = _class.TextName;
                        tqzq.Addess = _class.Address;
                        int tmp;
                        if (!int.TryParse(_class.SumMoney, out tmp))
                        {
                            _class.SumMoney = "0";
                        }

                        tqzq.Money =  Convert.ToDecimal(_class.SumMoney);
                        tqzq.LaiYuan = _class.Laiyuan;
                        tqzq.Images = _class.Image_str;
                        tqzq.FbTime = _class.FbTime;
                        tqzq.PingMi = Convert.ToDecimal(_class.FwMianji.Replace(" ", ""));
                        tqzq.LianXiPerson = _class.PersonName;
                        tqzq.LianXiPhoto = _class.photo;
                        tqzq.Bak = _class.bak;
                        tqzq.ChuZhuHref = _class.href;
                        tqzq.CityID = cityItems;
                        var distic= TTimeData.Where(x => x.ChuZhuName == tqzq.ChuZhuName && x.Addess == tqzq.Addess && x.PingMi == tqzq.PingMi && x.Money == tqzq.Money).FirstOrDefault();
                        if (distic == null) {
                            ctx.T_ChuZhuInfo.Add(tqzq);
                            ctx.SaveChanges();
                        }
                        

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            ADDC58.Text = r.ToString();

                        }));
                      
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            ltextd58.Text = "更新一条数据————" + _class.TextName + "———" + DateTime.Now.ToString();

                        }));
                        #endregion

                    }
                    #endregion
                }
            WhileBOOL58 = false;
            try
            {
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    LISTbox.Text = DateTime.Now.ToString() + ex.ToString();

                }));

            }
          
        }
    }
}
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
public class URL_list
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Href { get; set; }
}
public class Quyu
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Href { get; set; }
}