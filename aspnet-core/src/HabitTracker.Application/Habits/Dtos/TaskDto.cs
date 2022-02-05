using HabitTracker.Habits.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Dtos
{
    public class TaskDto
    {
        public int HabitId { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string CategoryColor { get; set; }
        public string CategoryIcon { get; set; }
        public float TimeLog { get; set; }
        public HabitLogType LogType { get; set; }
    }
}
