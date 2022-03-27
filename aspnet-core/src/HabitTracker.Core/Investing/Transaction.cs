using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HabitTracker.Investing.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Investing
{
    public class Transaction : Entity<int>, ICreationAudited, IModificationAudited
    {
        /// <summary>
        /// Giá giao dịch trên một cố phiếu (mua/bán)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///  Số lượng cổ phiếu
        /// </summary>
        public ulong NumberOfShares { get; set; }

        /// <summary>
        ///  Ngày giao dịch
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public long? LastModifierUserId { get; set; }

        /// <summary>
        /// Thời gian sửa
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Loại giao dịch
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Id mã cổ phiếu
        /// </summary>
        public int InvestmentId { get; set; }

        /// <summary>
        /// Tổng tiền phí
        /// </summary>
        [Column(TypeName = "decimal(18,5)")]
        public decimal TotalFee { get; set; }

        /// <summary>
        /// Giá vốn lúc bán CP
        /// </summary>
        [Column(TypeName = "decimal(18,5)")]
        public decimal CapitalCost { get; set; }

        [ForeignKey("InvestmentId")]
        public Investment Investment { get; set; }
    }
}