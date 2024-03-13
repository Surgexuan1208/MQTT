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
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging;
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
        Dictionary<string, List<string>> members = new Dictionary<string, List<string>>();
        static void AddEmployee(Dictionary<string, List<string>> companies, string code, string companyKey, string employee)
        {
            string key = $"{companyKey}";

            if (!companies.ContainsKey(key))
            {
                companies[key] = new List<string>();
            }

            companies[key].Add(employee);
        }
        static bool CheckExistence(Dictionary<string, List<string>> dictionary, string key, string value)
        {
            if (dictionary.ContainsKey(key))
            {
                if (dictionary[key].Contains(value))
                {
                    return true;
                }
            }
            return false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            if (txtname.Text.Length>0&&txtcid.Text.Length>0&&txtlevel.Text.Length>0&&txtphone.Text.Length>0&&txtbday.Text.Length>0&&txtfday.Text.Length>0&&txtaddress.Text.Length>0)
            {
                if (txtmid.Text.Length!=4)
                {
                    MessageBox.Show("員工代號必須為4碼");
                    return;
                }
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
                var foundItems1 = comID.Where(item => item.Contains(txtcid.Text)).ToList();
                if (!foundItems1.Any())
                {
                    MessageBox.Show($"公司ID {txtcid.Text} 不存在");
                    return;
                }
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();  //資料庫連線my'Unable to connect to any of the specified MySQL hosts.''Unable to connect to any of the specified MySQL hosts.'
                    // 在這裡執行資料庫操作
                    string sql = "SELECT * FROM member_db";
                    using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AddEmployee(members, reader.GetString("Card_ID"), reader.GetString("Company_ID"), reader.GetString("Card_ID"));
                            }
                        }
                    }
                    connection.Close();
                }

                if (CheckExistence(members,txtcid.Text,txtmid.Text))
                {
                    MessageBox.Show($"公司 {txtcid.Text} 中，員工ID {txtmid.Text} 已存在");
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
                    string insertSql = "INSERT INTO member_db (Company_ID, Card_ID, Name, Address, Cellphone, First_Day, Birth_Day, Effect) VALUES (@comid, @carid, @n, @a, @c, @f, @b, @e)";
                    using (MySqlCommand insertCommand = new MySqlCommand(insertSql, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@comid",txtcid.Text);
                        insertCommand.Parameters.AddWithValue("@carid",txtmid.Text);
                        insertCommand.Parameters.AddWithValue("@n", txtname.Text);
                        insertCommand.Parameters.AddWithValue("@a", txtaddress.Text);
                        insertCommand.Parameters.AddWithValue("@c", txtphone.Text);
                        insertCommand.Parameters.AddWithValue("@f", txtfday.Text);
                        insertCommand.Parameters.AddWithValue("@b", txtbday.Text);
                        insertCommand.Parameters.AddWithValue("@e", chken.IsChecked);
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
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (txtmid.IsEnabled)
            {
                txtmid.IsEnabled = false;
                editmid.Content = "開啟";
            }
            else { 
                txtmid.IsEnabled = true;
                editmid.Content = "關閉";
            }
            
        }
    }
}
