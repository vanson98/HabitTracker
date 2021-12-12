using System.Threading.Tasks;
using Abp.Application.Services;
using HabitTracker.Sessions.Dto;

namespace HabitTracker.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
