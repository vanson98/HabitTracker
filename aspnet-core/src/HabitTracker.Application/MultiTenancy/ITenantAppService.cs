using Abp.Application.Services;
using HabitTracker.MultiTenancy.Dto;

namespace HabitTracker.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

