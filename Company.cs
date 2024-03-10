using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MQTT
{
    internal class Company
    {
        public string Company_ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Cellphone { get; set; }

        public Company(string i,string n,string a,string c) {
            Company_ID = i;
            Name = n;
            Address = a;
            Cellphone = c;
        }
    }
}
