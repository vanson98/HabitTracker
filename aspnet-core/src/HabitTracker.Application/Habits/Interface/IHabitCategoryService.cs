using HabitTracker.Habits.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Interface
{
    public interface IHabitCategoryService
    {
        public List<HabitCategoryAnalysisDto> GetAllCategoryAnalysis();
    }
}
