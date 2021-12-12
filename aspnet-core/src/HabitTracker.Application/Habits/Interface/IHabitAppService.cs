using HabitTracker.Habits.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Interface
{
    public interface IHabitAppService
    {
        Task<Habit> UpdateHabit(HabitDto input);
        Task<ICollection<HabitDto>> GetAllNoPaging(string keyword);
        Task<bool> LogWork(LogWorkInputDto input);
    }
}
