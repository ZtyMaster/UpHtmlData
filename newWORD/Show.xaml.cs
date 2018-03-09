using Ivony.Html;
using Ivony.Html.Parser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net;
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
using System.Xml;

namespace newWORD
{
    /// <summary>
    /// Show.xaml 的交互逻辑
    /// </summary>
    /// 
   
    public partial class Show : Window
    {
        public  string Href { get; set; }
        public Class1 class1 { get; set; }
        public int Page { get; set; }
        public int MaxPage { get; set; }
        public List<string> liImg = new List<string>();
        public Show()
        {
            InitializeComponent();
            Webbrowser1.Navigating += WebBrowserMain_Navigating;
            this.Webbrowser1.LoadCompleted += new LoadCompletedEventHandler(webbrowser1_LoadCompleted);

            Webbrowser2.Navigating += WebBrowserMain_Navigating;
            Webbrowser2.LoadCompleted += new LoadCompletedEventHandler(webbrowser2_LoadCompleted);
        }
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
            Webbrowser1 = Webbrowser2;
            string html = mhtml.body.innerHTML;
            IHtmlDocument document_1 = new JumonyParser().Parse(html);
           
            IEnumerable<IHtmlElement> rl = document_1.Find("div");
            string sss = "";
            foreach (var rs in rl)
            {
                if (rs.Exists("p"))
                {
                    if (rs.FindFirst("p").Identity() == "smallPicDescShow")
                    {
                        sss = rs.FindFirst("p").InnerText();
                    }
                }

            }
            IEnumerable<IHtmlElement> ul_il = document_1.Find("ul").Where(p => p.Identity() == "leftImg");
            IEnumerable<IHtmlElement> li = ul_il.Find("li");

