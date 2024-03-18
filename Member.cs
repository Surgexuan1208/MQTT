using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MQTT
{
    public class Member
    {
        public string Company_ID { get; set; }
        public string Card_ID { get; set; }
        public string Member_ID { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string Address { get; set; }
        public string Cellphone { get; set; }
        public string Homephone { get; set; }
        public string FirstDay { get; set; }
        public string BirthDay { get; set; }
        public bool Effect { get; set; }
        public Member(string coi,string cai,String m,string n,string i,string a,string c,string h,string fd,string bd,bool e)
        {
            Company_ID = coi;
            Card_ID = cai;
            Member_ID = m;
            Name = n;
            ID = i;
            Address = a;
            Cellphone = c;
            Homephone = h;
            FirstDay = fd;
            BirthDay = bd;
            Effect = e;   
        }
    }
}
