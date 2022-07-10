using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HabitTracker.Investing.Enum;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace HabitTracker.Investing
{
    public class MoneyTransfer : Entity<int>, ICreationAudited
    {
        public int ChannelId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Amount { get; set; }
        public TransferType TransferType { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }

        [ForeignKey("ChannelId")]
        public InvestmentChannel Channel { get; set; }
    }
}
