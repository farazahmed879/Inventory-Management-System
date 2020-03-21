using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InventoryManagementSystem.Authorization;

namespace InventoryManagementSystem
{
    [DependsOn(
        typeof(InventoryManagementSystemCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class InventoryManagementSystemApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<InventoryManagementSystemAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(InventoryManagementSystemApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
