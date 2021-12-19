using Abp.Application.Services;
using Abp.Domain.Repositories;
using HabitTracker.Habits.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class HabitCategoryAppService : AsyncCrudAppService<HabitCategory, HabitCategoryDto, int, PageHabitCategoryResultRequestDto>
    {
        public HabitCategoryAppService(IRepository<HabitCategory, int> repository) : base(repository)
        {
        }
    }
}
