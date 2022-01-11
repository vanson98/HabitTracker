using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HabitTracker.Investing.Enum;
using System;

namespace HabitTracker.Investing.Dtos.TransactionDtos
{
    [AutoMapFrom(typeof(Transaction))]
    [AutoMapTo(typeof(Transaction))]
    public class TransactionDto : EntityDto<int>
    {
        public float Price { get; set; }

        public int NumberOfShares { get; set; }

        public DateTime TransactionTime { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public TransactionType TransactionType { get; set; }

        public int InvestmentId { get; set; }

        public float TotalFee { get; set; }
    }
}