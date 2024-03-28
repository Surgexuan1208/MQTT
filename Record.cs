using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Effects;
using System.IO;

namespace MQTT
{
    internal class Record
    {
        public string Card_ID { get; set; }
        public string Machine_ID { get; set; }
        public string Company_ID { get; set; }
        public string Name { get; set; }
        public string Checktime { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
        public string Weekday { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public Record(string c, string mi, string ci, string n, string ch)
        {
            Card_ID = c;
            Machine_ID = mi;
            Company_ID = ci;
            Name = n;
            Checktime = ch;

            // Splitting Checktime string assuming it's formatted in a specific way
            string[] parts = ch.Split(' '); // Assuming space is the delimiter

            // Assuming the format is "YYYY-MM-DD HH:MM:SS"
            if (parts.Length == 2)
            {
                string[] dateParts = parts[0].Split('-');
                string[] timeParts = parts[1].Split(':');

                if (dateParts.Length == 3 && timeParts.Length == 3)
                {
                    Year = int.Parse(dateParts[0]);
                    Month = dateParts[1];
                    Day = int.Parse(dateParts[2]);

                    Hour = int.Parse(timeParts[0]);
                    Minute = int.Parse(timeParts[1]);
                    Second = int.Parse(timeParts[2]);
                }
            }
        }
    }
}