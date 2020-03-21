using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using InventoryManagementSystem.Configuration.Dto;

namespace InventoryManagementSystem.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : InventoryManagementSystemAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
