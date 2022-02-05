using Abp.Application.Services;
using Abp.Domain.Repositories;
using HabitTracker.Common.Dto;
using HabitTracker.Habits.Dtos;
using HabitTracker.Habits.Enum;
using HabitTracker.Habits.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class HabitAppService : AsyncCrudAppService<Habit, HabitDto>, IHabitAppService
    {
        private readonly IRepository<Habit, int> _repository;
        private readonly IRepository<HabitLog, long> _habitLogRepository;
        private readonly IRepository<HabitCategory, int> _habitCategoryRepository;

        public HabitAppService(IRepository<Habit, int> repository, IRepository<HabitLog, long> habitLogRepository, IRepository<HabitCategory, int> habitCategoryRepository) : base(repository)
        {
            this._repository = repository;
            _habitLogRepository = habitLogRepository;
            _habitCategoryRepository = habitCategoryRepository;
        }

        public async Task<Habit> UpdateHabit(HabitDto input)
        {
            var habit = _repository.FirstOrDefault(input.Id);
            habit.Name = input.Name;
            habit.GoalPerDay = input.GoalPerDay;
            habit.TimeGoal = input.TimeGoal;
            habit.Order = input.Order;
            habit.HabitLogType = input.HabitLogType;
            habit.CategoryId = input.CategoryId;
            return await _repository.UpdateAsync(habit);
        }

        /// <summary>
        /// Get all habit
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<ICollection<HabitDto>> GetAllNoPaging(string keyword, int? categoryId)
        {
            return await _repository
                .GetAll()
                .Where(h => keyword == null || h.Name.Contains(keyword))
                .Where(h => categoryId == -1 || h.CategoryId == categoryId)
                .OrderBy(h => h.Order)
                .Select(h => new HabitDto()
                {
                    Id = h.Id,
                    Name = h.Name,
                    GoalPerDay = h.GoalPerDay,
                    TimeGoal = h.TimeGoal,
                    HabitLogType = h.HabitLogType,
                    Order = h.Order,
                    PracticeTime = h.PracticeTime,
                    IsLogDaily = h.IsLogDaily
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
                if (habit.HabitLogType == HabitLogType.ByGoalTime)
                {
                    logInDay.TimeLog += input.TimeLog;
                    if (logInDay.TimeLog >= habit.GoalPerDay)
                    {
                        logInDay.Status = HabitLogStatus.Done;
                    }
                    habit.PracticeTime += input.TimeLog;
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

        public async Task<List<HabitLogColumnChartDto>> GetHabitLogByTime(int habitId, int dayAmount)
        {
            var fromDate = DateTime.Now.AddDays(-dayAmount);
            var listHabitLog = await (from hl in _habitLogRepository.GetAll()
                                      where hl.DateLog >= fromDate && hl.HabitId == habitId
                                      select hl
                          ).ToListAsync();
            var result = new List<HabitLogColumnChartDto>();
            var accumulationTime = 0f;
            for (var i = (dayAmount - 1); i >= 0; i--)
            {
                var dateLog = DateTime.Now.AddDays(-i);
                var habitLog = listHabitLog.FirstOrDefault(hl => hl.DateLog.Date == dateLog.Date);
                if (habitLog != null)
                {
                    if (i == 0)
                    {
                        accumulationTime += habitLog.TimeLog;
                    }
                    result.Add(new HabitLogColumnChartDto()
                    {
                        DateAgo = dateLog.ToString("MM/dd/yyyy"),
                        TimeLog = habitLog.TimeLog,
                        AccumulationTime = accumulationTime
                    });
                    accumulationTime += habitLog.TimeLog;
                }
                else
                {
                    result.Add(new HabitLogColumnChartDto()
                    {
                        DateAgo = dateLog.ToString("MM/dd/yyyy"),
                        TimeLog = 0,
                        AccumulationTime = accumulationTime
                    });
                }

            }
            return result;
        }

        public async Task<List<TaskDto>> GetAllTasks(bool isDaily)
        {
            return (from x in await (from h in _repository.GetAll()
                                     join hl in _habitLogRepository.GetAll() on h.Id equals hl.HabitId into jhl
                                     from hl in jhl.DefaultIfEmpty()
                                     join c in _habitCategoryRepository.GetAll() on h.CategoryId equals c.Id into jc
                                     from c in jc.DefaultIfEmpty()
                                     where (h.IsLogDaily && isDaily) || (!h.IsLogDaily && !isDaily)
                                     select new { h, hl, c }).ToListAsync()
                    group x by new { Habit = x.h, Category = x.c } into gh
                    where !isDaily || !(gh.Count(g => g.hl != null && g.hl.DateLog.Date == DateTime.Now.Date) > 0)
                    select new TaskDto()
                    {
                        HabitId = gh.Key.Habit.Id,
                        Name = gh.Key.Habit.Name,
                        CategoryName = gh.Key.Category != null ? gh.Key.Category.Name : null,
                        TimeLog = gh.Key.Habit.GoalPerDay,
                        LogType = gh.Key.Habit.HabitLogType,
                        CategoryColor = gh.Key.Category != null ? gh.Key.Category.ColorCode : null,
                        CategoryIcon = gh.Key.Category != null ? gh.Key.Category.IconCode : null
                    }).ToList();
        }

        public async Task<List<HabitLogColumnChartDto>> GetTotalWorkingOfEachDay(int dayAmount)
        {
            var fromDate = DateTime.Now.AddDays(-dayAmount);
            var groupHabitLogByDay = (from lhl in await (from hl in _habitLogRepository.GetAll()
                                                         where hl.DateLog.Date >= fromDate.Date
                                                         select new { Date = hl.DateLog.Date, WorkingTime = hl.TimeLog }).ToListAsync()
                                      group lhl by lhl.Date into ghl
                                      select new
                                      {
                                          Date = ghl.Key.Date,
                                          TotalWorkingTime = ghl.Sum(hl => hl.WorkingTime)
                                      }).ToList();
            var result = new List<HabitLogColumnChartDto>();
            var accumulationTime = 0f;
            for (var i = (dayAmount - 1); i >= 0; i--)
            {
                var dateLog = DateTime.Now.AddDays(-i);
                var habitLog = groupHabitLogByDay.FirstOrDefault(hl => hl.Date == dateLog.Date);
                if (habitLog != null)
                {
                    if (i == 0)
                    {
                        accumulationTime += habitLog.TotalWorkingTime;
                    }
                    result.Add(new HabitLogColumnChartDto()
                    {
                        DateAgo = dateLog.ToString("MM/dd/yyyy"),
                        TimeLog = habitLog.TotalWorkingTime,
                        AccumulationTime = accumulationTime
                    });
                    accumulationTime += habitLog.TotalWorkingTime;
                }
                else
                {
                    result.Add(new HabitLogColumnChartDto()
                    {
                        DateAgo = dateLog.ToString("MM/dd/yyyy"),
                        TimeLog = 0,
                        AccumulationTime = accumulationTime
                    });
                }

            }
            return result;
        }
    }
}