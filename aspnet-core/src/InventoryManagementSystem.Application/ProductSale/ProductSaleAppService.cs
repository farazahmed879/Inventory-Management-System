using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using InventoryManagementSystem.Dashboards.Dto;
using InventoryManagementSystem.EntityFrameworkCore;
using InventoryManagementSystem.Expenses;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.ProductSales.Dto;
using InventoryManagementSystem.Shop;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.ProductSales
{
    public class ProductSaleService : AbpServiceBase, IProductSaleService
    {
        private readonly IRepository<ProductSell, long> _productSaleRepository;
        private readonly IRepository<ShopProduct, long> _shopProductRepository;
        private readonly IRepository<Expense, long> _expenseRepository;

        public ProductSaleService(
            IRepository<ProductSell, long> productSaleRepository,
            IRepository<ShopProduct, long> shopProductRepository,
            IRepository<Expense, long> expenseRepository
            )
        {
            _productSaleRepository = productSaleRepository;
            _shopProductRepository = shopProductRepository;
            _expenseRepository = expenseRepository;
        }


        public async Task<ResponseMessagesDto> CreateOrEditAsync(CreateProductSaleDto productSellDto)
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

        private async Task<ResponseMessagesDto> CreateProductSellAsync(CreateProductSaleDto productSellDto)
        {
            var productList = new List<ProductSell>();
            for (var x = 0; x < productSellDto.Quantity; x++)
            {
                var product = new ProductSell();
                product.Status = productSellDto.Status;
                product.Description = productSellDto.Description;
                product.SellingRate = productSellDto.SellingRate;
                product.ShopProductId = productSellDto.ShopProductId;
                product.TenantId = productSellDto.TenantId;
                productList.Add(product);
            }
            await _productSaleRepository.GetDbContext().AddRangeAsync(productList);

            //var result = await _productSaleRepository.InsertAsync(new ProductSell()
            //{
            //    Status = productSellDto.Status,
            //    SellingRate = productSellDto.SellingRate,
            //    ShopProductId = productSellDto.ShopProductId,
            //});


            await UnitOfWorkManager.Current.SaveChangesAsync();

            var shopProduct = await _shopProductRepository.GetAll()
                .Where(i => i.Id == productSellDto.ShopProductId && i.Quantity > 0)
                .SingleOrDefaultAsync();
            shopProduct.Quantity = shopProduct.Quantity - productSellDto.Quantity;
            await _shopProductRepository.UpdateAsync(shopProduct);

            if (productList[0].Id != 0)
            {
                return new ResponseMessagesDto()
                {
                    Id = productList[0].Id,
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

        private async Task<ResponseMessagesDto> UpdateProductSellAsync(CreateProductSaleDto productSellDto)
        {
            var result = await _productSaleRepository.UpdateAsync(new ProductSell()
            {
                Id = productSellDto.Id,
                Status = productSellDto.Status,
                Description = productSellDto.Description,
                SellingRate = productSellDto.SellingRate,
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

        public async Task<ProductSaleDto> GetById(long productSellId)
        {
            var result = await _productSaleRepository.GetAll()
                .Where(i => i.Id == productSellId)
                .Select(i =>
                new ProductSaleDto()
                {
                    Id = i.Id,
                    Status = i.Status,
                    Description = i.Description,
                    SellingRate = i.SellingRate,
                    ShopProductId = i.ShopProductId

                })
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<ResponseMessagesDto> DeleteAsync(long productSellId)
        {
            var model = await _productSaleRepository.GetAll().Where(i => i.Id == productSellId).FirstOrDefaultAsync();
            model.IsDeleted = true;
            var result = await _productSaleRepository.UpdateAsync(model);

            return new ResponseMessagesDto()
            {
                Id = productSellId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<ProductSaleDto>> GetAll(long? tenantId)
        {
            var result = await _productSaleRepository.GetAll()
                .Where(i=> i.IsDeleted == false && i.TenantId == tenantId).Select(i => new ProductSaleDto()
            {
                Id = i.Id,
                Status = i.Status,
                Description = i.Description,
                ProductName = i.ShopProduct.Product.Name,
                CompanyName = i.ShopProduct.Company.Name,
                ProductType = i.ShopProduct.Product.ProductSubType.Name,
                CreatorUserId = i.CreatorUserId,
                CreationTime = i.CreationTime,
                LastModificationTime = i.LastModificationTime
            }).ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<ProductSaleDto>> GetPaginatedAllAsync(PagedProductSaleResultRequestDto input)
        {
            var filteredProductSells = _productSaleRepository.GetAll()
                 .Where(i => i.IsDeleted == false && (!input.TenantId.HasValue || i.TenantId == input.TenantId))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Status), x => x.Status.Contains(input.Status));

            var pagedAndFilteredProductSells = filteredProductSells
                .OrderByDescending(i => i.Id)
                .PageBy(input);

            var totalCount = await pagedAndFilteredProductSells.CountAsync();

            return new PagedResultDto<ProductSaleDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredProductSells.Select(i => new ProductSaleDto()
                {
                    Id = i.Id,
                    SellingRate = i.SellingRate,
                    Description = i.Description,
                    ProductName = i.ShopProduct.Product.Name,
                    CompanyName = i.ShopProduct.Company.Name,
                    ProductType = i.ShopProduct.Product.ProductSubType.Name,
                    Profit = i.SellingRate - i.ShopProduct.WholeSaleRate.Value,
                    Status = i.Status,
                })
                    .ToListAsync());
        }

    }
}

