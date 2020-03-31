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
        private readonly InventoryManagementSystemDbContext _context;
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
                product.SellingRate = productSellDto.SellingRate;
                product.ShopProductId = productSellDto.ShopProductId;
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
                    SellingRate = i.SellingRate,
                    ShopProductId = i.ShopProductId

                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<ResponseMessagesDto> DeleteAsync(long productSellId)
        {
            await _productSaleRepository.DeleteAsync(new ProductSell()
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

        public async Task<List<ProductSaleDto>> GetAll()
        {
            var result = await _productSaleRepository.GetAll().Select(i => new ProductSaleDto()
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
        public async Task<PagedResultDto<ProductSaleDto>> GetPaginatedAllAsync(PagedProductSaleResultRequestDto input)
        {
            var filteredProductSells = _productSaleRepository.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Status), x => x.Status.Contains(input.Status));

            var pagedAndFilteredProductSells = filteredProductSells
                .OrderBy(i => i.Status)
                .PageBy(input);

            var totalCount = await pagedAndFilteredProductSells.CountAsync();

            return new PagedResultDto<ProductSaleDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredProductSells.Select(i => new ProductSaleDto()
                {
                    Id = i.Id,
                    SellingRate = i.SellingRate,
                    ProductName = i.ShopProduct.Product.Name,
                    CompanyName = i.ShopProduct.Company.Name,
                    ProductType = i.ShopProduct.Product.ProductSubType.Name,
                    Profit = i.SellingRate - i.ShopProduct.WholeSaleRate.Value,
                    Status = i.Status,
                })
                    .ToListAsync());
        }

        public async Task<List<ProductSaleGraphDto>> GetAllProductSale(string type, DateTime date)
        {
            var product = await _productSaleRepository.GetAll().Include(i => i.ShopProduct).ToListAsync();
            var productCost = await _shopProductRepository.GetAll().ToListAsync();
            var expense = await _expenseRepository.GetAll()
                            .ToListAsync();
            var result = new List<ProductSaleGraphDto>();
            switch (type)
            {
                case AppConsts.ThisWeek:
                    {
                        DayOfWeek Day = DateTime.Now.DayOfWeek;
                        int dayValue = 0;

                        switch (Day)
                        {
                            case DayOfWeek.Sunday:
                                {
                                    dayValue = -6;
                                    break;
                                }
                            case DayOfWeek.Monday:
                                {
                                    dayValue = -0;
                                    break;
                                }
                            case DayOfWeek.Tuesday:
                                {
                                    dayValue = -1;
                                    break;
                                }
                            case DayOfWeek.Wednesday:
                                {
                                    dayValue = -2;
                                    break;
                                }
                            case DayOfWeek.Thursday:
                                {
                                    dayValue = -3;
                                    break;
                                }
                            case DayOfWeek.Friday:
                                {
                                    dayValue = -4;
                                    break;
                                }
                            default:
                                {

                                    dayValue = 5;
                                    break;
                                }
                        }
                        DateTime WeekStartDate = DateTime.Now.AddDays(dayValue);
                        var dateList = new List<DateTime>();
                        dateList.Add(WeekStartDate);
                        for (var x = 1; x < 7; x++)
                        {
                            dateList.Add(WeekStartDate.AddDays(x));
                        }
                        foreach (var item in dateList)
                        {
                            var sale = new ProductSaleGraphDto();
                            sale.Label = item.DayOfWeek.ToString();
                            sale.Sale = product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.SellingRate);
                            sale.Profit = product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.SellingRate) -
                                product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.ShopProduct.WholeSaleRate.Value) > 0 ?
                                product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.SellingRate) -
                                product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.ShopProduct.WholeSaleRate.Value) : 0;
                            sale.Expense = expense.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.Cost);
                            sale.ProductCost = productCost.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.WholeSaleRate.Value);
                            result.Add(sale);
                        }
                        return result;
                    }
                case AppConsts.ThisMonth:
                    {

                        DateTime now = DateTime.Now;
                        var startDate = new DateTime(now.Year, now.Month, 1);
                        var endDate = startDate.AddMonths(1).AddDays(-1);


                        var dateList = new List<DateTime>();
                        dateList.Add(startDate);
                        for (var x = startDate.Day; x < endDate.Day; x++)
                        {
                            dateList.Add(startDate.AddDays(x));
                        }

                        foreach (var item in dateList)
                        {
                            var sale = new ProductSaleGraphDto();
                            sale.Label = item.Day.ToString();
                            sale.Text = item.DayOfWeek.ToString();
                            sale.Sale = product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.SellingRate);
                            sale.Profit = product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.SellingRate) -
                                product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.ShopProduct.WholeSaleRate.Value) > 0 ?
                                product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.SellingRate) -
                                product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.ShopProduct.WholeSaleRate.Value) : 0;
                            sale.Expense = expense.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.Cost);
                            result.Add(sale);
                            sale.ProductCost = productCost.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.WholeSaleRate.Value);
                        }
                        return result;
                    }
                case AppConsts.ThisYear:
                    {
                        var months = new List<string>() { "January", "February", "March", "April",
                            "May", "June", "July","August","September", "October","Novermber","December"};
                        var monthList = new List<GraphHelperDto>();
                        for (var x = 1; x <= 7; x++)
                        {
                            var day = new GraphHelperDto();
                            day.Label = months[x - 1];
                            day.Value = x;

                            monthList.Add(day);
                        }

                        foreach (var month in monthList)
                        {
                            var sale = new ProductSaleGraphDto();
                            sale.Label = month.Label;
                            sale.Sale = product.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.SellingRate);
                            sale.Profit = product.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.SellingRate) -
                                product.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.ShopProduct.WholeSaleRate.Value) > 0 ?
                                product.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.SellingRate) -
                                product.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.ShopProduct.WholeSaleRate.Value) : 0;
                            sale.Expense = expense.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.Cost);
                            result.Add(sale);
                            sale.ProductCost = productCost.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.WholeSaleRate.Value);
                            result.Add(sale);
                        }
                        return result;
                    }
                case AppConsts.AllYear:
                    {
                        var startYear = product.OrderByDescending(i => i.CreationTime).Select(i => i.CreationTime.Year).FirstOrDefault();
                        var currentYear = DateTime.Now.Year;
                        var yearList = new List<int>();
                        for (var x = startYear; x <= currentYear; x++)
                        {
                            yearList.Add(x);
                        }
                        foreach (var year in yearList)
                        {
                            var sale = new ProductSaleGraphDto();
                            sale.Label = year.ToString();
                            sale.Sale = product.Where(i => i.CreationTime.Year == year).Sum(i => i.SellingRate);
                            sale.Profit = product.Where(i => i.CreationTime.Year == year).Sum(i => i.SellingRate) -
                                product.Where(i => i.CreationTime.Year == year).Sum(i => i.ShopProduct.WholeSaleRate.Value) > 0 ?
                                product.Where(i => i.CreationTime.Year == year).Sum(i => i.SellingRate) -
                                product.Where(i => i.CreationTime.Year == year).Sum(i => i.ShopProduct.WholeSaleRate.Value) : 0;
                            sale.Expense = expense.Where(i => i.CreationTime.Year == year).Sum(i => i.Cost);
                            result.Add(sale);
                            sale.ProductCost = productCost.Where(i => i.CreationTime.Year == year).Sum(i => i.WholeSaleRate.Value);
                            result.Add(sale);
                        }
                        return result;
                    }
                default:
                    {
                        break;
                    }
            }

            return null;
        }
    }
}

