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
            Createlist();
        }
        List<char> items = new List<char>();
        List<String> machines = new List<String>();
        List<Machine> arr = new List<Machine>();
        private void Createlist()
        {
            for(int i = 0; i <= 26; i++){
                items.Add((char)(97 + i));
            }
            for (int i = 1; i <= 26; i++)
            {
                machines.Add("逼卡機 " + i.ToString() + "  (代號" + items[i-1] +"機)");
            }
            combobox.ItemsSource = machines;
            combobox.SelectedIndex = 0;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ip3.Text.Length < 1 || port3.Text.Length < 1)
            {
                MessageBox.Show("格子內資料不能為空");
                return;
            }
            arr.Add(new Machine(int.Parse(port3.Text), ip3.Text, items[combobox.SelectedIndex] + ""));
            datagrid.ItemsSource = arr;
            MessageBox.Show("建立成功");
        }
        private async void SubscribeToTopic(Machine m)
        {
            try
            {
                // 建立 MQTT 客戶端配置
                var configuration = new MqttConfiguration
                {
                    BufferSize = 65536,
                    Port = m.port,
                    KeepAliveSecs = 10,
                    WaitTimeoutSecs = 2,
                    MaximumQualityOfService = MqttQualityOfService.AtMostOnce,
                    AllowWildcardsInTopicFilters = true
                };
                // 建立 MQTT 客戶端
                var client = await MqttClient.CreateAsync((string)m.IP, configuration);
                MessageBox.Show($"主機'{m.Topic}'已建立 MQTT 客戶端！");
                // 連接到 MQTT 伺服器
                var sessionState = await client.ConnectAsync(new MqttClientCredentials(clientId: "Foo"), cleanSession: true);
                MessageBox.Show($"主機'{m.Topic}'已連接到 MQTT 伺服器！");
                startbtn1.IsEnabled = false;
                await client.SubscribeAsync(m.Topic, MqttQualityOfService.AtMostOnce); //QoS0
                client.MessageStream.Subscribe(async msg =>
                {
                    // 將收到的消息顯示在 UI 中
                    Dispatcher.Invoke(() =>
                    {
                        listener.AppendText($"收到來自主機 '{msg.Topic}' 的消息：{Encoding.UTF8.GetString(msg.Payload)}\n");
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
        private void startbtn1_Click(object sender, RoutedEventArgs e)
        {
            if(arr.Count == 0)
            {
                MessageBox.Show("無任何機台登記");
                return;
            }
            startbtn1.IsEnabled = false;
            MessageBox.Show("開始監聽");
            /*for (int i = 0; i < arr.Count; i++)
            {
                SubscribeToTopic(arr[i]);
            }*/
            SubscribeToTopic(arr[0]);
            SubscribeToTopic(arr[1]);
        }
    }
}
