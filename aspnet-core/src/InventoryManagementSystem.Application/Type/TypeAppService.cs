using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using InventoryManagementSystem.Products;
using Microsoft.EntityFrameworkCore;
using Planner.Types.Dto;

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
                Name = typeDto.Name
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
                    Name = i.Name
                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<ResponseMessagesDto> DeleteAsync(long typeId)
        {
            await _typeRepository.DeleteAsync(new Type()
            {
                Id = typeId
            });

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
            var result = await _typeRepository.GetAll().Select(i => new TypeDto()
            {
                Id = i.Id,
                Name = i.Name,
                CreatorUserId = i.CreatorUserId,
                CreationTime = i.CreationTime,
                LastModificationTime = i.LastModificationTime
            }).ToListAsync();
            return result;
        }
        public async Task<PagedResultDto<TypeDto>> GetPaginatedAllAsync(PagedTypeResultRequestDto input)
        {
            var filteredTypes = _typeRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword));

            var pagedAndFilteredTypes = filteredTypes
                .OrderBy(i => i.Name)
                .PageBy(input);

            var totalCount = await pagedAndFilteredTypes.CountAsync();

            return new PagedResultDto<TypeDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredTypes.Select(i => new TypeDto()
                {
                    Id = i.Id,
                    Name = i.Name
                })
                    .ToListAsync());
        }
    }
}

