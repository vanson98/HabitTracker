using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using HabitTracker.Authorization.Roles;
using HabitTracker.Authorization.Users;
using HabitTracker.MultiTenancy;
using HabitTracker.Habits;
using HabitTracker.Investing;

namespace HabitTracker.EntityFrameworkCore
{
    public class HabitTrackerDbContext : AbpZeroDbContext<Tenant, Role, User, HabitTrackerDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Habit> Habits  { get; set; }
        public DbSet<HabitLog> HabitLogs { get; set; }
        public DbSet<DailyLog> DailyLogs { get; set; }
        public DbSet<HabitCategory> HabitCategories { get; set; }
        public DbSet<Investment> Investments { get; set; }
        public DbSet<InvestingInfo> InvestingInfos { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public HabitTrackerDbContext(DbContextOptions<HabitTrackerDbContext> options)
            : base(options)
        {
        }
    }
}
