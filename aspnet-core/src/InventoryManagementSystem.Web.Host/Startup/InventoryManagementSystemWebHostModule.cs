using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using InventoryManagementSystem.Configuration;

namespace InventoryManagementSystem.Web.Host.Startup
{
    [DependsOn(
       typeof(InventoryManagementSystemWebCoreModule))]
    public class InventoryManagementSystemWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public InventoryManagementSystemWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InventoryManagementSystemWebHostModule).GetAssembly());
        }
    }
}
