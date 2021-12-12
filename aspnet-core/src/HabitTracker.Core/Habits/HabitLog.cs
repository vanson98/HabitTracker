using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HabitTracker.Authorization.Users;
using HabitTracker.Habits.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class HabitLog : Entity<long>
    {
        public int HabitId { get; set; }
        // Thời gian thực hiện (phút)
        public float TimeLog { get; set; }
        public HabitLogStatus Status { get; set; }
        public DateTime DateLog { get; set; }

        [ForeignKey("HabitId")]
        public virtual Habit Habit { get; set; }
    }
}
