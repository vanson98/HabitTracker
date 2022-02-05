using HabitTracker.Common.Dto;
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
        Task<ICollection<HabitDto>> GetAllNoPaging(string keyword,int? categoryId);
        Task<BaseReponseDto> LogWork(LogWorkInputDto input);
        Task<List<GetAllHabitLogOutputDto>> GetAllHabitLogInYear(int habitId, int year);
        Task<List<HabitLogColumnChartDto>> GetHabitLogByTime(int habitId, int dayAmount);
        Task<List<TaskDto>> GetAllTasks(bool isDaily);
        Task<List<HabitLogColumnChartDto>> GetTotalWorkingOfEachDay(int dayAmount);
    }
}
