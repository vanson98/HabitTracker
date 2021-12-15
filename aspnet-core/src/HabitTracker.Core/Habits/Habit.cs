using Abp.Domain.Entities;
using HabitTracker.Habits.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class Habit : Entity<int>
    {
        [StringLength(255)]
        public string Name { get; set; }
        // Mục tiêu (600000 phút = 10000 giờ)
        public float TimeGoal { get; set; } = 600000;
        // Số phút đã thực hiện
        public float PracticeTime { get; set; } = 0;
        public int Order { get; set; }
        public float GoalPerDay { get; set; }
        public HabitLogType HabitLogType { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public virtual ICollection<HabitLog> HabitLogs { get; set; }
    }
}
