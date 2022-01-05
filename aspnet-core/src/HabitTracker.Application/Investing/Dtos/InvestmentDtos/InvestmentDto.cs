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
    public class InvestmentDto : EntityDto<int>
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
        /// Tổng số tiền đã mua
        /// </summary>
        public float TotalBuy { get; set; }
        /// <summary>
        /// Tổng số tiền thu được sau khi bán
        /// </summary>
        public float TotalSell { get; set; }
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
        /// Danh sách giao dịch
        /// </summary>
    }
}
