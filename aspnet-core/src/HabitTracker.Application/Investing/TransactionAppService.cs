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
        private readonly IRepository<InvestmentChannel, int> _investmentChannelRepository;
        private readonly IRepository<Transaction, int> _repository;
        public TransactionAppService(
            IRepository<Transaction, int> repository,
            IRepository<Investment, int> investmentRepository, 
            IRepository<InvestmentChannel, int> investmentChannelRepository) : base(repository)
        {
            _repository = repository;
            _investmentRepository = investmentRepository;
            _investmentChannelRepository = investmentChannelRepository;
            _investmentChannelRepository = investmentChannelRepository;
        }

        [HttpGet]
        public async Task<PagedResultDto<SearchTransactionOutputDto>> SearchPaging(SearchTransactionPagingInputDto input)
        {
            var listTransactionDto = from ivm in _investmentRepository.GetAll()
                                     join t in _repository.GetAll() on ivm.Id equals t.InvestmentId
                                     where input.InvestmentId == null || ivm.Id == input.InvestmentId
                                     where input.TransactionType == -1 || (int)t.TransactionType == input.TransactionType
                                     where input.FromTransactionDate == null || t.TransactionTime >= input.FromTransactionDate
                                     where input.ToTransactionDate == null || t.TransactionTime <= input.ToTransactionDate
                                     select new SearchTransactionOutputDto()
                                     {
                                         Id = t.Id,
                                         TransactionTime = t.TransactionTime,
                                         CreationTime = t.CreationTime,
                                         InvestmentId = ivm.Id,
                                         NumberOfShares = t.NumberOfShares,
                                         Price = t.Price,
                                         StockCode = ivm.StockCode,
                                         TransactionType = t.TransactionType,
                                         LastModificationTime = t.LastModificationTime
                                     };
            var result = new PagedResultDto<SearchTransactionOutputDto>()
            {
                Items = await listTransactionDto.OrderByDescending(t=>t.TransactionTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync(),
                TotalCount = listTransactionDto.Count()
            };
            return result;
        }

        public async override Task<TransactionDto> CreateAsync(TransactionDto transaction)
        {
            var investmentChannel = await (from ivm in _investmentRepository.GetAll()
                                     join c in _investmentChannelRepository.GetAll() on ivm.ChannelId equals c.Id
                                     where ivm.Id == transaction.Id
                                     select c).FirstOrDefaultAsync();
            var fee = transaction.TransactionType == Enum.TransactionType.BUY ? investmentChannel.BuyFee : investmentChannel.SellFee;
            transaction.TotalFee = (transaction.NumberOfShares * transaction.Price) * fee / 100;
            investmentChannel.Overheads += transaction.TotalFee;
            _investmentChannelRepository.Update(investmentChannel);
            return await base.CreateAsync(transaction);
        }

        public async override Task<TransactionDto> UpdateAsync(TransactionDto transaction)
        {
            var investmentChannel = await (from ivm in _investmentRepository.GetAll()
                                     join c in _investmentChannelRepository.GetAll() on ivm.ChannelId equals c.Id
                                     where ivm.Id == transaction.Id
                                     select c).FirstOrDefaultAsync();
            // Trừ đi phí cũ
            investmentChannel.Overheads -= transaction.TotalFee;
            // Tính phí mới
            var fee = transaction.TransactionType == Enum.TransactionType.BUY ? investmentChannel.BuyFee : investmentChannel.SellFee;
            transaction.TotalFee = (transaction.NumberOfShares * transaction.Price) * fee / 100;
            investmentChannel.Overheads += transaction.TotalFee;
            _investmentChannelRepository.Update(investmentChannel);
            return await base.UpdateAsync(transaction);
        }

        public async override Task DeleteAsync(EntityDto<int> input)
        {
            var transaction = await _repository.FirstOrDefaultAsync(input.Id);

            var investmentChannel = await (from ivm in _investmentRepository.GetAll()
                                     join c in _investmentChannelRepository.GetAll() on ivm.ChannelId equals c.Id
                                     where ivm.Id == transaction.Id
                                     select c).FirstOrDefaultAsync();
            investmentChannel.Overheads -= transaction.TotalFee;
            await _investmentChannelRepository.UpdateAsync(investmentChannel);
            await base.DeleteAsync(input);
        }
    }
}
