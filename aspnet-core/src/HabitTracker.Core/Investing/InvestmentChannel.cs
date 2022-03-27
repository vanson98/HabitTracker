using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "decimal(18,8)")]
        public decimal BuyFee { get; set; }

        /// <summary>
        /// Phí bán (%)
        /// </summary>
        [Column(TypeName = "decimal(18,8)")]
        public decimal SellFee { get; set; }

        /// <summary>
        /// Tổng phí mua
        /// </summary>
        [Column(TypeName = "decimal(18,5)")]
        public decimal TotalBuyFee { get; set; }

        /// <summary>
        /// Tổng phí bán
        /// </summary>
        [Column(TypeName = "decimal(18,5)")]
        public decimal TotalSellFee { get; set; }

        #region Foreign Key
        public IEnumerable<Investment> Investments { get; set; }
        #endregion
    }
}
