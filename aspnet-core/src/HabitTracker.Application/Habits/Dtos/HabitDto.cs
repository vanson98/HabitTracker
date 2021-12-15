using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HabitTracker.Habits.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Dtos
{
    [AutoMapTo(typeof(Habit))]
    [AutoMapFrom(typeof(Habit))]
    public class HabitDto : EntityDto<int>
    {
        public string Name { get; set; }
        // Mục tiêu (600000 phút = 10000 giờ)
        public float TimeGoal { get; set; } = 600000;
        public float PracticeTime { get; set; } = 0;
        public int Order { get; set; }
        public float GoalPerDay { get; set; }
        public HabitLogType HabitLogType { get; set; } = 0;
        public string Description { get; set; }
    }
}
