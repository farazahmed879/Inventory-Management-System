using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.ProductSells.Dto;
using InventoryManagementSystem.Types.Dto;

namespace InventoryManagementSystem.ProductSells
{
    public interface IProductSellService : IApplicationService
    {
        Task<ResponseMessagesDto> CreateOrEditAsync(CreateProductSellDto productSellDto);

        Task<ProductSellDto> GetById(long productSellId);

        Task<ResponseMessagesDto> DeleteAsync(long productSellId);
        Task<List<ProductSellDto>> GetAll();

        Task<PagedResultDto<ProductSellDto>> GetPaginatedAllAsync(PagedProductSellResultRequestDto input);


        Task<List<ProductSaleGraphDto>> GetAllProductSale(string type, DateTime date);
    }
}
