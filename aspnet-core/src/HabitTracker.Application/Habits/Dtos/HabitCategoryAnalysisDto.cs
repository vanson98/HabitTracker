using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits.Dtos
{
    public class HabitCategoryAnalysisDto : HabitCategoryDto
    {
        public float TotalPracticeTime { get; set; }
    }
}
