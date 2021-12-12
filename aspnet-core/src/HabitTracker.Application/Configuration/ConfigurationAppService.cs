using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using HabitTracker.Configuration.Dto;

namespace HabitTracker.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : HabitTrackerAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
