﻿using System;
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
    /// chgcom.xaml 的互動邏輯
    /// </summary>
    public partial class chgcom : Window
    {
        Company c;
        public string originID;
        public int mode;
        private MainWindow mainWindow;
        public chgcom(String s, MainWindow mainWindow)
        {
            InitializeComponent();
            c = Find(s);
            txtaddress.Text = c.Address;
            txtid.Text = c.Company_ID;
            txtname.Text = c.Name;
            txtphone.Text = c.Cellphone;
            originID = c.Company_ID;
            mode = 0;
            this.mainWindow = mainWindow;
            this.WindowState = WindowState.Maximized;
        }
        List<string> comID = new List<string>();
        private Company Find(string ID)
        {
            Company company = new Company("","","","");
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
                string sql = $"SELECT * FROM company_info_db WHERE Company_ID = '{ID}'";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using(MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            company.Company_ID = reader.GetString("Company_ID");
                            company.Address = reader.GetString("Address");
                            company.Name = reader.GetString("Name");
                            company.Cellphone = reader.GetString("Cellphone");
                        }
                    }
                }
                connection.Close();
            }
            return company;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            if (txtid.Text.Length > 0 && txtname.Text.Length > 0 && txtaddress.Text.Length > 0 && txtphone.Text.Length > 0)
            {
                if(originID != txtid.Text)
                {
                    mode = 1;
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
                                    comID.Add(reader.GetString("Company_ID"));
                                }
                            }
                        }
                        connection.Close();
                    }
                    var foundItems = comID.Where(item => item.Contains(txtid.Text)).ToList();
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
                        string updateSql = "UPDATE company_info_db SET Name = @n,Address = @a, Cellphone = @c WHERE Company_ID = @i";
                        using (MySqlCommand updateCommand = new MySqlCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@n", txtname.Text);
                            updateCommand.Parameters.AddWithValue("@a", txtaddress.Text);
                            updateCommand.Parameters.AddWithValue("@c", txtphone.Text);
                            updateCommand.Parameters.AddWithValue("@i", txtid.Text);
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
                        string deleteSql = "DELETE FROM company_info_db WHERE Company_ID = @i";
                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@i",originID);
                            int rowsAffected = deleteCommand.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string insertSql = "INSERT INTO company_info_db (Company_ID, Name, Address, Cellphone) VALUES (@i, @n, @a ,@c)";
                        using (MySqlCommand insertCommand = new MySqlCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@i", txtid.Text);
                            insertCommand.Parameters.AddWithValue("@n", txtname.Text);
                            insertCommand.Parameters.AddWithValue("@a", txtaddress.Text);
                            insertCommand.Parameters.AddWithValue("@c", txtphone.Text);
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
