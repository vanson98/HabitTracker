using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
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
                    CompanyName = ivm.CompanyName == null ? "" : ivm.CompanyName,
                    StockCode = ivm.StockCode
                }).ToList();
        }

        public async Task<PagedResultDto<InvestmentOverviewDto>> GetAllOverview(GetAllOverviewInputDto input)
        {
            var listInvestment = from ivm in _repository.GetAll()
                                 where ivm.ChannelId == input.ChannelId 
                                        && (input.Ids == null || input.Ids.Any(id=>id==ivm.Id))
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

                                     GainLossPercent = ivm.CapitalCost > 0 ?
                                        ((ivm.MarketPrice - ivm.CapitalCost) * 100 / ivm.CapitalCost) : 0,
                                     GainLossValue = ((ivm.MarketPrice - ivm.CapitalCost) * ivm.Vol),
                                     
                                     SellInterest = ivm.Status == Enum.InvestmentStatus.BuyOut ? (ivm.TotalMoneySell - ivm.TotalMoneyBuy) : 0,
                                     SellInterestPercent = ivm.Status == Enum.InvestmentStatus.BuyOut  ? (ivm.TotalMoneySell - ivm.TotalMoneyBuy) * 100 / ivm.TotalMoneySell : 0,
                                     
                                 };

            var result = new PagedResultDto<InvestmentOverviewDto>()
            {
                Items = await listInvestment.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync(),
                TotalCount = await listInvestment.CountAsync()
            };
            return result;
        }

        public override Task DeleteAsync(EntityDto<int> input)
        {
            var totalTransaction = _transactionRepository.GetAll().Where(t => t.InvestmentId == input.Id).Count();
            if(totalTransaction > 0)
            {
                throw new UserFriendlyException("Danh mục này đã có giao dịch. Không thể xóa!");
            }
            return base.DeleteAsync(input);
        }
    }
}