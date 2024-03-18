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
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace MQTT
{
    /// <summary>
    /// chgmachine.xaml 的互動邏輯
    /// </summary>
    public partial class chgmachine : Window
    {
        Machine m;
        public string originID;
        public int mode;
        private MainWindow mainWindow;
        List<string> comID = new List<string>();
        List<string> MachID = new List<string>();
        public chgmachine(String s, MainWindow mainWindow)
        {
            InitializeComponent();
            m = Find(s);
            CreateCompany();
            txtcid.SelectedItem = m.Company_ID;
            txtdes.Text= m.Machine_Location;
            txtmid.Text = m.Machine_ID;
            chken.IsChecked = m.Effect;
            originID = m.Machine_ID;
            mode = 0;
            this.mainWindow = mainWindow;
            this.WindowState = WindowState.Maximized;
        }

        private Machine Find(string ID)
        {
            Machine m = new Machine("", "", "", "",false);
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
                string sql = $"SELECT * FROM machine_db WHERE Machine_ID = '{ID}'";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            m.Company_ID = reader.GetString("Company_ID");
                            m.Machine_ID = reader.GetString("Machine_ID");
                            m.Machine_Location = reader.GetString("Machine_Location");
                            m.Effect = reader.GetBoolean("Effect");
                        }
                    }
                }
                connection.Close();
            }
            return m;
        }
        private void CreateCompany()
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
            if (txtdes.Text.Length > 0 && txtmid.Text.Length > 0 && txtcid.SelectedIndex != 0)
            {
                if (originID != txtmid.Text)
                {
                    mode = 1;
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
                                    MachID.Add(reader.GetString("Machine_ID"));
                                }
                            }
                        }
                        connection.Close();
                    }
                    var foundItems = MachID.Where(item => item.Contains(txtmid.Text)).ToList();
                    if (foundItems.Any())
                    {
                        MessageBox.Show("ID不能重複");
                        return;
                    }
                }
                else
                {
                    mode = 0;
                }
            }
            else
            {
                MessageBox.Show("資料不能有空");
                return;
            }
            MessageBoxResult result = MessageBox.Show("確定要更改嗎?", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (mode == 0)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string updateSql = "UPDATE machine_db SET Company_ID = @c,Machine_Location = @ml,Status = 0,Effect = @e WHERE Machine_ID = @mi";
                        using (MySqlCommand updateCommand = new MySqlCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@c", txtcid.SelectedItem);
                            updateCommand.Parameters.AddWithValue("@ml", txtdes.Text);
                            updateCommand.Parameters.AddWithValue("@e", chken.IsChecked);
                            updateCommand.Parameters.AddWithValue("@mi", txtmid.Text);
                            int rowsAffected = updateCommand.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                else
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string deleteSql = "DELETE FROM machine_db WHERE Machine_ID = @i";
                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@i", originID);
                            int rowsAffected = deleteCommand.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string insertSql = "INSERT INTO machine_db (Company_ID, Machine_ID, Machine_Location, Status, Effect) VALUES (@c, @mi, @ml ,@s,@e)";
                        using (MySqlCommand insertCommand = new MySqlCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@c", txtcid.SelectedItem);
                            insertCommand.Parameters.AddWithValue("@mi", txtmid.Text);
                            insertCommand.Parameters.AddWithValue("@ml", txtdes.Text);
                            insertCommand.Parameters.AddWithValue("@s", 0);
                            insertCommand.Parameters.AddWithValue("@e", chken.IsChecked);
                            int rowsAffected = insertCommand.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                mainWindow.MySQLCreatelist();
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
