using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using HabitTracker.Investing.Dtos.InvestmentDtos;
using HabitTracker.Investing.Dtos.TransactionDtos;
using HabitTracker.Investing.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing
{
    public class TransactionAppService : AsyncCrudAppService<Transaction,TransactionDto>, ITransactionService
    {
        private readonly IRepository<Investment, int> _investmentRepository;
        private readonly IRepository<Transaction, int> _repository;
        public TransactionAppService(
            IRepository<Transaction,int> repository, 
            IRepository<Investment, int> investmentRepository) : base(repository)
        {
            _repository = repository;
            _investmentRepository = investmentRepository;
        }

        [HttpGet]
        public async Task<PagedResultDto<SearchTransactionOutputDto>> SearchPaging(SearchTransactionPagingInputDto input)
        {
            var listTransactionDto = from ivm in _investmentRepository.GetAll()
                                     join t in _repository.GetAll() on ivm.Id equals t.InvestmentId
                                     where input.InvestmentId == null || ivm.Id == input.InvestmentId
                                     where input.TransactionType == null || t.TransactionType == input.TransactionType
                                     where input.FromTransactionDate == null || t.TransactionTime >= input.FromTransactionDate
                                     where input.ToTransactionDate == null || t.TransactionTime <= input.ToTransactionDate
                                     select new SearchTransactionOutputDto()
                                     {
                                         Id = t.Id,
                                         TransactionTime = t.TransactionTime,
                                         CreateDate = t.CreateDate,
                                         InvestmentId = ivm.Id,
                                         NumberOfShares = t.NumberOfShares,
                                         Price = t.Price,
                                         StockCode = ivm.StockCode,
                                         TransactionType = t.TransactionType,
                                         UpdateDate = t.UpdateDate
                                     };
            var result = new PagedResultDto<SearchTransactionOutputDto>()
            {
                Items = await listTransactionDto.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync(),
                TotalCount = listTransactionDto.Count()
            };
            return result;
        }
    }
}
