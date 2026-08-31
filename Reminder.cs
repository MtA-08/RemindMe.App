namespace TodoApp
{
    public class Reminder
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<DateOnly> Dates { get; set; } = new();

        public TimeOnly Time { get; set; }

        public string FormattedTime => Time.ToString("HH:mm");

        public Reminder()
        {
            var defaultDateTime = DateTime.Now.AddHours(1);

            Dates.Add(DateOnly.FromDateTime(defaultDateTime));
            Time = TimeOnly.FromDateTime(defaultDateTime);
        }
    }
}
