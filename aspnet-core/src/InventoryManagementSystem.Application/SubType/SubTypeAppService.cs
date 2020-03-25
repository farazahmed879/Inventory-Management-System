using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.SubTypes.Dto;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.SubTypes
{
    public class SubTypeService : AbpServiceBase, ISubTypeService
    {
        private readonly IRepository<SubType, long> _subTypeRepository;
        public SubTypeService(IRepository<SubType, long> subTypeRepository)
        {
            _subTypeRepository = subTypeRepository;
        }


        public async Task<ResponseMessagesDto> CreateOrEditAsync(CreateSubTypeDto subTypeDto)
        {
            ResponseMessagesDto result;
            if (subTypeDto.Id == 0)
            {
                result = await CreateSubTypeAsync(subTypeDto);
            }
            else
            {
                result = await UpdateSubTypeAsync(subTypeDto);
            }
            return result;
        }

        private async Task<ResponseMessagesDto> CreateSubTypeAsync(CreateSubTypeDto subTypeDto)
        {
            var result = await _subTypeRepository.InsertAsync(new SubType()
            {
                Name = subTypeDto.Name
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

        private async Task<ResponseMessagesDto> UpdateSubTypeAsync(CreateSubTypeDto subTypeDto)
        {
            var result = await _subTypeRepository.UpdateAsync(new SubType()
            {
                Id = subTypeDto.Id,
                Name = subTypeDto.Name
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
        public async Task<SubTypeDto> GetById(long subTypeId)
        {
            var result = await _subTypeRepository.GetAll()
                .Where(i => i.Id == subTypeId)
                .Select(i =>
                new SubTypeDto()
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<ResponseMessagesDto> DeleteAsync(long subTypeId)
        {
            await _subTypeRepository.DeleteAsync(new SubType()
            {
                Id = subTypeId
            });

            return new ResponseMessagesDto()
            {
                Id = subTypeId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<SubTypeDto>> GetAll()
        {
            var result = await _subTypeRepository.GetAll().Select(i => new SubTypeDto()
            {
                Id = i.Id,
                Name = i.Name,
                CreatorUserId = i.CreatorUserId,
                CreationTime = i.CreationTime,
                LastModificationTime = i.LastModificationTime
            }).ToListAsync();
            return result;
        }
        public async Task<PagedResultDto<SubTypeDto>> GetPaginatedAllAsync(PagedSubTypeResultRequestDto input)
        {
            var filteredSubTypes = _subTypeRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), x => x.Name.Contains(input.Name));

            var pagedAndFilteredTypes = filteredSubTypes
                .OrderBy(i => i.Name)
                .PageBy(input);

            var totalCount = await pagedAndFilteredTypes.CountAsync();

            return new PagedResultDto<SubTypeDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredTypes.Select(i => new SubTypeDto()
                {
                    Id = i.Id,
                    Name = i.Name
                })
                    .ToListAsync());
        }
    }
}

