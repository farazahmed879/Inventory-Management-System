using System.Threading.Tasks;
using Abp.Application.Services;
using InventoryManagementSystem.Authorization.Accounts.Dto;

namespace InventoryManagementSystem.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
