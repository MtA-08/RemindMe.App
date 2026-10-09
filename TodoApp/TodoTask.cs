namespace TodoApp
{
    public class TodoTask
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public List<Reminder> Reminders { get; set; } = new();

        public List<int> NotificationIds { get; set; } = new();
    }
}
