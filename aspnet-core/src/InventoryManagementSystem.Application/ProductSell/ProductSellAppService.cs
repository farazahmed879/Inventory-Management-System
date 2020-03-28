using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.ProductSells.Dto;
using InventoryManagementSystem.Shop;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.ProductSells
{
    public class ProductSellService : AbpServiceBase, IProductSellService
    {
        private readonly IRepository<ProductSell, long> _productSellRepository;
        public ProductSellService(IRepository<ProductSell, long> productSellRepository)
        {
            _productSellRepository = productSellRepository;
        }


        public async Task<ResponseMessagesDto> CreateOrEditAsync(CreateProductSellDto productSellDto)
        {
            ResponseMessagesDto result;
            if (productSellDto.Id == 0)
            {
                result = await CreateProductSellAsync(productSellDto);
            }
            else
            {
                result = await UpdateProductSellAsync(productSellDto);
            }
            return result;
        }

        private async Task<ResponseMessagesDto> CreateProductSellAsync(CreateProductSellDto productSellDto)
        {
            var result = await _productSellRepository.InsertAsync(new ProductSell()
            {
                Status = productSellDto.Status,
                ShopProductId = productSellDto.ShopProductId,
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

        private async Task<ResponseMessagesDto> UpdateProductSellAsync(CreateProductSellDto productSellDto)
        {
            var result = await _productSellRepository.UpdateAsync(new ProductSell()
            {
                Id = productSellDto.Id,
                Status = productSellDto.Status,
                ShopProductId = productSellDto.ShopProductId,
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
        public async Task<ProductSellDto> GetById(long productSellId)
        {
            var result = await _productSellRepository.GetAll()
                .Where(i => i.Id == productSellId)
                .Select(i =>
                new ProductSellDto()
                {
                    Id = i.Id,
                    Status = i.Status,
                    ShopProductId = i.ShopProductId

                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<ResponseMessagesDto> DeleteAsync(long productSellId)
        {
            await _productSellRepository.DeleteAsync(new ProductSell()
            {
                Id = productSellId
            });

            return new ResponseMessagesDto()
            {
                Id = productSellId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<ProductSellDto>> GetAll()
        {
            var result = await _productSellRepository.GetAll().Select(i => new ProductSellDto()
            {
                Id = i.Id,
                Status = i.Status,
                ProductName = i.ShopProduct.Product.Name,
                CompanyName = i.ShopProduct.Company.Name,
                ProductType = i.ShopProduct.Product.ProductSubType.Name,
                CreatorUserId = i.CreatorUserId,
                CreationTime = i.CreationTime,
                LastModificationTime = i.LastModificationTime
            }).ToListAsync();
            return result;
        }
        public async Task<PagedResultDto<ProductSellDto>> GetPaginatedAllAsync(PagedProductSellResultRequestDto input)
        {
           var filteredProductSells = _productSellRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Status), x => x.Status.Contains(input.Status));

            var pagedAndFilteredProductSells = filteredProductSells
                .OrderBy(i => i.Status)
                .PageBy(input);

            var totalCount = await pagedAndFilteredProductSells.CountAsync();

            return new PagedResultDto<ProductSellDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredProductSells.Select(i => new ProductSellDto()
                {
                    Id = i.Id,
                    ProductName = i.ShopProduct.Product.Name,
                    CompanyName = i.ShopProduct.Company.Name,
                    ProductType = i.ShopProduct.Product.ProductSubType.Name,
                    Profit = 0,
                    Status = i.Status,
                })
                    .ToListAsync());
        }
    }
}

