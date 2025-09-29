using Microsoft.EntityFrameworkCore;
using backend.Models; // <-- reference your Models folder

namespace backend.Data  // <-- matches folder structure
{
    public class HabitContext : DbContext
    {
        public HabitContext(DbContextOptions<HabitContext> options) : base(options) { }

        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitLog> HabitLogs { get; set; }
    }
}
