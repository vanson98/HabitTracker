using HabitTracker.Habits.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Dtos
{
    public class LogWorkInputDto
    {
        public int HabitId { get; set; }
        // Thời gian thực hiện (phút)
        public float TimeLog { get; set; } = 0;
        public HabitLogStatus Status { get; set; }
        public DateTime DateLog { get; set; }
    }
}
