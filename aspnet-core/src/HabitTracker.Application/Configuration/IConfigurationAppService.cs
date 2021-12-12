using System.Threading.Tasks;
using HabitTracker.Configuration.Dto;

namespace HabitTracker.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
