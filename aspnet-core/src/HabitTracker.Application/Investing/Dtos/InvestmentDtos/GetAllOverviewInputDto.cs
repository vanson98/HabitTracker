using Abp.Application.Services.Dto;
using HabitTracker.Investing.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Investing.Dtos.InvestmentDtos
{
    public class GetAllOverviewInputDto : PagedResultRequestDto
    {
        public int[] Ids { get; set; }
        public int Status { get; set; }
    }
}
