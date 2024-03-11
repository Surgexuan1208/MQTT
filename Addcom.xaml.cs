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
using System.Windows.Shapes;

namespace MQTT
{
    /// <summary>
    /// Addcom.xaml 的互動邏輯
    /// </summary>
    public partial class Addcom : Window
    {
        public Addcom()
        {
            InitializeComponent();
        }
        public string GetData()
        {
            // 這裡可以取得輸入資料，並將其回傳給呼叫它的程式
            return "0";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show();
        }
    }
}
