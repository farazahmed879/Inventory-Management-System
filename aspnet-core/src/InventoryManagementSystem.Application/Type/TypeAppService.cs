using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.Types.Dto;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Types
{
    public class TypeService : AbpServiceBase, ITypeService
    {
        private readonly IRepository<Type, long> _typeRepository;
        public TypeService(IRepository<Type, long> typeRepository)
        {
            _typeRepository = typeRepository;
        }


        public async Task<ResponseMessagesDto> CreateOrEditAsync(CreateTypeDto typeDto)
        {
            ResponseMessagesDto result;
            if (typeDto.Id == 0)
            {
                result = await CreateTypeAsync(typeDto);
            }
            else
            {
                result = await UpdateTypeAsync(typeDto);
            }
            return result;
        }

        private async Task<ResponseMessagesDto> CreateTypeAsync(CreateTypeDto typeDto)
        {
            var result = await _typeRepository.InsertAsync(new Type()
            {
                Name = typeDto.Name,
                Description = typeDto.Description,
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

        private async Task<ResponseMessagesDto> UpdateTypeAsync(CreateTypeDto typeDto)
        {
            var result = await _typeRepository.UpdateAsync(new Type()
            {
                Id = typeDto.Id,
                Description = typeDto.Description,
                Name = typeDto.Name
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
        public async Task<TypeDto> GetById(long typeId)
        {
            var result = await _typeRepository.GetAll()
                .Where(i => i.Id == typeId)
                .Select(i =>
                new TypeDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<ResponseMessagesDto> DeleteAsync(long typeId)
        {
            var model = await _typeRepository.GetAll().Where(i => i.Id == typeId).FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _typeRepository.UpdateAsync(model);

            return new ResponseMessagesDto()
            {
                Id = typeId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<TypeDto>> GetAll()
        {
            var result = await _typeRepository.GetAll()
                .Where(i=> i.IsDeleted == false).Select(i => new TypeDto()
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
        public async Task<PagedResultDto<TypeDto>> GetPaginatedAllAsync(PagedTypeResultRequestDto input)
        {
            var filteredTypes = _typeRepository.GetAll()
                .Where(i => i.IsDeleted == false && i.TenantId == input.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), x => x.Name.Contains(input.Name));

            var pagedAndFilteredTypes = filteredTypes
                .OrderBy(i => i.Name)
                .PageBy(input);

            var totalCount = await pagedAndFilteredTypes.CountAsync();

            return new PagedResultDto<TypeDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredTypes.Select(i => new TypeDto()
                {
                    Id = i.Id,
                    Description = i.Description,
                    Name = i.Name
                })
                    .ToListAsync());
        }
    }
}

