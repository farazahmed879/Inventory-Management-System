using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Products.Dto;

namespace InventoryManagementSystem.Products
{
    public class ProductService : AbpServiceBase, IProductService
    {
        private readonly IRepository<Product, long> _productRepository;
        public ProductService(IRepository<Product, long> productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<ResponseMessagesDto> CreateOrEditAsync(CreateProductDto productDto)
        {
            ResponseMessagesDto result;
            if (productDto.Id == 0)
            {
                result = await CreateProductAsync(productDto);
            }
            else
            {
                result = await UpdateProductAsync(productDto);
            }
            return result;
        }

        private async Task<ResponseMessagesDto> CreateProductAsync(CreateProductDto productDto)
        {
            var result = await _productRepository.InsertAsync(new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                ProductSubTypeId = productDto.SubTypeId
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

        private async Task<ResponseMessagesDto> UpdateProductAsync(CreateProductDto productDto)
        {
            var result = await _productRepository.UpdateAsync(new Product()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                ProductSubTypeId = productDto.SubTypeId
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
        public async Task<ProductDto> GetById(long productId)
        {
            var result = await _productRepository.GetAll()
                .Where(i => i.Id == productId)
                .Select(i =>
                new ProductDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    SubTypeId = i.ProductSubTypeId.Value
                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<ResponseMessagesDto> DeleteAsync(long productId)
        {
            var model = await _productRepository.GetAll().Where(i => i.Id == productId).FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _productRepository.UpdateAsync(model);

            return new ResponseMessagesDto()
            {
                Id = productId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<ProductDto>> GetAll()
        {
            var result = await _productRepository.GetAll().Where(i=> i.IsDeleted == false).Select(i => new ProductDto()
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                SubTypeId = i.ProductSubTypeId.Value,
                SubTypeName = i.ProductSubType.Name,
                CreatorUserId = i.CreatorUserId,
                CreationTime = i.CreationTime,
                LastModificationTime = i.LastModificationTime
            }).ToListAsync();
            return result;
        }
        public async Task<PagedResultDto<ProductDto>> GetPaginatedAllAsync(PagedProductResultRequestDto input)
        {
            var filteredProducts = _productRepository.GetAll()
                .Where(i => i.IsDeleted == false)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name),
                    x => x.Name.Contains(input.Name))
            .WhereIf(input.SubTypeId.HasValue, x=> x.ProductSubTypeId == input.SubTypeId);

            var pagedAndFilteredProducts = filteredProducts
                .OrderBy(i => i.Name)
                .PageBy(input);

            var totalCount = await pagedAndFilteredProducts.CountAsync();

            return new PagedResultDto<ProductDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredProducts.Select(i => new ProductDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    SubTypeId = i.ProductSubTypeId.Value,
                    SubTypeName = i.ProductSubType.Name
                })
                    .ToListAsync());
        }
    }
}

