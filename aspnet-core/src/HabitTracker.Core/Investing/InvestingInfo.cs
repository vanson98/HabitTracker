using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing
{
    public class InvestingInfo : Entity<int>
    {
        public string InvestmentChannel { get; set; }
        // Tiền đã đem đi đầu tư
        public float MoneyInput { get; set; }
        // Tiền đã rút ra
        public float MoneyOutput { get; set; }
        // tài sản ròng (tổng tài sản)
        public float NAV { get; set; }
        // Số tiền mặt có thể mua cổ phiếu (sức mua)
        public float PurchasingPower { get; set; }
        // Giá trị thị trường CK
        public float MarketValueOfStocks { get; set; }

    }
}
