using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using HabitTracker.EntityFrameworkCore;
using HabitTracker.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace HabitTracker.Web.Tests
{
    [DependsOn(
        typeof(HabitTrackerWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class HabitTrackerWebTestModule : AbpModule
    {
        public HabitTrackerWebTestModule(HabitTrackerEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HabitTrackerWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(HabitTrackerWebMvcModule).Assembly);
        }
    }
}