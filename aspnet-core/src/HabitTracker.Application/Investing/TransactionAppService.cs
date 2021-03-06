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
    public class TransactionAppService : AsyncCrudAppService<Transaction, TransactionDto>, ITransactionService
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
                                     where input.InvestmentIds == null || input.InvestmentIds.Any(id=>id==ivm.Id)
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
                                         LastModificationTime = t.LastModificationTime,
                                         TotalFee = t.TotalFee
                                     };
            var result = new PagedResultDto<SearchTransactionOutputDto>()
            {
                Items = await listTransactionDto.OrderByDescending(t => t.TransactionTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync(),
                TotalCount = listTransactionDto.Count()
            };
            return result;
        }

        public async override Task<TransactionDto> CreateAsync(TransactionDto transaction)
        {
            var investmentChannel = await (from ivm in _investmentRepository.GetAll()
                                           join c in _investmentChannelRepository.GetAll() on ivm.ChannelId equals c.Id
                                           where ivm.Id == transaction.InvestmentId
                                           select c).FirstOrDefaultAsync();
            var investment = await _investmentRepository.FirstOrDefaultAsync(transaction.InvestmentId);
            // Đơn giá giao dịch
            var amount = (transaction.NumberOfShares * transaction.Price);
            //  Phí GD
            if (transaction.TotalFee <= 0)
            {
                var fee = transaction.TransactionType == TransactionType.BUY ? investmentChannel.BuyFee : investmentChannel.SellFee;
                transaction.TotalFee = amount * fee / 100;
            }

            // Cập nhật thông tin investment
            if (investment != null && transaction.TransactionType == TransactionType.BUY)
            {
                if((transaction.TotalFee + amount) < investmentChannel.MoneyStock)
                {
                    var newVol = investment.Vol + transaction.NumberOfShares;
                    investment.TotalAmountBuy += transaction.NumberOfShares;
                    investment.TotalMoneyBuy += amount;
                    // Tính capital cost
                    investment.CapitalCost = CalculateCapitalCost(amount, investment.Vol,investment.CapitalCost,newVol);
                    investment.Vol = newVol;
                }
                else
                {
                    throw new UserFriendlyException("Không đủ tiền mặt để mua!");
                }
            }
            else if (investment != null && transaction.TransactionType == TransactionType.SELL)
            {
                if (transaction.NumberOfShares <= investment.Vol)
                {
                    investment.TotalAmountSell += transaction.NumberOfShares;
                    investment.TotalMoneySell += (transaction.NumberOfShares * transaction.Price);
                   
                    investment.Vol -= transaction.NumberOfShares;
                    transaction.CapitalCost = investment.CapitalCost;
                    
                }
                else
                {
                    throw new UserFriendlyException("Không thể bán số lượng lớn hơn số lượng hiện có!");
                }
            }

            if (investment.TotalAmountBuy == investment.TotalAmountSell)
            {
                investment.Status = InvestmentStatus.BuyOut;
            }
            else
            {
                investment.Status = InvestmentStatus.Active;
            }
            await _investmentRepository.UpdateAsync(investment);

           
           
            if (transaction.TransactionType == TransactionType.BUY)
            {
                investmentChannel.TotalBuyFee += transaction.TotalFee;
                investmentChannel.MoneyStock -= (amount + transaction.TotalFee);
            }
            else
            {
                investmentChannel.TotalSellFee += transaction.TotalFee;
                investmentChannel.MoneyStock += amount;
                investmentChannel.MoneyStock -= transaction.TotalFee;
            }

            await _investmentChannelRepository.UpdateAsync(investmentChannel);
            return await base.CreateAsync(transaction);
        }

        public async override Task DeleteAsync(EntityDto<int> input)
        {
            var transaction = await _repository.FirstOrDefaultAsync(input.Id);
            var investment = await _investmentRepository.FirstOrDefaultAsync(transaction.InvestmentId);
            var investmentChannel = await _investmentChannelRepository.FirstOrDefaultAsync(investment.ChannelId);
            var amount = transaction.NumberOfShares * transaction.Price;
            // Cập nhật thông tin investment
            if (investment != null && transaction.TransactionType == TransactionType.BUY)
            {
                investment.TotalAmountBuy -= transaction.NumberOfShares;
                investment.TotalMoneyBuy -= amount;
                investment.CapitalCost = RevertBuyCapitalCost(investment.Vol,investment.CapitalCost,transaction.NumberOfShares,transaction.Price);
                investment.Vol -= transaction.NumberOfShares;
            }
            else if (investment != null && transaction.TransactionType == TransactionType.SELL)
            {
                investment.TotalAmountSell -= transaction.NumberOfShares;
                investment.TotalMoneySell -= amount;
                investment.TotalAmountBuy += transaction.NumberOfShares;
                investment.TotalMoneyBuy += amount;
                investment.CapitalCost = RevertSellCapitalCost(investment.Vol, investment.CapitalCost, transaction.NumberOfShares, transaction.CapitalCost);
                investment.Vol += transaction.NumberOfShares;
            }
            // Cập nhật trạng thái
            if (investment.TotalAmountBuy == 0)
            {
                investment.Status = InvestmentStatus.NotActive;
            }
            else if (investment.TotalAmountBuy == investment.TotalAmountSell)
            {
                investment.Status = InvestmentStatus.BuyOut;
            }
            else
            {
                investment.Status = InvestmentStatus.Active;
            }
            await _investmentRepository.UpdateAsync(investment);

            // Cập nhật lại channel info
            if (transaction.TransactionType == TransactionType.BUY)
            {
                investmentChannel.TotalBuyFee -= transaction.TotalFee;
                investmentChannel.MoneyStock += (amount + transaction.TotalFee);
            }
            else
            {
                investmentChannel.TotalSellFee -= transaction.TotalFee;
                investmentChannel.MoneyStock -= amount;
                investmentChannel.MoneyStock += transaction.TotalFee;
            }

            await _investmentChannelRepository.UpdateAsync(investmentChannel);
            await base.DeleteAsync(input);
        }

        private decimal CalculateCapitalCost(decimal transactionAmount, ulong formerVol, decimal formerCapitalCost, ulong newVol)
        {
            return (transactionAmount + (formerVol * formerCapitalCost)) / newVol;
        }

        private decimal RevertBuyCapitalCost(ulong currentVol, decimal currentCapitalCost, ulong transactionVol, decimal transactionPrice)
        {
            if (currentVol == transactionVol)
            {
                return 0;
            }
            else
            {
                return ((currentVol * currentCapitalCost) - (transactionVol * transactionPrice)) / (currentVol - transactionVol);
            }
        }
        
        private decimal RevertSellCapitalCost(ulong currentVol, decimal currentCapitalCost, ulong transactionVol, decimal transactionCapitalCost)
        {
            return ((currentVol * currentCapitalCost) + (transactionVol * transactionCapitalCost)) / (currentVol + transactionVol);
        }
    }
}
