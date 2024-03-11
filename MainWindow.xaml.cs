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
            SetDataGridTextSize(comdatagrid, 22);
            SetDataGridTextSize(machdatagrid, 22);
            SetDataGridTextSize(memdatagrid, 22);
            MySQLOpen();
        }
        List<Member> members = new List<Member>();
        List<Machine> machines = new List<Machine>();
        List<Company> companies = new List<Company>();
        private void MySQLOpen()
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
                string sql = "SELECT * FROM member_db";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            members.Add(new Member(reader.GetString("Company_ID"), reader.GetString("Card_ID"), reader.GetString("Name"), reader.GetString("Address"), reader.GetString("Cellphone"), reader.GetString("First_Day"), reader.GetString("Birth_Day"), reader.GetBoolean("Effect")));
                        }
                    }
                }
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
                            companies.Add(new Company(reader.GetString("ID"), reader.GetString("Name"), reader.GetString("Address"), reader.GetString("Cellphone")));
                        }
                    }
                }
            }
            comdatagrid.ItemsSource = companies;
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
                            machines.Add(new Machine(reader.GetString("Company_ID"), reader.GetString("Machine_ID"), reader.GetString("Machine_Location"), reader.GetString("Status"), reader.GetBoolean("Effect")));
                        }
                    }
                }
            }
            machdatagrid.ItemsSource = machines;
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
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
          
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
            var message1 = new MqttApplicationMessage(Topic2.Text, Encoding.UTF8.GetBytes("Stop"));
            await client.PublishAsync(message1, MqttQualityOfService.AtMostOnce); //QoS0
            startbtn.IsEnabled = true;
            ip2.IsEnabled = true; port2.IsEnabled = true; ClientID2.IsEnabled = true; Topic2.IsEnabled = true;

        }
        private void startbtn1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
