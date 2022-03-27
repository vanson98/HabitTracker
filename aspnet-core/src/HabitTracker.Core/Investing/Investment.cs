using Abp.Domain.Entities;
using HabitTracker.Investing.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Investing
{
    public class Investment : Entity<int>
    {
        /// <summary>
        /// Mã cổ phiếu
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Tổng số lượng cổ mua
        /// </summary>
        public ulong TotalAmountBuy { get; set; }

        /// <summary>
        /// Tổng số tiền mua
        /// </summary>
        [Column(TypeName = "decimal(18,5)")]
        public decimal TotalMoneyBuy { get; set; }

        /// <summary>
        /// Giá trung bình mỗi CP
        /// </summary>
        [Column(TypeName = "decimal(18,5)")]
        public decimal CapitalCost { get; set; }

        /// <summary>
        /// Giá hiện tại
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// Tổng số lượng bán
        /// </summary>
        public ulong TotalAmountSell { get; set; }

        /// <summary>
        /// Tổng số tiền bán
        /// </summary>
        [Column(TypeName = "decimal(18,5)")]
        public decimal TotalMoneySell { get; set; }

        /// <summary>
        /// Khối lượng cổ phiếu hiện thời
        /// </summary>
        public ulong Vol { get; set; }

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
        public InvestmentStatus Status { get; set; } = InvestmentStatus.NotActive;


        #region Foreign Key
        public IEnumerable<Transaction> Transactions { get; set; }

        [ForeignKey("ChannelId")]
        public InvestmentChannel InvestmentChannel { get; set; }
        #endregion


    }
}
