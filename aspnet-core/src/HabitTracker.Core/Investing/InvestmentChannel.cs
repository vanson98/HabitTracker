using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing
{
    public class InvestmentChannel : Entity<int>
    {
        /// <summary>
        /// Mã kênh đầu tư
        /// </summary>
        public string ChannelCode { get; set; }

        /// <summary>
        /// Tên kênh đầu tư
        /// </summary>
        public string ChangnelName { get; set; }

        /// <summary>
        /// Tiền đã đem đi đầu tư
        /// </summary>
        public float MoneyInput { get; set; }

        /// <summary>
        /// Tiền đã rút ra
        /// </summary>
        public float MoneyOutput { get; set; }
        /// <summary>
        /// Tiền mặt thực có
        /// </summary>
        public float MoneyStock { get; set; }

        /// <summary>
        /// Phí mua (%)
        /// </summary>
        public float BuyFee { get; set; }

        /// <summary>
        /// Phí bán (%)
        /// </summary>
        public float SellFee { get; set; }

        /// <summary>
        /// Tổng phí mua
        /// </summary>
        public float TotalBuyFee { get; set; }

        /// <summary>
        /// Tổng phí bán
        /// </summary>
        public float TotalSellFee { get; set; }

        #region Foreign Key
        public IEnumerable<Investment> Investments { get; set; }
        #endregion
    }
}
