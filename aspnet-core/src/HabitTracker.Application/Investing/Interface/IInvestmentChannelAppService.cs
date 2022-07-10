using HabitTracker.Investing.Dtos.InvestmentChannelDtos;
using HabitTracker.Investing.Dtos.MoneyTransferDtos;
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
        public Task<InvestmentChannelDto> AddMoneyInput(string channelCode, decimal income);
        public Task<InvestmentChannelDto> WithdrawMoney(string channelCode, decimal value);
        public Task<InvestmentChannelDto> UpdateFee(int channelId, string type,decimal value); 
        public Task<InvestmentChannelOverviewDto> GetChannelOverview(int id);
        Task<List<MoneyTransferDto>> GetAllMoneyTransfer(SearchMoneyTransferDto input);
        Task<InvestmentChannelDto> DeleteMoneyTransferTransaction(int transactionId, int channelId);
    }
}
