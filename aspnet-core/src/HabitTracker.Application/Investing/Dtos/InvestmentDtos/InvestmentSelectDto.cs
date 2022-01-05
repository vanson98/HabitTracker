using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.InvestmentDtos
{
    public class InvestmentSelectDto
    {
        public int Id { get; set; }
        public string StockCode { get; set; }
        public string CompanyName { get; set; }
    }
}
