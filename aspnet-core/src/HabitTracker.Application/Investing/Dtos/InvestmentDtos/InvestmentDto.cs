using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HabitTracker.Investing.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.InvestmentDtos
{
    [AutoMapTo(typeof(Investment))]
    [AutoMapFrom(typeof(Investment))]
    public class InvestmentDto : EntityDto<int>
    {
        public string StockCode { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Tổng số lượng cổ mua
        /// </summary>
        public long TotalAmountBuy { get; set; }

        /// <summary>
        /// Tổng số tiền mua
        /// </summary>
        public float TotalMoneyBuy { get; set; }

        /// <summary>
        /// Giá vốn
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
        /// Mô tả 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kênh đầu tư
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public InvestmentStatus Status { get; set; }
    }
}
