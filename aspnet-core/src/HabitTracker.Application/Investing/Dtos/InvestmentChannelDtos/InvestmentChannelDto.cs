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
    public class InvestmentChannelDto : EntityDto<int>
    {
        public string ChannelCode { get; set; }

        public string ChangnelName { get; set; }

        public float MoneyInput { get; set; }

        public float MoneyOutput { get; set; }

        public float MoneyStock { get; set; }

        public float MarketValueOfStocks { get; set; }

        public float PurchaseFee { get; set; }

        public float TotalBuyFee { get; set; }
        public float TotalSellFee { get; set; }
    }
}
