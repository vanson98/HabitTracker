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
        public float TotalAmountBuy { get; set; }

        /// <summary>
        /// Tổng số tiền mua
        /// </summary>
        public float TotalMoneyBuy { get; set; }

        /// <summary>
        /// Tổng số lượng bán
        /// </summary>
        public float TotalAmountSell { get; set; }

        /// <summary>
        /// Tổng số tiền bán
        /// </summary>
        public float TotalMoneySell { get; set; }

        /// <summary>
        /// Giá cố phiếu hiện tại
        /// </summary>
        public float CurrentPrice { get; set; }

        /// <summary>
        /// Khối lượng cổ phiếu hiện thời
        /// </summary>
        public int Vol { get; set; }

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


        #region Foreign Key
        public IEnumerable<Transaction> Transactions { get; set; }

        [ForeignKey("ChannelId")]
        public InvestmentChannel InvestmentChannel { get; set; }
        #endregion


    }
}
