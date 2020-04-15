using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.Companies.Dto;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Companies
{
    public class CompanyService : AbpServiceBase, ICompanyService
    {
        private readonly IRepository<Company, long> _companyRepository;
        public CompanyService(IRepository<Company, long> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<ResponseMessagesDto> CreateOrEditAsync(CreateCompanyDto companyDto)
        {
            ResponseMessagesDto result;
            if (companyDto.Id == 0)
            {
                result = await CreateCompanyAsync(companyDto);
            }
            else
            {
                result = await UpdateCompanyAsync(companyDto);
            }
            return result;
        }

        private async Task<ResponseMessagesDto> CreateCompanyAsync(CreateCompanyDto companyDto)
        {
            var result = await _companyRepository.InsertAsync(new Company()
            {
                Name = companyDto.Name,
                Description = companyDto.Description,
                TenantId = companyDto.TenantId
            });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (result.Id != 0)
            {
                return new ResponseMessagesDto()
                {
                    Id = result.Id,
                    SuccessMessage = AppConsts.SuccessfullyInserted,
                    Success = true,
                    Error = false,
                };
            }
            return new ResponseMessagesDto()
            {
                Id = 0,
                ErrorMessage = AppConsts.InsertFailure,
                Success = false,
                Error = true,
            };
        }

        private async Task<ResponseMessagesDto> UpdateCompanyAsync(CreateCompanyDto companyDto)
        {
            var result = await _companyRepository.UpdateAsync(new Company()
            {
                Id = companyDto.Id,
                Name = companyDto.Name,
                Description = companyDto.Description,

            });

            if (result != null)
            {
                return new ResponseMessagesDto()
                {
                    Id = result.Id,
                    SuccessMessage = AppConsts.SuccessfullyUpdated,
                    Success = true,
                    Error = false,
                };
            }
            return new ResponseMessagesDto()
            {
                Id = 0,
                ErrorMessage = AppConsts.UpdateFailure,
                Success = false,
                Error = true,
            };
        }

        public async Task<CompanyDto> GetById(long companyId)
        {
            var result = await _companyRepository.GetAll()
                .Where(i => i.Id == companyId)
                .Select(i =>
                new CompanyDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description
                })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessagesDto> DeleteAsync(long companyId)
        {


            var model = await _companyRepository.GetAll().Where(i => i.Id == companyId).FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _companyRepository.UpdateAsync(model);

            return new ResponseMessagesDto()
            {
                Id = companyId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<CompanyDto>> GetAll(long? tenantId)
        {
            var result = await _companyRepository.GetAll().Where(i => i.IsDeleted == false && i.TenantId == tenantId).Select(i => new CompanyDto()
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                CreatorUserId = i.CreatorUserId,
                CreationTime = i.CreationTime,
                LastModificationTime = i.LastModificationTime
            }).ToListAsync();
            return result;
        }
        
        public async Task<PagedResultDto<CompanyDto>> GetPaginatedAllAsync(PagedCompanyResultRequestDto input)
        {
            var filteredCompanys = _companyRepository.GetAll()
                //.Where(i => i.IsDeleted == false && (input.TenantId == null || i.TenantId == input.TenantId))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), x => x.Name.Contains(input.Name));

            var pagedAndFilteredCompanys = filteredCompanys
                .OrderBy(i => i.Name)
                .PageBy(input);

            var totalCount = filteredCompanys.Count();

            return new PagedResultDto<CompanyDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredCompanys.Where(i=> i.IsDeleted == false).Select(i => new CompanyDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    TenantId = i.TenantId
                    
                })
                    .ToListAsync());
        }
    }
}

