using Abp.Domain.Entities;
using HabitTracker.Habits.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Habits
{
    public class Habit : Entity<int>
    {

        public int? CategoryId { get; set; }
        
        [StringLength(255)]
        public string Name { get; set; }
        
        // Mục tiêu (600000 phút = 10000 giờ)
        public float TimeGoal { get; set; }
        
        // Số phút đã thực hiện
        public float PracticeTime { get; set; } = 0;
        
        // Số thứ tự
        public int Order { get; set; }

        // Mục tiêu mỗi ngày
        public float GoalPerDay { get; set; }
        
        // Kiểu log
        public HabitLogType HabitLogType { get; set; }
        [StringLength(500)]
        
        // Mô tả
        public string Description { get; set; }

        // Có log hằng ngày ko
        public bool IsLogDaily { get; set; }

        public virtual ICollection<HabitLog> HabitLogs { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual HabitCategory HabitCategory { get; set; }
    }
}
