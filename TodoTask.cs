using System;
using System.Collections.Generic;
using System.Text;
using static TodoApp.Components.Pages.Home;

namespace TodoApp
{
    // Task class
    internal class TodoTask
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "-";
        public bool IsCompleted { get; set; } = false;

        public List<Reminder> Reminders { get; set; } = new();
        public List<int> NotificationIds { get; set; } = new();
    }
}
