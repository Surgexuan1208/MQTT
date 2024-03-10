using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT
{
    internal class Member
    {
        public string Company_ID { get; set; }
        public string Card_ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Cellphone { get; set; }
        public string FirstDay { get; set; }
        public string BirthDay { get; set; }
        public bool Effect { get; set; }
        public Member(string coi,string cai,string n,string a,string p,string fd,string bd,bool e)
        {
            Company_ID=coi;
            Card_ID=cai;
            Name=n;
            Address=a;
            Cellphone=p;
            FirstDay=fd;
            BirthDay=bd;
            Effect = e;
        }
    }
}
