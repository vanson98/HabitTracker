using Abp.Application.Services.Dto;
using HabitTracker.Investing.Dtos.InvestmentDtos;
using HabitTracker.Investing.Dtos.TransactionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Interface
{
    public interface ITransactionService
    {
        Task<PagedResultDto<SearchTransactionOutputDto>> SearchPaging(SearchTransactionPagingInputDto input);
        
    }
}
