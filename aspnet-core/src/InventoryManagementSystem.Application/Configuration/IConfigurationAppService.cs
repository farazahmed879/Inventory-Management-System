using System.Threading.Tasks;
using InventoryManagementSystem.Configuration.Dto;

namespace InventoryManagementSystem.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
