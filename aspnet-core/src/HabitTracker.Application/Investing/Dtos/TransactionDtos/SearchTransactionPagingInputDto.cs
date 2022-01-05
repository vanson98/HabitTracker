using Abp.Application.Services.Dto;
using HabitTracker.Investing.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.InvestmentDtos
{
    public class SearchTransactionPagingInputDto : PagedResultRequestDto
    {
        public int? InvestmentId { get; set; }
        public DateTime? FromTransactionDate { get; set; }
        public DateTime? ToTransactionDate { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}
