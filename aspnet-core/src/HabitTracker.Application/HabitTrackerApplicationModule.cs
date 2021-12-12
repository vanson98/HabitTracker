using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using HabitTracker.Authorization;

namespace HabitTracker
{
    [DependsOn(
        typeof(HabitTrackerCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class HabitTrackerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<HabitTrackerAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(HabitTrackerApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
