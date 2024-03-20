using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
    /// chgmember.xaml 的互動邏輯
    /// </summary>
    public partial class chgmember : Window
    {
        Member m;
        public string originID;
        public string originCom;
        public string originmID;
        public int mode;
        private MainWindow mainWindow;
        List<string> comID = new List<string>();
        List<string> carID = new List<string>();
        Dictionary<string, List<string>> members = new Dictionary<string, List<string>>();
        List<string> level = new List<string>();
        public chgmember(string s,MainWindow mainWindow)
        {
            InitializeComponent();
            m = Find(s);
            CreateCompany();
            Createlevel();
            txtcid.SelectedItem = m.Company_ID;
            txtaddress.Text = m.Address;
            txtbday.Text = m.BirthDay;
            txtcaid.Text = m.Card_ID;
            txtcid.Text = m.Company_ID;
            txtfday.Text = m.FirstDay;
            txtid.Text = m.ID;
            txtlevel.SelectedItem = m.Level;
            txtmid.Text = m.Member_ID;
            txtname.Text = m.Name;
            txtphone1.Text = m.Homephone;
            txtphone2.Text = m.Cellphone;
            chken.IsChecked = m.Effect;
            originID = m.Card_ID;
            originCom = m.Company_ID;
            originmID = m.Member_ID;
            mode = 0;
            this.mainWindow = mainWindow;
            this.WindowState = WindowState.Maximized;
        }
        private void Createlevel()
        {
            level.Add("未選擇");
            level.Add("老闆");
            level.Add("主管");
            level.Add("員工");
            txtlevel.ItemsSource = level;
        }
        private Member Find(string ID)
        {
            Member m = new Member("", "", "", "", "", "", "", "", "", "", "",false);
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
                string sql = $"SELECT * FROM member_db WHERE Card_ID = '{ID}'";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            m.Company_ID = reader.GetString("Company_ID");
                            m.Card_ID = reader.GetString("Card_ID");
                            m.Member_ID = reader.GetString("Member_ID");
                            m.Name = reader.GetString("Name");
                            m.Level = reader.GetString("Level");
                            m.ID = reader.GetString("ID");
                            m.Address = reader.GetString("Address");
                            m.Cellphone = reader.GetString("Cellphone");
                            m.Homephone = reader.GetString("Homephone"); 
                            m.FirstDay = reader.GetString("First_Day");
                            m.BirthDay = reader.GetString("Birth_Day");
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
                            carID.Add(reader.GetString("Card_ID"));
                        }
                    }
                }
                connection.Close();
            }
        }
        static void AddEmployee(Dictionary<string, List<string>> members, string code, string memberKey, string employee)
        {
            string key = $"{memberKey}";

            if (!members.ContainsKey(key))
            {
                members[key] = new List<string>();
            }

            members[key].Add(employee);
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
            if (txtmid.Text.Length != 4)
            {
                MessageBox.Show("員工代號必須為4碼");
                return;
            }
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            if (txtcaid.Text.Length > 0 && txtname.Text.Length > 0 && txtcid.SelectedIndex != 0 && txtlevel.SelectedIndex != 0 && txtbday.Text.Length > 0 && txtfday.Text.Length > 0 && txtaddress.Text.Length > 0)
            {
                if (originID != txtcaid.Text)
                {
                    mode = 1;
                    var foundItems = carID.Where(item => item.Contains(txtcaid.Text)).ToList();
                    if (foundItems.Any())
                    {
                        MessageBox.Show($"卡片ID {txtcaid.Text} 已存在");
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
            if(originCom != comID[txtcid.SelectedIndex] || originmID != txtmid.Text)
            {
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
                                AddEmployee(members, reader.GetString("Member_ID"), reader.GetString("Company_ID"), reader.GetString("Member_ID"));
                            }
                        }
                    }
                    connection.Close();
                }
                if (CheckExistence(members, comID[txtcid.SelectedIndex], txtmid.Text))
                {
                    MessageBox.Show($"公司 {comID[txtcid.SelectedIndex]} 中，員工ID {txtmid.Text} 已存在");
                    return;
                }
            }
            
            MessageBoxResult result = MessageBox.Show("確定要更改嗎?", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (mode == 0)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string updateSql = "UPDATE member_db SET Company_ID = @coi,Member_ID = @mi,Name = @n,Level = @l,ID = @i,Address = @a,Cellphone = @c,Homephone = @h,First_Day = @f,Birth_Day = @b,Effect = @e  WHERE Card_ID = @cai";
                        using (MySqlCommand updateCommand = new MySqlCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@coi", txtcid.Text);
                            updateCommand.Parameters.AddWithValue("@mi", txtmid.Text);
                            updateCommand.Parameters.AddWithValue("@n", txtname.Text);
                            updateCommand.Parameters.AddWithValue("@l", txtlevel.SelectedItem);
                            updateCommand.Parameters.AddWithValue("@i", txtid.Text);
                            updateCommand.Parameters.AddWithValue("@a", txtaddress.Text);
                            updateCommand.Parameters.AddWithValue("@c", txtphone2.Text);
                            updateCommand.Parameters.AddWithValue("@h", txtphone1.Text);
                            updateCommand.Parameters.AddWithValue("@f", txtfday.Text);
                            updateCommand.Parameters.AddWithValue("@b", txtbday.Text);
                            updateCommand.Parameters.AddWithValue("@e", chken.IsChecked);
                            updateCommand.Parameters.AddWithValue("@cai", txtcaid.Text);
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
                        string deleteSql = "DELETE FROM member_db WHERE Card_ID = @i";
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
                        string insertSql = "INSERT INTO member_db (Company_ID, Card_ID, Member_ID, Name, Level, ID, Address, Cellphone, Homephone, First_Day, Birth_Day, Effect) VALUES (@comid, @cardid, @m, @n, @l, @i, @a, @c, @h, @f, @b, @e)";
                        using (MySqlCommand insertCommand = new MySqlCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@comid", comID[txtcid.SelectedIndex]);
                            insertCommand.Parameters.AddWithValue("@cardid", txtcaid.Text);
                            insertCommand.Parameters.AddWithValue("@m", txtmid.Text);
                            insertCommand.Parameters.AddWithValue("@n", txtname.Text);
                            insertCommand.Parameters.AddWithValue("@l", level[txtlevel.SelectedIndex]);
                            insertCommand.Parameters.AddWithValue("@i", txtid.Text);
                            insertCommand.Parameters.AddWithValue("@a", txtaddress.Text);
                            insertCommand.Parameters.AddWithValue("@c", txtphone2.Text);
                            insertCommand.Parameters.AddWithValue("@h", txtphone1.Text);
                            insertCommand.Parameters.AddWithValue("@f", txtfday.Text);
                            insertCommand.Parameters.AddWithValue("@b", txtbday.Text);
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
