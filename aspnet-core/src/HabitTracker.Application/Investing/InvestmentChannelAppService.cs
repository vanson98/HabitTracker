using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using HabitTracker.Investing.Dtos.InvestmentChannelDtos;
using HabitTracker.Investing.Dtos.MoneyTransferDtos;
using HabitTracker.Investing.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing
{
    public class InvestmentChannelAppService : AsyncCrudAppService<InvestmentChannel, InvestmentChannelDto>, IInvestmentChannelAppService
    {
        public IRepository<InvestmentChannel, int> _repository { get; set; }
        public IRepository<MoneyTransfer, int> _moneyTransferRepository { get; set; }
        public IRepository<Investment, int> _investmentRepository { get; set; }

        private readonly IObjectMapper _objectMapper;
        public InvestmentChannelAppService(IRepository<InvestmentChannel, int> repository, IObjectMapper objectMapper, IRepository<Investment, int> investmentRepository, IRepository<MoneyTransfer, int> moneyTransferRepository) : base(repository)
        {
            _repository = repository;
            _objectMapper = objectMapper;
            _investmentRepository = investmentRepository;
            _moneyTransferRepository = moneyTransferRepository;
        }

        public async Task<List<ChannelSelectDto>> GetAllChannel()
        {
            return await _repository.GetAll().Select(ivc => new ChannelSelectDto()
            {
                Id = ivc.Id,
                Code = ivc.ChannelCode,
                Name = ivc.ChangnelName
            }).ToListAsync();
        }

        public async Task<InvestmentChannelDto> AddMoneyInput(string channelCode, decimal income)
        {
            var channel = await _repository.FirstOrDefaultAsync(ivt => ivt.ChannelCode == channelCode);
            if (channel == null)
            {
                throw new UserFriendlyException("Không tìm thấy channel");
            }
            // Add log
            _moneyTransferRepository.InsertAsync(new MoneyTransfer()
            {
                Amount = income,
                ChannelId = channel.Id,
                TransferType = Enum.TransferType.ADD,
                CreationTime = DateTime.Now,
            });

            channel.MoneyInput += income;
            channel.MoneyStock += income;
            return MapToEntityDto(await _repository.UpdateAsync(channel));
        }

        public async Task<InvestmentChannelDto> WithdrawMoney(string channelCode, decimal value)
        {
            var channel = await _repository.FirstOrDefaultAsync(ivt => ivt.ChannelCode == channelCode);

            if (channel != null)
            {
                if (channel.MoneyStock < value)
                {
                    throw new UserFriendlyException("Không đủ tiền để rút!");
                }
                _moneyTransferRepository.InsertAsync(new MoneyTransfer()
                {
                    Amount = value,
                    ChannelId = channel.Id,
                    TransferType = Enum.TransferType.WITHDRAW,
                    CreationTime = DateTime.Now,
                });
                channel.MoneyOutput += value;
                channel.MoneyStock -= value;
                return MapToEntityDto(await _repository.UpdateAsync(channel));
            }
            throw new UserFriendlyException("Không tìm thấy channel");
        }

        public async Task<InvestmentChannelOverviewDto> GetChannelOverview(int id)
        {
            var channelInfo = await _repository.FirstOrDefaultAsync(id);
            if (channelInfo != null)
            {
                var ivmChannelOverview = _objectMapper.Map<InvestmentChannelOverviewDto>(channelInfo);
                var listActiveInvestment = await _investmentRepository
                    .GetAll()
                    .Where(ivm => ivm.Status == Enum.InvestmentStatus.Active && ivm.ChannelId == channelInfo.Id)
                    .Select(ivm => new { Vol = ivm.Vol, TotalMoneyBuy = ivm.TotalMoneyBuy, MarketPrice = ivm.MarketPrice, CapitalCost = ivm.CapitalCost })
                    .ToListAsync();
                ivmChannelOverview.MarketValueOfStocks = listActiveInvestment.Sum(ivm => ivm.MarketPrice * ivm.Vol);
                ivmChannelOverview.ValueOfStocks = listActiveInvestment.Sum(ivm => ivm.Vol * ivm.CapitalCost);
                ivmChannelOverview.NAV = ivmChannelOverview.MarketValueOfStocks + ivmChannelOverview.MoneyStock;
                return ivmChannelOverview;
            }
            else
            {
                return null;
            }
        }

        public async Task<InvestmentChannelDto> UpdateFee(int channelId, string type, decimal value)
        {
            var investmentChannel = await _repository.FirstOrDefaultAsync(channelId);
            if (investmentChannel != null)
            {
                // SF - sell fee
                if (type == "SF")
                {
                    investmentChannel.SellFee = value;
                }
                else if (type == "BF")
                {
                    investmentChannel.BuyFee = value;
                }
                return MapToEntityDto(await _repository.UpdateAsync(investmentChannel));
            }
            throw new UserFriendlyException("Không tìm thấy channel");
        }

        public async Task<InvestmentChannelDto> DeleteMoneyTransferTransaction(int transactionId, int channelId)
        {
            var transaction = await _moneyTransferRepository.FirstOrDefaultAsync(transactionId);
            if (transaction != null)
            {
                var channel = _repository.FirstOrDefault(channelId);
                if (transaction.TransferType == Enum.TransferType.ADD)
                {
                    channel.MoneyStock -= transaction.Amount;
                    channel.MoneyInput -= transaction.Amount;
                }
                else
                {
                    channel.MoneyStock += transaction.Amount;
                    channel.MoneyOutput -= transaction.Amount;
                }
                _moneyTransferRepository.Delete(transactionId);
                return MapToEntityDto(await _repository.UpdateAsync(channel));
            }
            throw new UserFriendlyException("Không tìm thấy giao dịch");
        }

        public async Task<List<MoneyTransferDto>> GetAllMoneyTransfer(SearchMoneyTransferDto input)
        {
            return await _moneyTransferRepository.GetAll().Where(t =>
                                        t.ChannelId == input.ChannelId &&
                                        t.TransferType == input.TransferType &&
                                        (input.FromDate == null || t.CreationTime > input.FromDate) &&
                                        (input.ToDate == null || t.CreationTime < input.ToDate)
                                        ).Select(mt => new MoneyTransferDto()
                                        {
                                            Amount = mt.Amount,
                                            CreationTime = mt.CreationTime,
                                            Id = mt.Id,
                                            TransferType = mt.TransferType
                                        }).ToListAsync();
        }
    }
}
