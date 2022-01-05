﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HabitTracker.Investing.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing
{
    public class Transaction : Entity<int>
    {
        /// <summary>
        /// Giá giao dịch trên một cố phiếu (mua/bán)
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        ///  Số lượng cổ phiếu
        /// </summary>
        public int NumberOfShares { get; set; }
        /// <summary>
        ///  Ngày giao dịch
        /// </summary>
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// Ngày tạo trên hệ thống
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Thời gian update trên hệ thống
        /// </summary>
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// Loại giao dịch
        /// </summary>
        public TransactionType TransactionType { get; set; }
        /// <summary>
        /// Id mã cổ phiếu
        /// </summary>
        public int InvestmentId { get; set; }

        [ForeignKey("InvestmentId")]
        public Investment Investment { get; set; }
    }
}
