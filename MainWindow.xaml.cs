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
        }
        List<Member> members = new List<Member>();
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
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (Topic2.Text.Length < 1 || Topic2.Text.Length == 0)
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
                    Port = int.Parse(port2.Text),
                    KeepAliveSecs = 10,
                    WaitTimeoutSecs = 2,
                    MaximumQualityOfService = MqttQualityOfService.AtMostOnce,
                    AllowWildcardsInTopicFilters = true
                };
                // 建立 MQTT 客戶端
                var client = await MqttClient.CreateAsync((string)ip2.Text, configuration);
                MessageBox.Show("已建立 MQTT 客戶端！");
                // 連接到 MQTT 伺服器
                var sessionState = await client.ConnectAsync(new MqttClientCredentials(clientId: ClientID2.Text), cleanSession: true);
                MessageBox.Show("已連接到 MQTT 伺服器！");
                startbtn.IsEnabled = false;
                stopbtn.IsEnabled = true;
                ip2.IsEnabled = false; port2.IsEnabled = false; ClientID2.IsEnabled = false; Topic2.IsEnabled = false;
                await client.SubscribeAsync(Topic2.Text, MqttQualityOfService.AtMostOnce); //QoS0
                client.MessageStream.Subscribe(async msg =>
                {
                    // 將收到的消息顯示在 UI 中
                    Dispatcher.Invoke(() =>
                    {
                        ReceivedMessagesTextBox.AppendText($"收到來自主題 '{msg.Topic}' 的消息：{Encoding.UTF8.GetString(msg.Payload)}\n");
                    });
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        // 斷開 MQTT 連接
                        await client.DisconnectAsync();
                        MessageBox.Show("已斷開 MQTT 連接！");
                        cancellationTokenSource = new CancellationTokenSource();
                    }
                });
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
            var message1 = new MqttApplicationMessage(Topic2.Text, Encoding.UTF8.GetBytes("Stop"));
            await client.PublishAsync(message1, MqttQualityOfService.AtMostOnce); //QoS0
            startbtn.IsEnabled = true;
            ip2.IsEnabled = true; port2.IsEnabled = true; ClientID2.IsEnabled = true; Topic2.IsEnabled = true;

        }
        private void startbtn1_Click(object sender, RoutedEventArgs e)
        {
            /*if(arr.Count == 0)
            {
                MessageBox.Show("無任何機台登記");
                return;
            }*/
            const string database = "company_db";
            const string databaseServer = "220.132.141.9";
            const string databasePort = "6833";
            const string databaseUser = "root";
            const string databasePassword = "edys1234";
            string connectionString = $"server={databaseServer};" + $"port={databasePort};" + $"user={databaseUser};" + $"password={databasePassword};" + $"database={database};" + "charset=utf8;";
            using (MySqlConnection connection = new MySqlConnection(connectionString)){
                MessageBox.Show("正在連接資料庫");
                connection.Open();  //資料庫連線my'Unable to connect to any of the specified MySQL hosts.''Unable to connect to any of the specified MySQL hosts.'
                MessageBox.Show("已連接資料庫");
                // 在這裡執行資料庫操作
                string sql = "SELECT * FROM member_db";
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            members.Add(new Member(reader.GetString("Company_ID"),reader.GetString("Card_ID"),reader.GetString("Name"),reader.GetString("Address"), reader.GetString("Cellphone"), reader.GetString("First_Day"), reader.GetString("Birth_Day")));
                        }
                    }
                }
                connection.Close(); //資料庫斷線
                MessageBox.Show("資料傳送完成");
                datagrid.ItemsSource=members;
            }
        }
    }
}
