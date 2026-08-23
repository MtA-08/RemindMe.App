using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp
{
    //reminder class
    internal class Reminder
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<string> Days { get; set; } = new List<string> { "Mo", "Di", "Mi", "Do", "Fr" };
        public int Hour { get; set; } = 12;
        public int Minute { get; set; } = 0;

        public string FormattedTime => $"{Hour:D2}:{Minute:D2}";

        public Reminder()
        {
            var defaultTime = DateTime.Now.AddHours(1);
            Hour = defaultTime.Hour;
            Minute = defaultTime.Minute;
        }
    }
}
