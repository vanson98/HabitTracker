using Abp.Domain.Entities;
using HabitTracker.Habits.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class DailyLog : Entity<int>
    {
        public DateTime Date { get; set; }
        public EmotionStatus? Status { get; set; }
    }
}
