using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.InvestmentDtos
{
    [AutoMapTo(typeof(Investment))]
    [AutoMapFrom(typeof(Investment))]
    public class CreateOrUpdateInvestmentDto : EntityDto
    {
        public string StockCode { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Giá cố phiếu hiện tại
        /// </summary>
        public float MarketPrice { get; set; }

        /// <summary>
        /// Mô tả 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kênh đầu tư
        /// </summary>
        public int ChannelId { get; set; }
    }
}
