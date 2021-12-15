using HabitTracker.Habits.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Dtos
{
    public class GetAllHabitLogOutputDto
    {
        public int Month { get; set; }
        public List<HabitLogDto> HabitLogDtos { get; set; }
    }
}
