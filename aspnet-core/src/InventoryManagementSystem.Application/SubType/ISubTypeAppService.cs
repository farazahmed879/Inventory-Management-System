using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.SubTypes.Dto;

namespace InventoryManagementSystem.SubTypes
{
    public interface ISubTypeService : IApplicationService
    {
        Task<ResponseMessagesDto> CreateOrEditAsync(CreateSubTypeDto typeDto);

        Task<SubTypeDto> GetById(long typeId);

        Task<ResponseMessagesDto> DeleteAsync(long typeId);
        Task<List<SubTypeDto>> GetAll();

        Task<PagedResultDto<SubTypeDto>> GetPaginatedAllAsync(PagedSubTypeResultRequestDto input);

        Task<PagedResultDto<TypeLookupTableDto>> GetAllQuestionTypeForLookupTable(GetAllForLookupTableInput input);
    }
}
