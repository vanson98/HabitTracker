using Abp.Application.Services;
using Abp.Domain.Repositories;
using HabitTracker.Habits.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class DailyLogAppService : ApplicationService
    {
        public IRepository<DailyLog, int> _repository;
        public DailyLogAppService(IRepository<DailyLog, int> repository)
        {
            _repository = repository;
        }

        public void PushDate()
        {
            var dateMark = new DateTime(1998, 9, 15);
            for (int i = 1; i <= 25620; i++)
            {
                dateMark = dateMark.AddDays(1);
                var dailyLog = new DailyLog()
                {
                    Date = dateMark,
                    Status = dateMark < DateTime.Now ? EmotionStatus.Normal : null
                };
                _repository.Insert(dailyLog);
            }
        }
    }
}
