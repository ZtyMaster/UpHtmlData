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

namespace newWORD
{
    /// <summary>
    /// zc.xaml 的交互逻辑
    /// </summary>
    public partial class zc : Window
    {
        RegisterClass rrc = new RegisterClass();
        public zc()
        {
            InitializeComponent();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (rrc.BoolRegist(rrc.CreateCode()))
            {
                this.Visibility=Visibility.Collapsed;
            }
            else
            {
                jiqima.Text = rrc.CreateCode();

            }
           
        }

        private void NewMethod()
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
             rrc.GetCodeMD5(jiqima.Text);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            if (rrc.RegistIt(jihuoma.Text, jiqima.Text))
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("注册失败！");
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
