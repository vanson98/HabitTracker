using HabitTracker.Investing.Dtos.TransactionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.TransactionDtos
{
    public class SearchTransactionOutputDto : TransactionDto
    {
        public string StockCode { get; set; }
    }
}
