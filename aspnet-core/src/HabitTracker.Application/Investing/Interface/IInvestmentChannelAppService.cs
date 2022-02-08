using HabitTracker.Investing.Dtos.InvestmentChannelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Interface
{
    public interface IInvestmentChannelAppService
    {
        public Task<List<ChannelSelectDto>> GetAllChannel();
        public Task<InvestmentChannelDto> AddMoneyInput(string channelCode, float income);
        public Task<InvestmentChannelDto> WithdrawMoney(string channelCode, float value);
        public Task<InvestmentChannelDto> UpdateFee(int channelId, string type,float value); 
        public Task<InvestmentChannelOverviewDto> GetChannelOverview(int id);
    }
}