            foreach (var img in li)
            {
                liImg.Add(img.FindFirst("img").Attribute("src").Value());
            }
            IEnumerable<IHtmlElement> Phon = document_1.Find("div").Where(d => d.Identity() == "houseChatEntry");
            IEnumerable<IHtmlElement> Phon_p = Phon.Find(".phone-num");
            string phone = "";
            foreach (var p in Phon_p)
            {
                phone = p.InnerText();
            }
            PersonnamePhoto.Text = phone;
            Bak.Text = sss;
            Personnametext.Text = class1.PersonName;
            text_.Text = class1.TextName;
            this.Title = class1.TextName;
            class1.photo = phone;
            class1.bak = sss;
            MaxPage = liImg.Count;
            GoPage(0);
            
        }
        void webbrowser2_LoadCompleted(object sender, NavigationEventArgs e)
        {
            mshtml.HTMLDocument mhtml = (mshtml.HTMLDocument)Webbrowser2.Document;
            Webbrowser2.Navigate("http://#");
            string html = mhtml.body.innerHTML;
            IHtmlDocument document_1 = new JumonyParser().Parse(html);
           
        }
        #endregion
        private void Loadeds()
        {

            Webbrowser1.Navigate(Href);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (class1.ID == "1")
                {
                    Loadeds();
                }
                else if (class1.ID == "2")
                {
                    Loadeds1();
                }
                else
                {
                    Webbrowser2.Navigate(Href);
                }
                
            }
            catch 
            {
                MessageBox.Show("获取图片格式错误！");
            }
          
        }

        private void Loadeds1()
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string ThisHtml = client.DownloadString(Href);
            IHtmlDocument document_1 = new JumonyParser().Parse(ThisHtml);
            IEnumerable<IHtmlElement> rl = document_1.Find("div");
           
            IEnumerable<IHtmlElement> personname = rl.Find(".broker");
            IEnumerable<IHtmlElement> photo = rl.Find(".tel");

            IEnumerable<IHtmlElement> text_aaa = rl.Find(".house-mian-info");
            string this_ = "";
            foreach (var aaa in text_aaa)
            {
               string ss= aaa.InnerText();
                string[] sss = ss.Split(' ');
                
                foreach (var lss in sss)
                {
                    this_ = lss == "" ? this_ : this_+"|"+ lss;
                }
               
            }
            foreach (var p in photo)
            {
                PersonnamePhoto.Text = p.Attribute("href").Value();
            }
            foreach (var p in personname)
            {
                Personnametext.Text = p.FindFirst("span").InnerText();
            }
            IEnumerable<IHtmlElement> ul_il = document_1.Find(".show-pic");
            IEnumerable<IHtmlElement> li = ul_il.Find("li");

            foreach (var img in li)
            {
                liImg.Add(img.FindFirst("img").Attribute("data-src").Value());
            }

            Bak.Text = this_;

            class1.photo = PersonnamePhoto.Text;
            class1.PersonName = Personnametext.Text;
            text_.Text = class1.TextName;
            this.Title = class1.TextName;
            MaxPage = liImg.Count;
            GoPage(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

             Page = Page + 1 > MaxPage-1 ? 0 : Page + 1;
            GoPage(Page);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             Page = Page - 1 < 0 ? MaxPage-1 : Page - 1;
            GoPage(Page);
        }
        #region 图片跳转
        private void GoPage(int p)
        {

            try
            {
                string url = @liImg[p];
                Page = p;
                Pagetext.Text = Page + 1 + "/" + MaxPage;

                Images.Source = new BitmapImage(new Uri(url));
            }
            catch
            {
               
            }
           
        }
        #endregion

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            #region 原始保持XML
            //int ALLcount = GetALLxmlCount();


            //class1.time = DateTime.Now;
            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"..\..\XMLFile2.xml");
            //XmlNode root = doc.SelectSingleNode("class1");
            //XmlElement xelKey = doc.CreateElement("Lists");
            //XmlAttribute xelType = doc.CreateAttribute("Type");
            //xelType.InnerText = class1.time.ToString();
            //xelKey.SetAttributeNode(xelType);

            //XmlAttribute xelType1 = doc.CreateAttribute("Counts");
            //xelType1.InnerText = ALLcount.ToString();
            //xelKey.SetAttributeNode(xelType1);


            //XmlElement xelAuthor = doc.CreateElement("Photo");
            //xelAuthor.InnerText = class1.photo;
            //xelKey.AppendChild(xelAuthor);

            //XmlElement xelAuthor1 = doc.CreateElement("Personname");
            //xelAuthor1.InnerText = class1.PersonName;
            //xelKey.AppendChild(xelAuthor1);

            //XmlElement xelAuthor2 = doc.CreateElement("Time");
            //xelAuthor2.InnerText = class1.time.ToString();
            //xelKey.AppendChild(xelAuthor2);

            //XmlElement xelAuthor3 = doc.CreateElement("Href");
            //xelAuthor3.InnerText = class1.href;
            //xelKey.AppendChild(xelAuthor3);



            //XmlElement xelAuthor4 = doc.CreateElement("del");
            //xelAuthor4.InnerText = "0";
            //xelKey.AppendChild(xelAuthor4);

            //XmlElement xelAuthor5 = doc.CreateElement("TextName");
            //xelAuthor5.InnerText = class1.TextName;
            //xelKey.AppendChild(xelAuthor5);

            //XmlElement xelAuthor6 = doc.CreateElement("http");
            //xelAuthor6.InnerText = class1.ID == "1" ? "58同城" : "赶集网";
            //xelKey.AppendChild(xelAuthor6);

            //XmlElement xelAuthor7 = doc.CreateElement("Addess");
            //xelAuthor7.InnerText = class1.Address;
            //xelKey.AppendChild(xelAuthor7);

            //XmlElement xelAuthor8 = doc.CreateElement("PM");
            //xelAuthor8.InnerText = class1.Allpm;
            //xelKey.AppendChild(xelAuthor8);

            //XmlElement xelAuthor9 = doc.CreateElement("Money");
            //xelAuthor9.InnerText = class1.SumMoney;
            //xelKey.AppendChild(xelAuthor9);

            //XmlElement xelAuthor10 = doc.CreateElement("Count");
            //xelAuthor10.InnerText = ALLcount.ToString();
            //xelKey.AppendChild(xelAuthor10);

            //root.AppendChild(xelKey);
            //doc.Save(@"..\..\XMLFile2.xml");
            //doc.Clone();

            #endregion
            if (saveDATA())
            { MessageBox.Show("添加完成！"); }
            else
            {
                MessageBox.Show("添加数据出错！或已存在该信息！");
            }
           
        }
        private bool saveDATA()
        {
            Boolean tempvalue = false; //定义返回值，并设置初值
                                       //下面把note中的数据添加到数据库中！
            try
            {               
                System.Data.DataSet mydataset; //定义DataSet
                OleDbConnection conn = SaveDate.getConn(); //getConn():得到连接对象
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                string sqlstr = "select * from mdb where Photo='" + class1.photo + "' and TextName='" + class1.TextName+"'";
                mydataset = new System.Data.DataSet();
                adapter.SelectCommand = new OleDbCommand(sqlstr, conn);
                adapter.Fill(mydataset, "notes");
                conn.Close();

                if (mydataset.Tables[0].Rows.Count>0)
                {
                    return tempvalue;
                }
                else
                {
                    #region MyRegion
                    OleDbConnection conn_ = SaveDate.getConn();
                    conn_.Open();
                    string qk = class1.ID == "1" ? "58同城" : "赶集网";
                    //设置SQL语句
                    string insertstr = "INSERT INTO mdb(Counts, Photo, Personname, Times, Href,del, TextName ,http,Addess,PM,Moneys,datetimes) VALUES ('";
                    insertstr += 0 + "', '";
                    insertstr += class1.photo + "','";
                    insertstr += class1.PersonName + "','";
                    insertstr += DateTime.Now.ToString("yyyy-MM-dd") + "','";
                    insertstr += class1.href + "','";
                    insertstr += 0 + "','";
                    insertstr += class1.TextName + "','";
                    insertstr += qk + "','";
                    insertstr += class1.Address + "','";
                    insertstr += class1.Allpm + "','";
                    insertstr += class1.SumMoney + "','";
                    insertstr += class1.datetime + "')";

                    OleDbCommand insertcmd = new OleDbCommand(insertstr, conn_);
                    insertcmd.ExecuteNonQuery();

                    conn_.Close();
                    tempvalue = true;
                    #endregion
                }
            }
            catch (Exception e)
            {
                throw (new Exception("数据库出错:" + e.Message));
            }
            return tempvalue;
        }
        private static int GetALLxmlCount()
        {
            int ALLcount;
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(@"..\..\XMLFile2.xml", settings);
            xmlDoc.Load(reader);
            // 得到根节点bookstore
            XmlNode xn = xmlDoc.SelectSingleNode("class1");
            if (xn == null)
            {
                ALLcount = 0;
            }
            else
            {
                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;
                ALLcount = xnl.Count;
            }
              
          
            reader.Close();
            return ALLcount;
        }

        private void Images_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            string url_str = Images.Source.ToString();
            string url = url_str.IndexOf('?')>0? url_str.Remove(url_str.IndexOf('?')):url_str;
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            //可能要获取的路径名
            string localFilePath = "", fileNameExt = "", newFileName = "", FilePath = "";

            //设置文件类型
            //书写规则例如：txt files(*.txt)|*.txt
            saveFileDialog.Filter = "JPEG Files(*.jpg) | *.jpg | BMP Files(*.bmp) | *.bmp|All files(*.*)|*.*";
            //设置默认文件名（可以不设置）
            saveFileDialog.FileName = class1.TextName.Replace(":",string.Empty) + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //主设置默认文件extension（可以不设置）
            saveFileDialog.DefaultExt = "txt";
            //获取或设置一个值，该值指示如果用户省略扩展名，文件对话框是否自动在文件名中添加扩展名。（可以不设置）
            saveFileDialog.AddExtension = true;

            //设置默认文件类型显示顺序（可以不设置）
            saveFileDialog.FilterIndex = 1;

            //保存对话框是否记忆上次打开的目录
            saveFileDialog.RestoreDirectory = true;

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();
            //点了保存按钮进入
            if (result == true)
            {
                //获得文件路径
                localFilePath = saveFileDialog.FileName.ToString();

                //获取文件名，不带路径
                fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);

                //获取文件路径，不带文件名
                FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));

                //给文件名前加上时间
                newFileName = fileNameExt + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName = FilePath + "\\" + newFileName;

                //在文件名里加字符
                //saveFileDialog.FileName.Insert(1,"dameng");
                //为用户使用 SaveFileDialog 选定的文件名创建读/写文件流。
                //System.IO.File.WriteAllText(newFileName, wholestring); 
                //这里的文件名其实是含有路径的。
            }

            if (localFilePath.Length>0)
            {
                WebClient webClient = new WebClient();

                webClient.DownloadFile(new Uri(url), localFilePath);
            }
           
            
        }
    }
}
