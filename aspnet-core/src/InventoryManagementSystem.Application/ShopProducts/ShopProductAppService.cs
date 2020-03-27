using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.Shop;
using InventoryManagementSystem.ShopProducts.Dto;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.ShopProducts
{
    public class ShopProductService : AbpServiceBase, IShopProductService
    {
        private readonly IRepository<ShopProduct, long> _shopProductRepository;
        public ShopProductService(IRepository<ShopProduct, long> shopProductRepository)
        {
            _shopProductRepository = shopProductRepository;
        }


        public async Task<ResponseMessagesDto> CreateOrEditAsync(CreateShopProductDto shopProductDto)
        {
            ResponseMessagesDto result;
            if (shopProductDto.Id == 0)
            {
                result = await CreateShopProductAsync(shopProductDto);
            }
            else
            {
                result = await UpdateShopProductAsync(shopProductDto);
            }
            return result;
        }

        private async Task<ResponseMessagesDto> CreateShopProductAsync(CreateShopProductDto shopProductDto)
        {
            var result = await _shopProductRepository.InsertAsync(new ShopProduct()
            {
                WholeSaleRate = shopProductDto.WholeSaleRate,
                RetailPrice = shopProductDto.RetailPrice,
                Quantity = shopProductDto.Quantity,
                CompanyRate = shopProductDto.CompanyRate,
                ProductId = shopProductDto.ProductId,
                CompanyId = shopProductDto.CompanyId,

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

        private async Task<ResponseMessagesDto> UpdateShopProductAsync(CreateShopProductDto shopProductDto)
        {
            var result = await _shopProductRepository.UpdateAsync(new ShopProduct()
            {
                Id = shopProductDto.Id,
                WholeSaleRate = shopProductDto.WholeSaleRate,
                Quantity = shopProductDto.Quantity,
                RetailPrice = shopProductDto.RetailPrice,
                ProductId = shopProductDto.ProductId,
                CompanyId = shopProductDto.CompanyId,
                CompanyRate = shopProductDto.CompanyRate
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
        public async Task<ShopProductDto> GetById(long shopProductId)
        {
            var result = await _shopProductRepository.GetAll()
                .Where(i => i.Id == shopProductId)
                .Select(i =>
                new ShopProductDto()
                {
                    Id = i.Id,
                    WholeSaleRate = i.WholeSaleRate.Value,
                    RetailPrice = i.RetailPrice,
                    CompanyRate = i.CompanyRate,
                    CompanyId = i.CompanyId,
                    ProductId = i.ProductId
                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<ResponseMessagesDto> DeleteAsync(long shopProductId)
        {
            await _shopProductRepository.DeleteAsync(new ShopProduct()
            {
                Id = shopProductId
            });

            return new ResponseMessagesDto()
            {
                Id = shopProductId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<ShopProductDto>> GetAll()
        {
            var result = await _shopProductRepository.GetAll().Select(i => new ShopProductDto()
            {
                Id = i.Id,
                ProductName = i.Product.Name,
                CompanyName = i.Company.Name,
                CreatorUserId = i.CreatorUserId,
                CreationTime = i.CreationTime,
                LastModificationTime = i.LastModificationTime
            }).ToListAsync();
            return result;
        }
        public async Task<PagedResultDto<ShopProductDto>> GetPaginatedAllAsync(PagedShopProductResultRequestDto input)
        {
            var filteredShopProducts = _shopProductRepository.GetAll();

            //.WhereIf(!string.IsNullOrWhiteSpace(input.Name), x => x.Product.Name.Contains(input.Name));

            var pagedAndFilteredShopProducts = filteredShopProducts
                .OrderBy(i => i.Id)
                .PageBy(input);

            var totalCount = await pagedAndFilteredShopProducts.CountAsync();

            return new PagedResultDto<ShopProductDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredShopProducts.Select(i => new ShopProductDto()
                {
                    Id = i.Id,
                    ProductName = i.Product.Name,
                    CompanyName = i.Company.Name,
                    WholeSaleRate = i.WholeSaleRate.Value,
                    Quantity = i.Quantity.Value,
                    RetailPrice = i.RetailPrice

                })
                    .ToListAsync());
        }
    }
}

