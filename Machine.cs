using OpcLabs.EasyOpc.AlarmsAndEvents.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;
namespace MQTT
{
    internal class Machine
    {
        private
            MqttConfiguration configuration { get; set;}
            String IP { get; set; }
            int port { get; set; }
            String ClientID { get; set; }
            String Topic {get; set;}

        Machine(){
            port = 1883;
            IP = "10.0.0.1";
            ClientID = "admin";
            Topic = ""+(char)97;
            configuration= new MqttConfiguration
            {
                BufferSize = 65536,
                Port = port,
                KeepAliveSecs = 10,
                WaitTimeoutSecs = 2,
                MaximumQualityOfService = MqttQualityOfService.AtMostOnce,
                AllowWildcardsInTopicFilters = true
            };
        }
        
        
        
        
    }
}