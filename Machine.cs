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
            private MqttConfiguration configuration { get; set;}
            public String IP { get; set; }
            public int port { get; set; }
            private String ClientID { get; set; }
            public String Topic {get; set;}

            public Machine(int p=1883,string i="10.0.0.1",string t="a"){
                port = p;
                IP = i;
                ClientID = "admin";
                Topic = t;
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