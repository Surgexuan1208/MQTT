using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace MQTT
{
    internal class Machine
    {
        public string Company_ID { get; set; }
        public string Machine_ID { get; set; }
        public string Machine_Location { get; set; }
        public string Status { get; set; }
        public bool Effect { get; set; }
        public Machine(string c,string mi,string  ml,string s,bool e) {
            Company_ID = c;
            Machine_ID = mi;
            Machine_Location = ml;
            Status = s;
            Effect = e;
        }
    }
}
