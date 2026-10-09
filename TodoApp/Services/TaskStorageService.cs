using Microsoft.JSInterop;
using System.Globalization;
using System.Text.Json;

namespace TodoApp.Services
{
    public class TaskStorageService
    {
        private const string StorageKey = "myTasks";

        private readonly IJSRuntime _jsRuntime;

        public TaskStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<List<TodoTask>> LoadTasksAsync()
        {
            string? json = await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem",
                StorageKey);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<TodoTask>();
            }

            try
            {
                if (UsesLegacyReminderFormat(json))
                {
                    var legacyTasks =
                        JsonSerializer.Deserialize<List<LegacyTodoTask>>(json)
                        ?? new List<LegacyTodoTask>();

                    var migratedTasks = legacyTasks
                        .Select(ConvertLegacyTask)
                        .ToList();

                    // Nach erfolgreicher Konvertierung direkt
                    // im neuen Format speichern.
                    await SaveTasksAsync(migratedTasks);

                    return migratedTasks;
                }

                return JsonSerializer.Deserialize<List<TodoTask>>(json)
                       ?? new List<TodoTask>();
            }
            catch (JsonException)
            {
                // Beschädigte Daten sollen nicht die komplette App abstürzen lassen.
                return new List<TodoTask>();
            }
        }

        public async Task SaveTasksAsync(IEnumerable<TodoTask> tasks)
        {
            string json = JsonSerializer.Serialize(tasks);

            await _jsRuntime.InvokeVoidAsync(
                "localStorage.setItem",
                StorageKey,
                json);
        }

        // prüft ob Daten aus der alten Erinnerung version Stammen
        private static bool UsesLegacyReminderFormat(string json)
        {
            using JsonDocument document = JsonDocument.Parse(json);

            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                return false;
            }

            foreach (JsonElement taskElement in document.RootElement.EnumerateArray())
            {
                if (!taskElement.TryGetProperty("Reminders", out JsonElement reminders) ||
                    reminders.ValueKind != JsonValueKind.Array)
                {
                    continue;
                }

                foreach (JsonElement reminder in reminders.EnumerateArray())
                {
                    if (reminder.TryGetProperty("Days", out _) ||
                        reminder.TryGetProperty("Hour", out _) ||
                        reminder.TryGetProperty("Minute", out _))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static TodoTask ConvertLegacyTask(LegacyTodoTask legacyTask)
        {
            return new TodoTask
            {
                Id = string.IsNullOrWhiteSpace(legacyTask.Id)
                    ? Guid.NewGuid().ToString()
                    : legacyTask.Id,

                Name = legacyTask.Name,
                Description = legacyTask.Description,
                IsCompleted = legacyTask.IsCompleted,

                NotificationIds =
                    legacyTask.NotificationIds ?? new List<int>(),

                Reminders = (legacyTask.Reminders ?? new List<LegacyReminder>())
                    .Select(ConvertLegacyReminder)
                    .ToList()
            };
        }

        private static Reminder ConvertLegacyReminder(
            LegacyReminder legacyReminder)
        {
            int hour = Math.Clamp(legacyReminder.Hour, 0, 23);
            int minute = Math.Clamp(legacyReminder.Minute, 0, 59);

            var time = new TimeOnly(hour, minute);

            var dates = new List<DateOnly>();

            foreach (string day in legacyReminder.Days ?? new List<string>())
            {
                if (day == "Heute")
                {
                    dates.Add(
                        DateOnly.FromDateTime(DateTime.Today));
                }
                else if (day == "Morgen")
                {
                    dates.Add(
                        DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
                }
                else if (DateOnly.TryParseExact(
                             day,
                             "yyyy-MM-dd",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.None,
                             out DateOnly parsedDate))
                {
                    dates.Add(parsedDate);
                }
            }

            Guid reminderId =
                Guid.TryParse(legacyReminder.Id, out Guid parsedId)
                    ? parsedId
                    : Guid.NewGuid();

            return new Reminder
            {
                Id = reminderId,
                Dates = dates.Distinct().OrderBy(d => d).ToList(),
                Time = time
            };
        }

        private class LegacyTodoTask
        {
            public string Id { get; set; } = string.Empty;

            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public bool IsCompleted { get; set; }

            public List<LegacyReminder> Reminders { get; set; } = new();

            public List<int> NotificationIds { get; set; } = new();
        }

        private class LegacyReminder
        {
            public string Id { get; set; } = string.Empty;

            public List<string> Days { get; set; } = new();

            public int Hour { get; set; }

            public int Minute { get; set; }
        }
    }
}