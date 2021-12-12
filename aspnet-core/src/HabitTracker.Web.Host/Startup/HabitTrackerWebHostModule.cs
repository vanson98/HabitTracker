using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using HabitTracker.Configuration;

namespace HabitTracker.Web.Host.Startup
{
    [DependsOn(
       typeof(HabitTrackerWebCoreModule))]
    public class HabitTrackerWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public HabitTrackerWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HabitTrackerWebHostModule).GetAssembly());
        }
    }
}
