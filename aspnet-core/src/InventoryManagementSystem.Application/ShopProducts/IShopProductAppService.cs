using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.ShopProducts.Dto;

namespace InventoryManagementSystem.ShopProducts
{
    public interface IShopProductService : IApplicationService
    {
        Task<ResponseMessagesDto> CreateOrEditAsync(CreateShopProductDto shopProductDto);

        Task<ShopProductDto> GetById(long shopProductId);

        Task<ResponseMessagesDto> DeleteAsync(long shopProductId);

        Task<List<ShopProductDto>> GetAll(long? tenantId);

        Task<PagedResultDto<ShopProductDto>> GetPaginatedAllAsync(PagedShopProductResultRequestDto input);
    }
}
