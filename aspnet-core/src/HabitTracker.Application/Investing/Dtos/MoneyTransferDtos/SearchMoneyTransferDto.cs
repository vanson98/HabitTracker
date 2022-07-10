using HabitTracker.Investing.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.MoneyTransferDtos
{
    public class SearchMoneyTransferDto
    {
        public int ChannelId { get; set; }
        public TransferType TransferType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
