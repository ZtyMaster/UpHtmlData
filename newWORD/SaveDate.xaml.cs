using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace newWORD
{
    /// <summary>
    /// SaveDate.xaml 的交互逻辑
    /// </summary>
    public partial class SaveDate : Window
    {
        public int page { get; set; }
        public int allcount { get; set; }
        XmlDocument xmlDoc = new XmlDocument();
        public SaveDate()
        {
            InitializeComponent();
            page = 1;
            allcount = 0;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GoPage();
        }
       
        private DataView GoPage_Access(bool bol,string str)
        {

            DataView dataview;
            System.Data.DataSet mydataset; //定义DataSet
            try
            {
                OleDbConnection conn = getConn(); //getConn():得到连接对象
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                string sqlstr =bol? "select * from mdb order by ID desc": "select * from mdb where InStr(1,LCase(TextName),LCase('"+str+"'),0)<>0   order by ID desc";
                mydataset = new System.Data.DataSet();
                adapter.SelectCommand = new OleDbCommand(sqlstr, conn);
                adapter.Fill(mydataset, "notes");
                conn.Close();
            }
            catch (Exception e)
            {
                throw (new Exception("数据库出错:" + e.Message));
            }
            dataview = new DataView(mydataset.Tables["notes"]);
            return dataview;

            #region MyRegion
            //Class1 tempnote = new Class1(); //定义返回值

            //try
            //{
            //    OleDbConnection conn = getConn(); //getConn():得到连接对象
            //    string strCom = "Select * from mdb ";
            //    OleDbCommand myCommand = new OleDbCommand(strCom, conn);
            //    conn.Open();
            //    OleDbDataReader reader;
            //    reader = myCommand.ExecuteReader(); //执行command并得到相应的DataReader
            //                                        //下面把得到的值赋给tempnote对象
            //    if (reader.Read())
            //    {
            //        tempnote.ID = reader["ID"].ToString();
            //        tempnote.Id_count = reader["Counts"].ToString();
            //        tempnote.PersonName = reader["Personname"].ToString();
            //        tempnote.time = Convert.ToDateTime(reader["Time"]);
            //        tempnote.href = reader["Href"].ToString();
            //        tempnote.TextName = reader["TextName"].ToString();
            //        tempnote.Laiyuan = reader["http"].ToString();
            //        tempnote.Address = reader["Addess"].ToString();
            //        tempnote.Allpm = reader["PM"].ToString();
            //        tempnote.SumMoney = reader["Money"].ToString();
            //    }
            //    else //如没有该记录，则抛出一个错误！
            //    {
            //        throw (new Exception("当前没有该记录！"));
            //    }

            //    reader.Close();
            //    conn.Close();
            //}
            //catch (Exception e)
            //{
            //    //throw(new Exception("数据库出错:" + e.Message)) ;
            //}
            #endregion
        }

        private void GoPage()
        {
            int  Lp = 30;
            int stp = (page - 1) * Lp, sto = (Lp * page) - 1;
          
            Page_text.Text = page.ToString();
            bool b = SStext.Text.Trim().Length > 0 ? false : true;
            string str =SStext.Text.Trim().Length>0? SStext.Text.Trim(): string.Empty;

            DataView dvss = GoPage_Access(b,str);
           
            allcount = (int)Math.Ceiling(Convert.ToDouble(dvss.Table.Rows.Count) /Convert.ToDouble(Lp));

            Page_TEXT.Text = page.ToString() + "/" +allcount.ToString();
            Dg_info.ItemsSource = GetPagedTable(dvss.Table, page, Lp).AsDataView();

            #region 老版本xml
            ////if (xmlDoc.SelectSingleNode("class1") == null)
            ////    return;
            ////// 得到根节点bookstore
            ////XmlNode xn = xmlDoc.SelectSingleNode("class1");

            ////// 得到根节点的所有子节点
            ////XmlNodeList xnl = xn.ChildNodes;

            ////allcount = (xnl.Count/Lp)+1;
            ////page_text.Text = page + "/" + allcount;
            ////List<Class1> lc = new List<Class1>();

            ////for (int i = stp; i <= sto; i++)
            ////{
            ////    XmlNode xn1 = xnl[i];
            ////    if (xn1 == null)
            ////    {
            ////        continue;
            ////    }
            ////    #region MyRegion

            ////    Class1 cs = new Class1();
            ////    // 将节点转换为元素，便于得到节点的属性值
            ////    XmlElement xe = (XmlElement)xn1;
            ////    // 得到Type和ISBN两个属性的属性值
            ////    cs.datetime = Convert.ToDateTime(xe.GetAttribute("Type").ToString()).ToString("yyyy-MM-dd");

            ////    cs.Id_count = xe.GetAttribute("Counts").ToString();
            ////    // 得到Book节点的所有子节点
            ////    XmlNodeList xnl0 = xe.ChildNodes;
            ////    cs.photo = xnl0.Item(0).InnerText;
            ////    cs.PersonName = xnl0.Item(1).InnerText;
            ////    cs.href = xnl0.Item(3).InnerText;
            ////    cs.TextName = xnl0.Item(5).InnerText;
            ////    cs.ID = xnl0.Item(6).InnerText;
            ////    cs.Address = xnl0.Item(7).InnerText;
            ////    cs.Allpm = xnl0.Item(8).InnerText;
            ////    cs.SumMoney = xnl0.Item(9).InnerText;               

            ////    lc.Add(cs);
            ////    #endregion
            ////}


            //Dg_info.ItemsSource = lc;
            //xmlDoc.Clone();
            #endregion
        }
        //PageIndex表示第几页，PageSize表示每页的记录数
        public DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
                return dt;//0页代表每页数据，直接返回

            DataTable newdt = dt.Copy();
            newdt.Clear();//copy dt的框架

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;//源数据记录数小于等于要显示的记录，直接返回dt

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
        private void Dg_info_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView ds = (DataRowView)Dg_info.SelectedItem;
            if (ds.Row["href"].ToString().Length > 0)
            {
                System.Diagnostics.Process.Start(ds.Row["href"].ToString());
            }           
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            page = 1;
            GoPage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            page = page + 1 > allcount ? allcount : page + 1;
           
            GoPage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            page = page - 1 <= 0 ? 1 : page - 1;
            GoPage();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try {
                page=Convert.ToInt32( Page_text.Text.Trim());
                if (page > 0 && page <= allcount)
                {
                    GoPage();
                }
                else
                {
                    MessageBox.Show("非法页数！");
                }
                
            }
            catch
            {
                MessageBox.Show("页数格式错误！");
            }
            
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            page = allcount;
            GoPage();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DataRowView s =(DataRowView)  Dg_info.SelectedItem;
            object obj = MessageBox.Show("是否删除序号为" + s.Row["ID"] + "的信息", "删除信息", MessageBoxButton.YesNo);
            if(obj.ToString() =="Yes")
            {
                #region 老方法xml
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(@"..\..\XMLFile2.xml");
                //XmlNode root = xmlDoc.SelectSingleNode("class1");//查找<bookstore>  
                //foreach (XmlNode xn in root)
                //{
                //    XmlElement xe = (XmlElement)xn;
                //    if (xe.GetAttribute("Counts") == s.Id_count)
                //    {
                //        xe.RemoveAll();//删除该节点的全部内容  

                //    }
                //    else if (xe.GetAttribute("genre") == "update李赞红")
                //    {
                //        xe.RemoveAttribute("genre");//删除genre属性  
                //    }
                //}
                //xmlDoc.Save(@"..\..\XMLFile2.xml");
                //root.Clone();
                #endregion
               
                string sqlstr = "";
                //连接数据库
                try
                {
                    OleDbConnection conn = getConn(); //getConn():得到连接对象
                    conn.Open();

                    sqlstr = "delete * from mdb where id=" + s.Row["ID"];

                    //定义command对象，并执行相应的SQL语句
                    OleDbCommand myCommand = new OleDbCommand(sqlstr, conn);
                    myCommand.ExecuteNonQuery();
                    conn.Close();


                    //假如执行成功，则，返回TRUE，否则，返回FALSE
                   
                    GoPage();
                }
                catch (Exception es)
                {
                    MessageBox.Show("数据库更新出错:" + sqlstr + "\r" + es.Message);
                   
                }
            }
           
        }

        public static OleDbConnection getConn()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0 ;Data Source="+path+ "\\Fdata\\Database1.mdb";
            OleDbConnection tempconn = new OleDbConnection(connstr);
            return (tempconn);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SStext.Text.Trim().Length > 0)
            {
               page = 1;
                GoPage();
            }
            
        }
    }
}
