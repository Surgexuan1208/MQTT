using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Addmember.xaml 的互動邏輯
    /// </summary>
    public partial class Addmember : Window
    {
        public Addmember()
        {
            InitializeComponent();
        }
        List<string> comID = new List<string>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            if (true)
            {
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
                var foundItems = comID.Where(item => item.Contains("1")).ToList();
                if (!foundItems.Any())
                {
                    MessageBox.Show("ID不存在");
                    return;
                }
            }
            else
            {
                MessageBox.Show("資料不能有空");
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
                        insertCommand.Parameters.AddWithValue("@cid","1");
                        insertCommand.Parameters.AddWithValue("@mid","1");
                        insertCommand.Parameters.AddWithValue("@ml", "1");
                        insertCommand.Parameters.AddWithValue("@st", 0);
                        insertCommand.Parameters.AddWithValue("@effect","1");
                        int rowsAffected = insertCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                this.Close();
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
