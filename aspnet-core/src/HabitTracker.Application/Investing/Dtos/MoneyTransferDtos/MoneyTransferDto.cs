using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HabitTracker.Investing.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.MoneyTransferDtos
{
    [AutoMapFrom(typeof(MoneyTransfer))]
    [AutoMapTo(typeof(MoneyTransfer))]
    public class MoneyTransferDto : EntityDto<int>
    {
        public decimal Amount { get; set; }
        public TransferType TransferType { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
