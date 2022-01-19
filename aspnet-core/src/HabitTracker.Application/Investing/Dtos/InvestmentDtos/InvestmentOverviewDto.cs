using HabitTracker.Investing.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.InvestmentDtos
{
    public class InvestmentOverviewDto
    {
        public string StockCode { get; set; }

        /// <summary>
        /// Tổng số lượng cổ mua
        /// </summary>
        public long TotalAmountBuy { get; set; }

        /// <summary>
        /// Tổng số tiền mua
        /// </summary>
        public float TotalMoneyBuy { get; set; }

        /// <summary>
        /// Giá trung bình mỗi CP
        /// </summary>
        public decimal CapitalCost { get; set; }

        /// <summary>
        /// Giá cố phiếu hiện tại
        /// </summary>
        public float MarketPrice { get; set; }

        /// <summary>
        /// Tổng số lượng bán
        /// </summary>
        public long TotalAmountSell { get; set; }

        /// <summary>
        /// Tổng số tiền bán
        /// </summary>
        public float TotalMoneySell { get; set; }

        /// <summary>
        /// Khối lượng cổ phiếu hiện thời
        /// </summary>
        public long Vol { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public InvestmentStatus Status { get; set; }

        /// <summary>
        /// Tiền lãi của số cổ hiện tại
        /// </summary>
        public float CurrentInterest { get; set; }
        
        /// <summary>
        /// Tiền lãi số CP đã bán
        /// </summary>
        public float SellInterest { get; set; }

    }
}
