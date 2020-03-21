using System.Threading.Tasks;
using Abp.Application.Services;
using InventoryManagementSystem.Sessions.Dto;

namespace InventoryManagementSystem.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
