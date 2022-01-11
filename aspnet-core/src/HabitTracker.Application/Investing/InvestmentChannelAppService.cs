using Abp.Application.Services;
using Abp.Domain.Repositories;
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
        public InvestmentChannelAppService(IRepository<InvestmentChannel, int> repository) : base(repository)
        {
            _repository = repository;
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
    }
}
