using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using HabitTracker.Investing.Dtos.InvestmentDtos;
using HabitTracker.Investing.Dtos.TransactionDtos;
using HabitTracker.Investing.Enum;
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
            var investment = await _investmentRepository.FirstOrDefaultAsync(transaction.InvestmentId);

            // Cập nhật thông tin investment
            if(investment != null && transaction.TransactionType == TransactionType.BUY)
            {
                investment.TotalAmountBuy += transaction.NumberOfShares;
                investment.TotalMoneyBuy += (transaction.NumberOfShares * transaction.Price);
                investment.AveragePrice =  (decimal)(investment.TotalMoneyBuy / investment.TotalAmountBuy);
            }else if(investment != null && transaction.TransactionType == TransactionType.SELL)
            {
                
                investment.TotalAmountSell += transaction.NumberOfShares;
                investment.TotalMoneyBuy += (transaction.NumberOfShares * transaction.Price);
                if (investment.TotalAmountSell > investment.TotalMoneyBuy)
                {
                    throw new UserFriendlyException("Không thể bán số lượng lớn hơn số lượng hiện có!");
                }
            }
            if(investment.TotalAmountBuy == investment.TotalAmountSell)
            {
                investment.Status = InvestmentStatus.BuyOut;
            }else{
                investment.Status = InvestmentStatus.Active;
            }
            await _investmentRepository.UpdateAsync(investment);

            // Cập nhật phí 
            var fee = transaction.TransactionType == TransactionType.BUY ? investmentChannel.BuyFee : investmentChannel.SellFee;
            transaction.TotalFee = (transaction.NumberOfShares * transaction.Price) * fee / 100;
            if(transaction.TransactionType == TransactionType.BUY)
            {
                investmentChannel.TotalBuyFee += transaction.TotalFee;
            }
            else
            {
                investmentChannel.TotalSellFee += transaction.TotalFee;
            }
           
            await _investmentChannelRepository.UpdateAsync(investmentChannel);
            return await base.CreateAsync(transaction);
        }

        public async override Task<TransactionDto> UpdateAsync(TransactionDto transaction)
        {
            var investmentChannel = await (from ivm in _investmentRepository.GetAll()
                                           join c in _investmentChannelRepository.GetAll() on ivm.ChannelId equals c.Id
                                           where ivm.Id == transaction.Id
                                           select c).FirstOrDefaultAsync();
            // Trừ đi phí cũ
            var fee = 0f;
            if (transaction.TransactionType == TransactionType.BUY)
            {
                fee = investmentChannel.BuyFee;
                investmentChannel.TotalBuyFee -= transaction.TotalFee;
                transaction.TotalFee = (transaction.NumberOfShares * transaction.Price) * fee / 100;
                investmentChannel.TotalBuyFee += transaction.TotalFee;
            }
            else
            {
                fee = investmentChannel.SellFee;
                investmentChannel.TotalSellFee -= transaction.TotalFee;
                transaction.TotalFee = (transaction.NumberOfShares * transaction.Price) * fee / 100;
                investmentChannel.TotalSellFee += transaction.TotalFee;
            }
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
            if (transaction.TransactionType == Enum.TransactionType.BUY)
            {
                investmentChannel.TotalBuyFee -= transaction.TotalFee;
            }
            else
            {
                investmentChannel.TotalSellFee -= transaction.TotalFee;
            }
            await _investmentChannelRepository.UpdateAsync(investmentChannel);
            await base.DeleteAsync(input);
        }
    }
}
