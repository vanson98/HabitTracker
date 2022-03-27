using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.InvestmentChannelDtos
{
    [AutoMapFrom(typeof(InvestmentChannel))]
    [AutoMapTo(typeof(InvestmentChannel))]
    public class InvestmentChannelOverviewDto : EntityDto<int>
    {
        public int Id { get; set; }
        /// <summary>
        /// Mã kênh đầu tư
        /// </summary>
        public string ChannelCode { get; set; }

        /// <summary>
        /// Tiền đã đem đi đầu tư
        /// </summary>
        public decimal MoneyInput { get; set; }

        /// <summary>
        /// Tiền đã rút ra
        /// </summary>
        public decimal MoneyOutput { get; set; }
        /// <summary>
        /// Tiền mặt thực có
        /// </summary>
        public decimal MoneyStock { get; set; }

        /// <summary>
        /// Phí mua (%)
        /// </summary>
        public decimal BuyFee { get; set; }

        /// <summary>
        /// Phí bán (%)
        /// </summary>
        public decimal SellFee { get; set; }

        /// <summary>
        /// Tổng phí mua
        /// </summary>
        public decimal TotalBuyFee { get; set; }

        /// <summary>
        /// Tổng phí bán
        /// </summary>
        public decimal TotalSellFee { get; set; }

        /// <summary>
        /// tài sản ròng (Money + giá trị thị trường)
        /// </summary>
        public decimal NAV { get; set; }

        /// <summary>
        /// Giá trị thị trường CK
        /// </summary>
        public decimal MarketValueOfStocks { get; set; }

        /// <summary>
        /// Tổng giá trị lúc mua
        /// </summary>
        public decimal ValueOfStocks { get; set; }
     
    }
}
