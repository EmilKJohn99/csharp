using System;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class HabitLog
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }

        [JsonIgnore]
        public Habit Habit { get; set; }
    }
}
