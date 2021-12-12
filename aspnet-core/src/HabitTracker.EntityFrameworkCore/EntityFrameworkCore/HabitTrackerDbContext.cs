using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using HabitTracker.Authorization.Roles;
using HabitTracker.Authorization.Users;
using HabitTracker.MultiTenancy;
using HabitTracker.Habits;

namespace HabitTracker.EntityFrameworkCore
{
    public class HabitTrackerDbContext : AbpZeroDbContext<Tenant, Role, User, HabitTrackerDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Habit> Habits  { get; set; }
        public DbSet<HabitLog> HabitLogs { get; set; }
        public DbSet<DailyLog> DailyLogs { get; set; }
        public HabitTrackerDbContext(DbContextOptions<HabitTrackerDbContext> options)
            : base(options)
        {
        }
    }
}
