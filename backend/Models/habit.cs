using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Frequency { get; set; } 
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        
        public List<HabitLog> Logs { get; set; } = new();
    }
}
