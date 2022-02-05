using Abp.Application.Services.Dto;
using HabitTracker.Investing.Dtos.InvestmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Interface
{
    public interface IInvestmentAppService
    {
        public List<InvestmentSelectDto> GetAllForSelect(int channelId);
        public Task<PagedResultDto<InvestmentOverviewDto>> GetAllOverview(GetAllOverviewInputDto input);
    }
}
