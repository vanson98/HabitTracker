using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using HabitTracker.Investing.Dtos.InvestmentDtos;
using HabitTracker.Investing.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabitTracker.Investing
{
    public class InvestmentAppService : AsyncCrudAppService<Investment, InvestmentDto, int, 
        PagedAndSortedResultRequestDto, 
        CreateOrUpdateInvestmentDto, 
        CreateOrUpdateInvestmentDto>, 
        IInvestmentAppService
    {
        public IRepository<Investment, int> _repository { get; set; }
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

        public List<InvestmentSelectDto> GetAllForSelect(int channelId,string keyword)
        {
            var searchKeyWord = keyword!=null? keyword.ToLower() : "";
            return _repository
                .GetAll()
                .Where(ivm => ivm.ChannelId == channelId && 
                (
                   ivm.StockCode.ToLower().Contains(searchKeyWord) 
                || ivm.CompanyName.ToLower().Contains(searchKeyWord)
                || ivm.Id.ToString().Contains(searchKeyWord)
                ))
                .OrderByDescending(ivm => ivm.Id).Select((ivm) => new InvestmentSelectDto()
                {
                    Id = ivm.Id,
                    CompanyName = ivm.CompanyName,
                    StockCode = ivm.StockCode
                }).ToList();
        }

        public async Task<PagedResultDto<InvestmentOverviewDto>> GetAllOverview(GetAllOverviewInputDto input)
        {
            var listInvestment = from ivm in _repository.GetAll()
                                 where (input.Ids == null || input.Ids.Any(id=>id==ivm.Id))
                                       && (input.Status == -1 || (int)ivm.Status == input.Status)
                                 orderby ivm.StockCode
                                 select new InvestmentOverviewDto()
                                 {
                                     Id = ivm.Id,
                                     StockCode = ivm.StockCode,
                                     Status = ivm.Status,
                                     CapitalCost = ivm.CapitalCost,
                                     MarketPrice = ivm.MarketPrice,
                                     TotalAmountBuy = ivm.TotalAmountBuy,
                                     TotalAmountSell = ivm.TotalAmountSell,
                                     TotalMoneyBuy = ivm.TotalMoneyBuy,
                                     TotalMoneySell = ivm.TotalMoneySell,
                                     Vol = ivm.Vol,
                                     CurrentInterest = ivm.CapitalCost > 0 ?
                                        ((ivm.MarketPrice - (float)ivm.CapitalCost) / ivm.MarketPrice) : 0,
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