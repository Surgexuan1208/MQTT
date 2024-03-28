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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mqtt;
using System.ServiceModel.Channels;
using System.Net.Mqtt.Sdk;
using System.Threading;
using System.Reflection.PortableExecutable;
using MySql.Data.MySqlClient;
using System.Windows.Media.Effects;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Collections;
using System.Data;

namespace MQTT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetDataGridTextSize(comdatagrid, 40);
            SetDataGridTextSize(machdatagrid, 40);
            SetDataGridTextSize(memdatagrid, 40);
            this.WindowState = WindowState.Maximized;
            MySQLCreatelist();
        }
        Dictionary<string, string> ComcolumnMapping = new Dictionary<string, string>()
        {
            { "Company_ID", "部門編號" },
            { "Name", "部門名稱" },
            { "Address", "部門地址" },
            { "Cellphone", "部門電話" }
        };
        Dictionary<string, string> MachcolumnMapping = new Dictionary<string, string>()
        {
            { "Company_ID", "部門編號" },
            { "Machine_ID", "機器編號" },
            { "Machine_Location", "機器位置" },
            { "Status", "機器狀態" },
            { "Effect", "是否啟用" }
        };
        Dictionary<string, string> MemcolumnMapping = new Dictionary<string, string>()
        {
            { "Company_ID", "部門編號" },
            { "Card_ID", "卡片編號" },
            { "Member_ID", "員工編號" },
            { "Name", "姓名" },
            { "Level", "職位" },
            { "ID", "身分證號" },
            { "Address", "住家地址" },
            { "Cellphone", "手機號碼" },
            { "Homephone", "室內電話" },
            { "FirstDay", "入值日" },
            { "BirthDay", "生日" },
            { "Effect", "是否在職" }
        };
        List<Member> members = new List<Member>();
        List<Machine> machines = new List<Machine>();
        List<Company> companies = new List<Company>();
        List<String> companiesID = new List<String>();
        public void MySQLCreatelist()
        {
            machines.Clear();
            members.Clear();
            companies.Clear();
            companiesID.Clear();
            companiesID.Add("未選擇");
            comdatagrid.ItemsSource = null;
            machdatagrid.ItemsSource= null;
            memdatagrid.ItemsSource= null;
            txtcid1.ItemsSource = null;
            txtcid2.ItemsSource = null;
            txtcid3.ItemsSource = null;
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
                string sql = "SELECT * FROM member_db";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            members.Add(new Member(reader.GetString("Company_ID"), reader.GetString("Card_ID"), reader.GetString("Member_ID"), reader.GetString("Name"), reader.GetString("Level"), reader.GetString("ID") , reader.GetString("Address"), reader.GetString("Cellphone"), reader.GetString("Homephone"), reader.GetString("First_Day"), reader.GetString("Birth_Day"), reader.GetBoolean("Effect")));
                        }
                    }
                }
                connection.Close();
            }
            memdatagrid.ItemsSource = members;
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
                            companies.Add(new Company(reader.GetString("Company_ID"), reader.GetString("Name"), reader.GetString("Address"), reader.GetString("Cellphone")));
                            companiesID.Add(companies[companies.Count-1].Company_ID);
                        }
                    }
                }
                connection.Close();
            }
            comdatagrid.ItemsSource = companies;
            txtcid1.ItemsSource=companiesID;
            txtcid2.ItemsSource=companiesID;
            txtcid3.ItemsSource=companiesID;
            txtcid1.SelectedIndex = 0;
            txtcid2.SelectedIndex = 0;
            txtcid3.SelectedIndex = 0;
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
                            machines.Add(new Machine(reader.GetString("Company_ID"), reader.GetString("Machine_ID"), reader.GetString("Machine_Location"), reader.GetString("Status"), reader.GetBoolean("Effect")));
                        }
                    }
                }
                connection.Close();
            }
            machdatagrid.ItemsSource = machines;
            comdatagrid.AutoGeneratingColumn += (sender, e) =>
            {
               if (ComcolumnMapping.ContainsKey(e.PropertyName))
               {
                   e.Column.Header = ComcolumnMapping[e.PropertyName];
               }
            };
            machdatagrid.AutoGeneratingColumn += (sender, e) =>
            {
                if (MachcolumnMapping.ContainsKey(e.PropertyName))
                {
                    e.Column.Header = MachcolumnMapping[e.PropertyName];
                }
            };
            memdatagrid.AutoGeneratingColumn += (sender, e) =>
            {
                if (MemcolumnMapping.ContainsKey(e.PropertyName))
                {
                    e.Column.Header = MemcolumnMapping[e.PropertyName];
                }
            };
            comdatagrid.AutoGeneratingColumn += (sender, e) =>
            {
                // 设置 CanUserSort 属性为 false
                e.Column.CanUserSort = false;
                e.Column.CanUserResize = false;
                e.Column.CanUserReorder = false;
            };
            machdatagrid.AutoGeneratingColumn += (sender, e) =>
            {
                // 设置 CanUserSort 属性为 false
                e.Column.CanUserSort = false;
                e.Column.CanUserResize = false;
                e.Column.CanUserReorder = false;
            };
            memdatagrid.AutoGeneratingColumn += (sender, e) =>
            {
                // 设置 CanUserSort 属性为 false
                e.Column.CanUserSort = false;
                e.Column.CanUserResize = false;
                e.Column.CanUserReorder = false;
            };

        }
        private void SetDataGridTextSize(DataGrid dataGrid, double fontSize)
        {
            Style cellStyle = new Style(typeof(DataGridCell));
            cellStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, fontSize));
            dataGrid.CellStyle = cellStyle;
        }
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Topic.Text.Length <1 ||Topic.Text.Length==0)
            {
                MessageBox.Show("主題不能為空");
                return;
            }
            try
            {
                // 建立 MQTT 客戶端配置
                var configuration = new MqttConfiguration
                {
                    BufferSize = 65536,
                    Port = int.Parse(port.Text),
                    KeepAliveSecs = 10,
                    WaitTimeoutSecs = 2,
                    MaximumQualityOfService = MqttQualityOfService.AtMostOnce,
                    AllowWildcardsInTopicFilters = true
                };
                // 建立 MQTT 客戶端
                var client = await MqttClient.CreateAsync((string)ip.Text,configuration);
                MessageBox.Show("已建立 MQTT 客戶端！");
                // 連接到 MQTT 伺服器
                var sessionState = await client.ConnectAsync(new MqttClientCredentials(clientId: ClientID.Text), cleanSession: true);
                MessageBox.Show("已連接到 MQTT 伺服器！");
                // 在這裡可以加入訂閱主題或發佈訊息等操作
                var message1 = new MqttApplicationMessage(Topic.Text, Encoding.UTF8.GetBytes(msg.Text));
                await client.PublishAsync(message1, MqttQualityOfService.AtMostOnce); //QoS0
                // 斷開 MQTT 連接
                await client.DisconnectAsync();
                MessageBox.Show("已斷開 MQTT 連接！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"錯誤：{ex.Message}");
            }
        }
        // 斷開連接
        private async void StopButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
            stopbtn.IsEnabled = false;
            var configuration = new MqttConfiguration
            {
                BufferSize = 65536,
                Port = int.Parse(port2.Text),
                KeepAliveSecs = 10,
                WaitTimeoutSecs = 2,
                MaximumQualityOfService = MqttQualityOfService.AtMostOnce,
                AllowWildcardsInTopicFilters = true
            };
            // 建立 MQTT 客戶端
            var client = await MqttClient.CreateAsync((string)ip2.Text, configuration);
            // 連接到 MQTT 伺服器
            var sessionState = await client.ConnectAsync(new MqttClientCredentials(clientId: "debugger"), cleanSession: true);
            var message1 = new MqttApplicationMessage(Topic2.Text, Encoding.UTF8.GetBytes("@Stop"));
            await client.PublishAsync(message1, MqttQualityOfService.AtMostOnce); //QoS0
            startbtn.IsEnabled = true;
            ip2.IsEnabled = true; port2.IsEnabled = true; ClientID2.IsEnabled = true; Topic2.IsEnabled = true;

        }
        private void addbtn_Click1(object sender, RoutedEventArgs e)
        {
            Addcom newWindow = new Addcom(this);
            newWindow.Show();
        }

        private void chgbtn_Click1(object sender, RoutedEventArgs e)
        {
            if (comdatagrid.SelectedIndex >= 0 && comdatagrid.SelectedIndex < companies.Count)
            {
                chgcom newWindow = new chgcom(companies[comdatagrid.SelectedIndex].Company_ID,this);
                newWindow.Show();
            }else{
                // 在這裡處理索引無效的情況，例如顯示錯誤訊息或採取其他措施
                MessageBox.Show("請選擇清單內的項目", "未選取");
            }
        }

        private void srhbtn_Click1(object sender, RoutedEventArgs e)
        {
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            comdatagrid.ItemsSource = null;
            companies.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();  //資料庫連線my'Unable to connect to any of the specified MySQL hosts.''Unable to connect to any of the specified MySQL hosts.'
                                    // 在這裡執行資料庫操作
                string sql = $"SELECT * FROM company_info_db WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(txtname.Text))
                    sql += $" AND Name LIKE N'%{txtname.Text}%'";

                if (!(txtcid1.SelectedIndex == 0))
                    sql += $" AND Company_ID = '{companiesID[txtcid1.SelectedIndex]}'";

                if (!string.IsNullOrWhiteSpace(txtphone.Text))
                    sql += $" AND Cellphone LIKE '%{txtphone.Text}%'";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            companies.Add(new Company(reader.GetString("Company_ID"), reader.GetString("Name"), reader.GetString("Address"), reader.GetString("Cellphone")));
                        }
                    }
                }
                connection.Close();
            }
            comdatagrid.ItemsSource = companies;
        }

        private void delbtn_Click1(object sender, RoutedEventArgs e)
        {
            if (comdatagrid.SelectedIndex >= 0 && comdatagrid.SelectedIndex < companies.Count)
            {

            }else{                // 在這裡處理索引無效的情況，例如顯示錯誤訊息或採取其他措施
                MessageBox.Show("請選擇清單內的項目", "未選取");
                return;
            }
            MessageBoxResult result = MessageBox.Show("確定要刪除嗎?(接下來請再確認三次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                result = MessageBox.Show("確定要刪除嗎?(1次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    result = MessageBox.Show("確定要刪除嗎?(2次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        result = MessageBox.Show("確定要刪除嗎?(3次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            string database = "company_db";
                            string databaseServer = "220.132.141.9";
                            string databasePort = "6833";
                            string databaseUser = "root";
                            string databasePassword = "edys1234";
                            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
                            using (MySqlConnection connection = new MySqlConnection(connectionString))
                            {
                                connection.Open();
                                string deleteSql = "DELETE FROM company_info_db WHERE Company_ID = @i";
                                using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, connection))
                                {
                                    deleteCommand.Parameters.AddWithValue("@i", companies[comdatagrid.SelectedIndex].Company_ID);
                                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                                }
                            }
                            MySQLCreatelist();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void addbtn_Click2(object sender, RoutedEventArgs e)
        {
            Addmachine newWindow = new Addmachine(this);
            newWindow.Show();
        }

        private void chgbtn_Click2(object sender, RoutedEventArgs e)
        {
            if (machdatagrid.SelectedIndex >= 0 && machdatagrid.SelectedIndex < machines.Count)
            {
                chgmachine newWindow = new chgmachine(machines[machdatagrid.SelectedIndex].Machine_ID, this);
                newWindow.Show();
            }
            else
            {
                // 在這裡處理索引無效的情況，例如顯示錯誤訊息或採取其他措施
                MessageBox.Show("請選擇清單內的項目","未選取");
            }
            
        }

        private void srhbtn_Click2(object sender, RoutedEventArgs e)
        {
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            machdatagrid.ItemsSource = null;
            machines.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();  //資料庫連線my'Unable to connect to any of the specified MySQL hosts.''Unable to connect to any of the specified MySQL hosts.'
                                    // 在這裡執行資料庫操作
                string sql = $"SELECT * FROM machine_db WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(txtmid.Text))
                    sql += $" AND Machine_ID = '{txtmid.Text}'";
                if (!(txtcid2.SelectedIndex==0))
                    sql += $" AND Company_ID = '{companiesID[txtcid2.SelectedIndex]}'";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            machines.Add(new Machine(reader.GetString("Company_ID"), reader.GetString("Machine_ID"), reader.GetString("Machine_Location"), reader.GetString("Status"), reader.GetBoolean("Effect")));
                        }
                    }
                }
                connection.Close();
            }
            machdatagrid.ItemsSource = machines;
        }

        private void delbtn_Click2(object sender, RoutedEventArgs e)
        {
            if (machdatagrid.SelectedIndex >= 0 && machdatagrid.SelectedIndex < machines.Count)
            {

            }
            else
            {                // 在這裡處理索引無效的情況，例如顯示錯誤訊息或採取其他措施
                MessageBox.Show("請選擇清單內的項目", "未選取");
                return;
            }
            MessageBoxResult result = MessageBox.Show("確定要刪除嗎?(接下來請再確認三次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                result = MessageBox.Show("確定要刪除嗎?(1次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    result = MessageBox.Show("確定要刪除嗎?(2次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        result = MessageBox.Show("確定要刪除嗎?(3次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            string database = "company_db";
                            string databaseServer = "220.132.141.9";
                            string databasePort = "6833";
                            string databaseUser = "root";
                            string databasePassword = "edys1234";
                            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
                            using (MySqlConnection connection = new MySqlConnection(connectionString))
                            {
                                connection.Open();
                                string deleteSql = "DELETE FROM machine_db WHERE Machine_ID = @i";
                                using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, connection))
                                {
                                    deleteCommand.Parameters.AddWithValue("@i", machines[machdatagrid.SelectedIndex].Machine_ID);
                                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                                }
                            }
                            MySQLCreatelist();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void addbtn_Click3(object sender, RoutedEventArgs e)
        {
            Addmember newWindow3 = new Addmember(this);
            newWindow3.Show();
        }

        private void chgbtn_Click3(object sender, RoutedEventArgs e)
        {
            if (memdatagrid.SelectedIndex >= 0 && memdatagrid.SelectedIndex < members.Count)
            {
                chgmember newWindow = new chgmember(members[memdatagrid.SelectedIndex].Card_ID, this);
                newWindow.Show();
            }
            else
            {
                // 在這裡處理索引無效的情況，例如顯示錯誤訊息或採取其他措施
                MessageBox.Show("請選擇清單內的項目", "未選取");
            }
        }

        private void srhbtn_Click3(object sender, RoutedEventArgs e)
        {
            string database = "company_db";
            string databaseServer = "220.132.141.9";
            string databasePort = "6833";
            string databaseUser = "root";
            string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            memdatagrid.ItemsSource = null;
            members.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();  //資料庫連線my'Unable to connect to any of the specified MySQL hosts.''Unable to connect to any of the specified MySQL hosts.'
                                    // 在這裡執行資料庫操作
                string sql = $"SELECT * FROM member_db WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(txtname2.Text))
                    sql += $" AND Name LIKE N'%{txtname2.Text}%'";

                if (!(txtcid3.SelectedIndex == 0))
                    sql += $" AND Company_ID = '{companiesID[txtcid3.SelectedIndex]}'";

                if (!string.IsNullOrWhiteSpace(txtphone2.Text))
                    sql += $" AND Homephone LIKE '%{txtphone2.Text}%'";

                if (!string.IsNullOrWhiteSpace(txtphone3.Text))
                    sql += $" AND Cellphone LIKE '%{txtphone3.Text}%'";

                if (!string.IsNullOrWhiteSpace(txtmid2.Text))
                    sql += $" AND Member_ID = '{txtmid2.Text}'";

                if (!string.IsNullOrWhiteSpace(txtcarid.Text))
                    sql += $" AND Card_ID = '{txtcarid.Text}'";

                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            members.Add(new Member(reader.GetString("Company_ID"), reader.GetString("Card_ID"), reader.GetString("Member_ID"), reader.GetString("Name"), reader.GetString("Level"), reader.GetString("ID"), reader.GetString("Address"), reader.GetString("Cellphone"), reader.GetString("Homephone"), reader.GetString("First_Day"), reader.GetString("Birth_Day"), reader.GetBoolean("Effect")));
                        }
                    }
                }
                connection.Close();
            }
            memdatagrid.ItemsSource = members;
        }

        private void delbtn_Click3(object sender, RoutedEventArgs e)
        {
            if (memdatagrid.SelectedIndex >= 0 && memdatagrid.SelectedIndex < members.Count)
            {

            }
            else
            {                // 在這裡處理索引無效的情況，例如顯示錯誤訊息或採取其他措施
                MessageBox.Show("請選擇清單內的項目", "未選取");
                return;
            }
            MessageBoxResult result = MessageBox.Show("確定要刪除嗎?(接下來請再確認三次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                result = MessageBox.Show("確定要刪除嗎?(1次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    result = MessageBox.Show("確定要刪除嗎?(2次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        result = MessageBox.Show("確定要刪除嗎?(3次)", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            string database = "company_db";
                            string databaseServer = "220.132.141.9";
                            string databasePort = "6833";
                            string databaseUser = "root";
                            string databasePassword = "edys1234";
                            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
                            using (MySqlConnection connection = new MySqlConnection(connectionString))
                            {
                                connection.Open();
                                string deleteSql = "DELETE FROM member_db WHERE Card_ID = @i";
                                using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, connection))
                                {
                                    deleteCommand.Parameters.AddWithValue("@i", members[memdatagrid.SelectedIndex].Card_ID);
                                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                                }
                            }
                            MySQLCreatelist();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void qitbtn_Click1(object sender, RoutedEventArgs e)
        {
            comdatagrid.UnselectAll();
        }
        private void qitbtn2_Click(object sender, RoutedEventArgs e)
        {
            machdatagrid.UnselectAll();
        }

        private void qitbtn3_Click(object sender, RoutedEventArgs e)
        {
            memdatagrid.UnselectAll();
        }
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 建立 MQTT 客戶端配置
                var configuration = new MqttConfiguration
                {
                    BufferSize = 65536,
                    Port = int.Parse(port2.Text),
                    KeepAliveSecs = 10,
                    WaitTimeoutSecs = 2,
                    MaximumQualityOfService = MqttQualityOfService.AtMostOnce,
                    AllowWildcardsInTopicFilters = true
                };
                // 建立 MQTT 客戶端
                MessageBox.Show($"正在監聽: {Topic2.Text} 主題");
                var client = await MqttClient.CreateAsync((string)ip2.Text, configuration);
                // 連接到 MQTT 伺服器
                var sessionState = await client.ConnectAsync(new MqttClientCredentials(clientId: "Foo"), cleanSession: true);
                await client.SubscribeAsync(Topic2.Text, MqttQualityOfService.AtMostOnce); //QoS0
                MessageBox.Show($"已連線到伺服器");
                client.MessageStream.Subscribe(msg =>
                {
                    // 將收到的消息顯示在 UI 中
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ReceivedMessagesTextBox.Text+=$"收到來自主機 '{msg.Topic}' 的消息：{Encoding.UTF8.GetString(msg.Payload)}\n";
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"錯誤：{ex.Message}");
            }
            startbtn.IsEnabled = false;
            stopbtn.IsEnabled = true;
        }

        private void RadioButton3_Checked(object sender, RoutedEventArgs e)
        {
            var targetbutton = sender as RadioButton;
            memdatagrid.ItemsSource = null;
            string mode = targetbutton.Tag.ToString();
            // 检查选中的 RadioButton 的 Tag 属性
            switch (mode)
            {
                case "1":
                    members = members.OrderBy(p => p.Company_ID).ToList();
                    break;
                case "2":
                    members = members.OrderBy(p => p.Card_ID).ToList();
                    break;
                case "3":
                    members = members.OrderBy(p => p.Member_ID).ToList();
                    break;
            }
            memdatagrid.ItemsSource = members;
        }

        private void RadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            var targetbutton = sender as RadioButton;
            machdatagrid.ItemsSource = null;
            string mode = targetbutton.Tag.ToString();
            // 检查选中的 RadioButton 的 Tag 属性
            switch (mode)
            {
                case "1":
                    machines = machines.OrderBy(p => p.Company_ID).ToList();
                    break;
                case "2":
                    machines = machines.OrderBy(p => p.Machine_ID).ToList();
                    break;
                case "3":
                    machines = machines.OrderBy(p => p.Status).ToList();
                    break;
            }
            machdatagrid.ItemsSource = machines;
        }

        private void RadioButton1_Checked(object sender, RoutedEventArgs e)
        {
            var targetbutton = sender as RadioButton;
            comdatagrid.ItemsSource = null;
            string mode = targetbutton.Tag.ToString();
            // 检查选中的 RadioButton 的 Tag 属性
            switch (mode)
            {
                case "1":
                    companies = companies.OrderBy(p => p.Company_ID).ToList();
                    break;
                case "2":
                    companies = companies.OrderBy(p => p.Name).ToList();
                    break;
                case "3":
                    companies = companies.OrderBy(p => p.Address).ToList();
                    break;
            }
            comdatagrid.ItemsSource = companies;
        }
    }
}
