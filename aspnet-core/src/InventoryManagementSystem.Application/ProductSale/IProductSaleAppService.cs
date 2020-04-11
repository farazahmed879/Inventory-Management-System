using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.ProductSales.Dto;
using InventoryManagementSystem.Types.Dto;

namespace InventoryManagementSystem.ProductSales
{
    public interface IProductSaleService : IApplicationService
    {
        Task<ResponseMessagesDto> CreateOrEditAsync(CreateProductSaleDto productSellDto);

        Task<ProductSaleDto> GetById(long productSellId);

        Task<ResponseMessagesDto> DeleteAsync(long productSellId);

        Task<List<ProductSaleDto>> GetAll(long? tenantId);

        Task<PagedResultDto<ProductSaleDto>> GetPaginatedAllAsync(PagedProductSaleResultRequestDto input);

    }
}
