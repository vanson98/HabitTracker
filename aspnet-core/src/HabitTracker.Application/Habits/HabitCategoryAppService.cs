using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using HabitTracker.Habits.Dtos;
using HabitTracker.Habits.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class HabitCategoryAppService :
        AsyncCrudAppService<HabitCategory, HabitCategoryDto, int, PageHabitCategoryResultRequestDto>, IHabitCategoryService
    {
        private readonly IRepository<HabitCategory, int> _repository;
        private readonly IRepository<Habit, int> _habitRepository;
        public HabitCategoryAppService(IRepository<HabitCategory, int> repository,
            IRepository<Habit, int> habitRepository) : base(repository)
        {
            _repository = repository;
            _habitRepository = habitRepository;
        }



        public override Task DeleteAsync(EntityDto<int> input)
        {
            // check is having habit
            var habits = _habitRepository.GetAll().Where(h => h.CategoryId == input.Id).ToList();
            if (habits.Count() > 0)
            {
                throw new UserFriendlyException("Category này đang có habit dùng!!!");
            }
            else
            {
                return base.DeleteAsync(input);
            }
        }

        public List<HabitCategoryAnalysisDto> GetAllCategoryAnalysis()
        {
            var result = (from x in (from c in _repository.GetAll()
                                     join h in _habitRepository.GetAll() on c.Id equals h.CategoryId into jh
                                     from h in jh.DefaultIfEmpty()
                                     select new { 
                                         PracticeTime = h != null ? h.PracticeTime : 0, 
                                         c 
                                     }).ToList()
                          group x by x.c into gc
                          select new HabitCategoryAnalysisDto()
                          {
                              Id = gc.Key.Id,
                              ColorCode = gc.Key.ColorCode,
                              Description = gc.Key.Description,
                              GoalTime = gc.Key.GoalTime,
                              IconCode = gc.Key.IconCode,
                              Name = gc.Key.Name,
                              TotalPracticeTime = gc.Sum(pt => pt.PracticeTime)
                          }).OrderByDescending(c=>c.TotalPracticeTime).ToList();
            return result;
        }
    }
}
