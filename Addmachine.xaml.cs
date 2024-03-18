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
using MySql.Data.MySqlClient;

namespace MQTT
{
    /// <summary>
    /// Addmachine.xaml 的互動邏輯
    /// </summary>
    public partial class Addmachine : Window
    {
        private MainWindow mainWindow;
        public Addmachine(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.WindowState = WindowState.Maximized;
            Find();
        }
        List<string> comID = new List<string>();
        List<string> macID = new List<string>();
        private void Find()
        {
            comID.Clear();
            comID.Add("未選擇");
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();  //資料庫連線my'Unable to connect to any of the specified MySQL hosts.''Unable to connect to any of the specified MySQL hosts.'
                // 在這裡執行資料庫操作
                string sql = "SELECT * FROM company_info_db";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comID.Add(reader.GetString("ID"));
                        }
                    }
                }
                connection.Close();
            }
            txtcid.ItemsSource = comID;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();  //資料庫連線my'Unable to connect to any of the specified MySQL hosts.''Unable to connect to any of the specified MySQL hosts.'
                                    // 在這裡執行資料庫操作
                string sql = "SELECT * FROM machine_db";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            macID.Add(reader.GetString("Machine_ID"));
                        }
                    }
                }
                connection.Close();
            }
            var foundItems = macID.Where(item => item.Contains(txtmid.Text)).ToList();
            if (foundItems.Any())
            {
                MessageBox.Show("ID已存在");
                return;
            }
            if (txtcid.SelectedIndex == 0 && txtmid.Text.Length > 0 && txtdes.Text.Length > 0)
            {
                MessageBox.Show("資料不能未選擇或為空");
                return;
            }
            MessageBoxResult result = MessageBox.Show("確定要上傳嗎?", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string insertSql =
                        "INSERT INTO machine_db (Company_ID, Machine_ID, Machine_Location, Status, Effect) VALUES (@cid, @mid, @ml, @st,@effect)";
                    using (MySqlCommand insertCommand = new MySqlCommand(insertSql, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@cid", txtcid.Text);
                        insertCommand.Parameters.AddWithValue("@mid", txtmid.Text);
                        insertCommand.Parameters.AddWithValue("@ml", txtdes.Text);
                        insertCommand.Parameters.AddWithValue("@st", 0);
                        insertCommand.Parameters.AddWithValue("@effect", chken.IsChecked);
                        int rowsAffected = insertCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                this.Close();
                mainWindow.MySQLCreatelist();
            }
            else
            {
                return;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("確定要放棄嗎?", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }
    }
}
