using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.Companies.Dto;

namespace InventoryManagementSystem.Companies
{
    public interface ICompanyService : IApplicationService
    {
        Task<ResponseMessagesDto> CreateOrEditAsync(CreateCompanyDto companyDto);

        Task<CompanyDto> GetById(long companyId);

        Task<ResponseMessagesDto> DeleteAsync(long companyId);

        Task<List<CompanyDto>> GetAll(long? tenantId);

        Task<PagedResultDto<CompanyDto>> GetPaginatedAllAsync(PagedCompanyResultRequestDto input);
    }
}
