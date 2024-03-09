using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT
{
    internal class Member
    {
        public string company_id { get; set; }
        public string card_id { get; set; }
        public string name { get; set; }
        public string addr { get; set; }
        public string phone { get; set; }
        public string firstday { get; set; }
        public string birthday { get; set; }
        public Member(string coi,string cai,string n,string a,string p,string fd,string bd)
        {
            company_id=coi;
            card_id=cai;
            name=n;
            addr=a;
            phone=p;
            firstday=fd;
            birthday=bd;
        }
    }
}
