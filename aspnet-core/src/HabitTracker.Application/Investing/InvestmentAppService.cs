﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using HabitTracker.Investing.Dtos.InvestmentDtos;
using HabitTracker.Investing.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing
{
    public class InvestmentAppService : AsyncCrudAppService<Investment, InvestmentDto>, IInvestmentAppService
    {
        public IRepository<Investment,int> _repository { get; set; }
        public InvestmentAppService(IRepository<Investment, int> repository) : base(repository)
        {
            _repository = repository;
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

        
    }
}
