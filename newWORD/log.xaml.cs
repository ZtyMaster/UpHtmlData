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

namespace newWORD
{
    /// <summary>
    /// log.xaml 的交互逻辑
    /// </summary>
    public partial class log : Window
    {
        public int logs = 0;
        public log()
        {
            InitializeComponent();
           
            buttons.Visibility = Visibility.Collapsed;
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            logs++;
            if (logs>=10)
            {
                zztext.IsReadOnly = false;
                phototext.IsReadOnly = false;
                qqtext.IsReadOnly = false;
                zztext.BorderThickness = new Thickness(1);
                phototext.BorderThickness = new Thickness(1);
                qqtext.BorderThickness = new Thickness(1);
                buttons.Visibility = Visibility.Visible;
            }
            logs = logs > 10 ? 0 : logs;
            
        }

        private void buttons_Click(object sender, RoutedEventArgs e)
        {
            #region 更新表单
            Boolean tempvalue = false;
            string sqlstr = ""; //当时在这里定义，是为了在出现异常的时候看看我的SQL语句是否正确
            try
            {
                //用到了我前面写的那个得到数据库连接的函数
                OleDbConnection conn = SaveDate.getConn(); //getConn():得到连接对象，
                conn.Open();

                //确定我们需要执行的SQL语句，本处是UPDATE语句！
                sqlstr = "UPDATE Author SET ";
                sqlstr += "Author='" + zztext.Text.Trim() + "',";
                sqlstr += "Photo='" + phototext.Text.Trim() + "',";
                sqlstr += "QQ='" + qqtext.Text.Trim() + "'";
                sqlstr += " where id=4";

                //定义command对象，并执行相应的SQL语句
                OleDbCommand myCommand = new OleDbCommand(sqlstr, conn);
                myCommand.ExecuteNonQuery(); //执行SELECT的时候我们是用的ExecuteReader()
                conn.Close();


                //假如执行成功，则，返回TRUE，否则，返回FALSE
                tempvalue = true;
            }
            catch (Exception es)
            {
                MessageBox.Show("数据库更新出错:" + sqlstr + "\r" + es.Message);
            }
            #endregion
            if (tempvalue)
            {
                zztext.IsReadOnly = true;
                phototext.IsReadOnly = true;
                qqtext.IsReadOnly = true;
                zztext.BorderThickness = new Thickness(0);
                phototext.BorderThickness = new Thickness(0);
                qqtext.BorderThickness = new Thickness(0);
                buttons.Visibility = Visibility.Collapsed;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataView dataview;
            System.Data.DataSet mydataset; //定义DataSet
            try
            {
                OleDbConnection conn = SaveDate.getConn(); //getConn():得到连接对象
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                string sqlstr =  "select * from Author where ID=4";
                mydataset = new System.Data.DataSet();
                adapter.SelectCommand = new OleDbCommand(sqlstr, conn);
                adapter.Fill(mydataset, "notes");
                conn.Close();
            }
            catch (Exception es)
            {
                throw (new Exception("数据库出错:" + es.Message));
            }
            dataview = new DataView(mydataset.Tables["notes"]);
            zztext.Text = dataview.Table.Rows[0]["Author"].ToString();
            phototext.Text = dataview.Table.Rows[0]["Photo"].ToString();
            qqtext.Text = dataview.Table.Rows[0]["QQ"].ToString();
        }
    }
}
