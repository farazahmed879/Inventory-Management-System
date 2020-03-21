using Abp.Application.Services;
using InventoryManagementSystem.MultiTenancy.Dto;

namespace InventoryManagementSystem.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

