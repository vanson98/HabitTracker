using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Dtos
{
    public class HabitLogColumnChartDto
    {
        public string DateAgo { get; set; }
        public float TimeLog { get; set; }
        public float AccumulationTime { get; set; }
    }
}
