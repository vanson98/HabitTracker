using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using HabitTracker.Investing.Dtos.InvestmentChannelDtos;
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
        public IRepository<InvestmentChannel,int> _repository { get; set; }
        public IRepository<Investment, int> _investmentRepository { get; set; }

        private readonly IObjectMapper _objectMapper;
        public InvestmentChannelAppService(IRepository<InvestmentChannel, int> repository, IObjectMapper objectMapper, IRepository<Investment, int> investmentRepository) : base(repository)
        {
            _repository = repository;
            _objectMapper = objectMapper;
            _investmentRepository = investmentRepository;
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

        public async Task<InvestmentChannelDto> AddMoneyInput(string channelCode,float income)
        {
            var channel = await _repository.FirstOrDefaultAsync(ivt => ivt.ChannelCode == channelCode);
            if(channel== null)
            {
                return null;
            }
            channel.MoneyInput += income;
            channel.MoneyStock += income;
            return MapToEntityDto(await _repository.UpdateAsync(channel));
        }

        public async Task<InvestmentChannelDto> WithdrawMoney(string channelCode, float value)
        {
            var channel = await _repository.FirstOrDefaultAsync(ivt => ivt.ChannelCode == channelCode);
            if(channel!= null)
            {
                channel.MoneyOutput += value;
                channel.MoneyStock -= value;
                // Phí rút
                return MapToEntityDto(await _repository.UpdateAsync(channel));
            }
            return null;
        }

        public async Task<InvestmentChannelOverviewDto> GetChannelOverview(int id)
        {
            var channelInfo = await _repository.FirstOrDefaultAsync(id);
            if(channelInfo != null)
            {
                var ivmChannelOverview = _objectMapper.Map<InvestmentChannelOverviewDto>(channelInfo);
                var listActiveInvestment = await _investmentRepository
                    .GetAll()
                    .Where(ivm => ivm.Status == Enum.InvestmentStatus.Active && ivm.ChannelId == channelInfo.Id)
                    .Select(ivm => new { Vol = ivm.Vol, CapitalCost = ivm.CapitalCost, MarketPrice = ivm.MarketPrice })
                    .ToListAsync();
                ivmChannelOverview.MarketValueOfStocks = listActiveInvestment.Sum(ivm => ivm.MarketPrice * ivm.Vol);
                ivmChannelOverview.ValueOfStocks = (float)listActiveInvestment.Sum(ivm => ivm.CapitalCost * ivm.Vol);
                ivmChannelOverview.NAV = ivmChannelOverview.MarketValueOfStocks + ivmChannelOverview.MoneyStock;
                return ivmChannelOverview;
            }
            else
            {
                return null;
            }
        }
    }
}
