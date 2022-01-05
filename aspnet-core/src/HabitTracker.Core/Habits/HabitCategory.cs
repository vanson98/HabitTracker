using Abp.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Habits
{
    public class HabitCategory : Entity<int>
    {
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public float GoalTime { get; set; }
        public virtual ICollection<Habit> Habits { get; set; }
    }
}
