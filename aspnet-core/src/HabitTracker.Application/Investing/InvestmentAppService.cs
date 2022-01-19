using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using HabitTracker.Investing.Dtos.InvestmentDtos;
using HabitTracker.Investing.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing
{
    public class InvestmentAppService : AsyncCrudAppService<Investment, InvestmentDto,int, PagedAndSortedResultRequestDto,CreateOrUpdateInvestmentDto,CreateOrUpdateInvestmentDto>, IInvestmentAppService
    {
        public IRepository<Investment,int> _repository { get; set; }
        private readonly IRepository<InvestmentChannel, int> _investmentChannelRepository;
        private readonly IRepository<Transaction, int> _transactionRepository;
        public InvestmentAppService(IRepository<Investment, int> repository, 
            IRepository<Transaction, int> transactionRepository, 
            IRepository<InvestmentChannel, int> investmentChannelRepository) : base(repository)
        {
            _repository = repository;
            _transactionRepository = transactionRepository;
            _investmentChannelRepository = investmentChannelRepository;
        }
        public List<InvestmentSelectDto> GetAllForSelect()
        {
            return _repository.GetAll().OrderByDescending(ivm=>ivm.Id).Select((ivm) => new InvestmentSelectDto()
            {
                Id = ivm.Id,
                CompanyName = ivm.CompanyName,
                StockCode = ivm.StockCode
            }).ToList();
        }

        public async Task<PagedResultDto<InvestmentOverviewDto>> GetAllOverview(GetAllOverviewInputDto input)
        {
            var listInvestment = from ivm in _repository.GetAll()
                                 where ivm.StockCode.ToLower().Contains(input.StockCode.ToLower()) &&
                                       ivm.Status == input.Status
                                 orderby ivm.StockCode
                                 select new InvestmentOverviewDto()
                                 {
                                     StockCode = ivm.StockCode,
                                     Status = ivm.Status,
                                     CapitalCost = ivm.CapitalCost,
                                     MarketPrice = ivm.MarketPrice,
                                     TotalAmountBuy = ivm.TotalAmountBuy ,
                                     TotalAmountSell = ivm.TotalAmountSell ,
                                     TotalMoneyBuy = ivm.TotalMoneyBuy,
                                     TotalMoneySell = ivm.TotalMoneySell,
                                     Vol = ivm.Vol,
                                     CurrentInterest = ivm.Vol > 0 ?
                                        ((float)(ivm.Vol * ivm.MarketPrice) - (float)(ivm.Vol * ivm.CapitalCost)) / (ivm.Vol * ivm.MarketPrice) : 0,
                                     SellInterest = ivm.TotalAmountSell == 0 ? 0 : (ivm.TotalMoneySell - ivm.TotalMoneyBuy) / ivm.TotalMoneyBuy
                                 };

            var result = new PagedResultDto<InvestmentOverviewDto>()
            {
                Items = await listInvestment.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync(),
                TotalCount = await listInvestment.CountAsync()
            };
            return result;
        }
      
    }
}
