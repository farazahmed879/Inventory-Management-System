using System;
using System.Collections.Generic;
using System.Globalization;
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
                SellingRate = productSellDto.SellingRate,
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
        public async Task<ProductSellDto> GetById(long productSellId)
        {
            var result = await _productSellRepository.GetAll()
                .Where(i => i.Id == productSellId)
                .Select(i =>
                new ProductSellDto()
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
            var product = await _productSellRepository.GetAll().ToListAsync();
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
                        //DateTime WeekSecondDate = WeekStartDate.AddDays(1);
                        //DateTime WeekThirdDate = WeekStartDate.AddDays(2);
                        //DateTime WeekFourthDate = WeekStartDate.AddDays(3);
                        //DateTime WeekFifthDate = WeekStartDate.AddDays(4);
                        //DateTime WeekSixthDate = WeekStartDate.AddDays(5);
                        //DateTime WeekEndDate = WeekStartDate.AddDays(6);
                        var dateList = new List<DateTime>();
                        dateList.Add(WeekStartDate);
                        for (var x = 1; x < 7; x++)
                        {
                            dateList.Add(WeekStartDate.AddDays(x));
                        }
                        var dailySaleForThisWeek = product.Where(i => i.CreationTime.Date >= WeekStartDate.Date && i.CreationTime.Date <= dateList[6].Date);
                        foreach (var item in dateList)
                        {
                            var sale = new ProductSaleGraphDto();
                            sale.Label = item.DayOfWeek.ToString();
                            sale.Value = dailySaleForThisWeek.Where(i => i.CreationTime == item).Sum(i => i.SellingRate);
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

                        var days = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                        foreach (var item in dateList)
                        {
                            var sale = new ProductSaleGraphDto();
                            sale.Label = item.DayOfWeek.ToString();
                            sale.Value = product.Where(i => i.CreationTime == item).Sum(i => i.SellingRate);
                            result.Add(sale);
                        }
                        return result;
                    }
                case AppConsts.ThisYear:
                    {
                        var months = new List<string>() { "January", "February", "March", "April", 
                            "May", "June", "July","August","September", "October","Novermber","December"};
                        foreach (var month in months)
                        {
                            var sale = new ProductSaleGraphDto();
                            sale.Label = month;
                            sale.Value = product.Where(i => i.CreationTime.Month.ToString() == month).Sum(i => i.SellingRate);
                            result.Add(sale);
                        }
                        return result;
                    }
                case AppConsts.AllYear:
                    {
                        var startYear = product.OrderByDescending(i => i.CreationTime).Select(i => i.CreationTime.Year).FirstOrDefault();
                        var currentYear = DateTime.Now.Year;
                        var yearList = new List<int>();
                        for(var x = startYear; x <= currentYear; x++)
                        {
                            yearList.Add(x);
                        }
                        foreach (var year in yearList)
                        {
                            var sale = new ProductSaleGraphDto();
                            sale.Label = year.ToString();
                            sale.Value = product.Where(i => i.CreationTime.Year == year).Sum(i => i.SellingRate); ;
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

