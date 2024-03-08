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
            public String IP { get; set; }
            public int port { get; set; }
            public String Topic {get; set;}

            public Machine(int p=1883,string i="10.0.0.1",string t="a"){
                port = p;
                IP = i;
                Topic = t;
            }
    }
}