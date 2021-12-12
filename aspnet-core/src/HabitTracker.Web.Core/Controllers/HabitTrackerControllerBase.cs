using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace HabitTracker.Controllers
{
    public abstract class HabitTrackerControllerBase: AbpController
    {
        protected HabitTrackerControllerBase()
        {
            LocalizationSourceName = HabitTrackerConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
