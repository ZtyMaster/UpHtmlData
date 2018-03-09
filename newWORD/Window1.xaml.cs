using System;
using System.Collections.Generic;
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
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            List<URL_list> lul = new List<URL_list>();
            MainWindow.GetURL_lst(lul);
            comboBox.ItemsSource = lul;
            comboBox.DisplayMemberPath = "Name";
            comboBox.SelectedIndex = 0;
            textBox.Text = lul[0].Href;
        }
       public URL_list url_ { get; set; }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object obj = (object)e.AddedItems;
                textBox.Text  = ((newWORD.URL_list)((object[])obj)[0]).Href;
                url_ = ((newWORD.URL_list)((object[])obj)[0]);

            }
            catch (Exception)
            {

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(@"..\..\XMLFile1.xml", settings);
           
            xmlDoc.Load(reader);
            reader.Close();
            url_.Href = textBox.Text.Trim();
            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format("/bookstore/book[@ID=\"{0}\"]",url_.ID);
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);  //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            selectXe.SetAttribute("Type", url_.Name);//也可以通过SetAttribute来增加一个属性
            selectXe.GetElementsByTagName("title").Item(0).InnerText = url_.Href;
            xmlDoc.Save(@"..\..\XMLFile1.xml");
            xmlDoc.Clone();
            MessageBox.Show("完成修改");
            
        }
    }
}
