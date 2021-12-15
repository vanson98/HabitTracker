using Abp.Application.Services;
using Abp.Domain.Repositories;
using HabitTracker.Common.Dto;
using HabitTracker.Habits.Dtos;
using HabitTracker.Habits.Enum;
using HabitTracker.Habits.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
                .OrderBy(h => h.Order)
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
        public async Task<BaseReponseDto> LogWork(LogWorkInputDto input)
        {
            var habit = await _repository.FirstOrDefaultAsync(input.HabitId);

            // Check exist log
            var logInDay = await _habitLogRepository.GetAll()
                .Where(hl => hl.DateLog.Date == input.DateLog.Date && hl.HabitId == habit.Id)
                .FirstOrDefaultAsync();
            
            if (logInDay != null)
            {
                // TH update
                if(habit.HabitLogType == HabitLogType.ByGoalTime)
                {

                    logInDay.TimeLog += input.TimeLog;
                }
                else
                {
                    logInDay.Status = input.Status;
                }
                _habitLogRepository.Update(logInDay);
            }
            else
            {
                var habitLog = new HabitLog()
                {
                    HabitId = input.HabitId,
                    TimeLog = input.TimeLog,
                    DateLog = input.DateLog
                };
                // Check type log
                if (habit.HabitLogType == HabitLogType.ByGoalTime)
                {
                    if (input.TimeLog >= habit.GoalPerDay)
                    {
                        habitLog.Status = HabitLogStatus.Done;
                    }
                    else
                    {
                        habitLog.Status = HabitLogStatus.NotDone;
                    }
                    habit.PracticeTime += habitLog.TimeLog;
                }
                else
                {
                    habitLog.Status = input.Status;
                }

                _habitLogRepository.Insert(habitLog);
               
            }
            _repository.Update(habit);
            return new BaseReponseDto()
            {
                StatusCode = 200,
                Message = "Đã log thành công!"
            };
        }

        public async Task<List<GetAllHabitLogOutputDto>> GetAllHabitLogInYear(int habitId, int year)
        {
            var listHabitLog = await (from hl in _habitLogRepository.GetAll()
                                      where hl.HabitId == habitId && hl.DateLog.Year == year
                                      select hl).ToListAsync();

            return (from hl in listHabitLog
                    group hl by hl.DateLog.Month into ghl
                    select new GetAllHabitLogOutputDto()
                    {
                        Month = ghl.Key,
                        HabitLogDtos = ghl.Select(hl => new HabitLogDto()
                        {
                            Id = hl.Id,
                            DateLog = hl.DateLog,
                            Status = (int)hl.Status,
                            TimeLog = hl.TimeLog
                        }).ToList()
                    }).ToList();
        }
    }
}