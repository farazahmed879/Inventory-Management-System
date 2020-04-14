using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.Products.Dto;

namespace InventoryManagementSystem.Products
{
    public interface IProductService : IApplicationService
    {
        Task<ResponseMessagesDto> CreateOrEditAsync(CreateProductDto typeDto);

        Task<ProductDto> GetById(long typeId);

        Task<ResponseMessagesDto> DeleteAsync(long typeId);

        Task<List<ProductDto>> GetAll(long? tenantId);

        Task<PagedResultDto<ProductDto>> GetPaginatedAllAsync(PagedProductResultRequestDto input);
    }
}
