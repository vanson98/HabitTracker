using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Dtos
{
    public class HabitLogDto
    {
        public long Id { get; set; }
        public float TimeLog { get; set; }
        public DateTime DateLog { get; set; }
        public int Status { get; set; }
    }
}
