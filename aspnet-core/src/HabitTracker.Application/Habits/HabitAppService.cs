using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using HabitTracker.Habits.Dtos;
using HabitTracker.Habits.Enum;
using HabitTracker.Habits.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class HabitAppService : CrudAppService<Habit, HabitDto>, IHabitAppService
    {
        private readonly IRepository<Habit, int> _repository;
        private readonly IRepository<HabitLog, long> _habitLogRepository;

        public HabitAppService(IRepository<Habit, int> repository, IRepository<HabitLog, long> habitLogRepository) : base(repository)
        {
            this._repository = repository;
            _habitLogRepository = habitLogRepository;
        }

        public async Task<Habit> UpdateHabit(HabitDto input)
        {
            var habit = _repository.FirstOrDefault(input.Id);
            habit.Name = input.Name;
            habit.GoalPerDay = input.GoalPerDay;
            habit.TimeGoal = input.TimeGoal;
            habit.Order = input.Order;
            habit.HabitLogType = input.HabitLogType;
            return await _repository.UpdateAsync(habit);
        }

        /// <summary>
        /// Get all habit
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<ICollection<HabitDto>> GetAllNoPaging(string keyword)
        {
            return await _repository
                .GetAll()
                .Where(h => keyword == null || h.Name.Contains(keyword))
                .OrderBy(h=>h.Order)
                .Select(h => new HabitDto()
                {
                    Id = h.Id,
                    Name = h.Name,
                    GoalPerDay = h.GoalPerDay,
                    TimeGoal = h.TimeGoal,
                    HabitLogType = h.HabitLogType,
                    Order = h.Order,
                    PracticeTime = h.PracticeTime 
                }).ToListAsync();
        }

        /// <summary>
        /// Log work for habit
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> LogWork(LogWorkInputDto input)
        {
            var habit = await _repository.FirstOrDefaultAsync(input.HabitId);
           
            // Check exist log
            var logInDay = _habitLogRepository.GetAll().Where(hl => hl.DateLog.ToShortDateString() == input.DateLog.ToShortDateString()).FirstOrDefaultAsync();
            if(logInDay!= null)
            {
                throw new UserFriendlyException("Thói quen này đã được log vào ngày này, không thể log thêm");
            }

            var habitLog = new HabitLog()
            {
                HabitId = input.HabitId,
                TimeLog = input.TimeLog,
                DateLog = input.DateLog
            };

            // Check type log
            if (habit.HabitLogType == HabitLogType.ByGoalTime)
            {
                if(input.TimeLog >= habit.GoalPerDay)
                {
                    habitLog.Status = HabitLogStatus.Done;
                }
                else
                {
                    habitLog.Status = HabitLogStatus.NotDone;
                }
            }
            else
            {
                habitLog.Status = input.Status;
            }
            
            _habitLogRepository.Insert(habitLog);
            return true;
        }
    }
}
