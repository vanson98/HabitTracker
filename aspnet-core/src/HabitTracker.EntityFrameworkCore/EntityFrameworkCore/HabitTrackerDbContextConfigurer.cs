using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.EntityFrameworkCore
{
    public static class HabitTrackerDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<HabitTrackerDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<HabitTrackerDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
