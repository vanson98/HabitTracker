using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Dtos
{
    [AutoMapTo(typeof(HabitCategory))]
    [AutoMapFrom(typeof(HabitCategory))]
    public class HabitCategoryDto : EntityDto<int>
    {
        public string Name { get; set; }
        public float GoalTime { get; set; }
        public string Description { get; set; }
        public string ColorCode { get; set; }

        public string IconCode { get; set; }
    }
}
