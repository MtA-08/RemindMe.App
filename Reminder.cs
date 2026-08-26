using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp
{
    //reminder class
    public class Reminder
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<string> Days { get; set; } = new List<string> { "Heute" };
        public int Hour { get; set; }
        public int Minute { get; set; }

        public string FormattedTime => $"{Hour:D2}:{Minute:D2}";

        public Reminder()
        {
            var defaultTime = DateTime.Now.AddHours(1);
            Hour = defaultTime.Hour;
            Minute = defaultTime.Minute;
        }

    }
}
