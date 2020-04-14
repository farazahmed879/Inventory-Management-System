using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.Types.Dto;

namespace InventoryManagementSystem.Types
{
    public interface ITypeService : IApplicationService
    {
        Task<ResponseMessagesDto> CreateOrEditAsync(CreateTypeDto typeDto);

        Task<TypeDto> GetById(long typeId);

        Task<ResponseMessagesDto> DeleteAsync(long typeId);
        Task<List<TypeDto>> GetAll(long? tenentId);

        Task<PagedResultDto<TypeDto>> GetPaginatedAllAsync(PagedTypeResultRequestDto input);
    }
}
