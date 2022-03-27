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
        public int Id { get; set; }
        public string StockCode { get; set; }

        /// <summary>
        /// Tổng số lượng cổ mua
        /// </summary>
        public ulong TotalAmountBuy { get; set; }

        /// <summary>
        /// Tổng số tiền mua
        /// </summary>
        public decimal TotalMoneyBuy { get; set; }

        /// <summary>
        /// Giá trung bình mỗi CP
        /// </summary>
        public decimal CapitalCost { get; set; }

        /// <summary>
        /// Giá cố phiếu hiện tại
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// Tổng số lượng bán
        /// </summary>
        public ulong TotalAmountSell { get; set; }

        /// <summary>
        /// Tổng số tiền bán
        /// </summary>
        public decimal TotalMoneySell { get; set; }

        /// <summary>
        /// Khối lượng cổ phiếu hiện thời
        /// </summary>
        public ulong Vol { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public InvestmentStatus Status { get; set; }

        /// <summary>
        /// Tiền lãi của số cổ hiện tại
        /// </summary>
        public decimal GainLossValue { get; set; }
        
        /// <summary>
        /// Lãi lỗ hiện tại (phần trăm)
        /// </summary>
        public decimal GainLossPercent { get; set; }

        /// <summary>
        /// Tiền lãi số CP đã bán
        /// </summary>
        public decimal SellInterest { get; set; }

        /// <summary>
        /// Tỉ lệ lãi của số cổ đã bán
        /// </summary>
        public decimal SellInterestPercent { get; set; }
    }
}
