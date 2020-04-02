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

namespace InventoryManagementSystem.Dashboards
{
    public class DashboardService : AbpServiceBase, IDashboardService
    {
        private readonly IRepository<ProductSell, long> _productSaleRepository;
        private readonly IRepository<ShopProduct, long> _shopProductRepository;
        private readonly IRepository<Expense, long> _expenseRepository;
        private readonly InventoryManagementSystemDbContext _context;
        public DashboardService(
            IRepository<ProductSell, long> productSaleRepository,
            IRepository<ShopProduct, long> shopProductRepository,
            IRepository<Expense, long> expenseRepository
            )
        {
            _productSaleRepository = productSaleRepository;
            _shopProductRepository = shopProductRepository;
            _expenseRepository = expenseRepository;
        }


        public async Task<List<ProductSaleGraphDto>> GetProductGraph(string type, DateTime date)
        {
            var product = await _productSaleRepository.GetAll()
               .Include(i => i.ShopProduct)
               .ToListAsync();
            var productCost = await _shopProductRepository.GetAll().ToListAsync();
            var expense = await _expenseRepository.GetAll()
                            .ToListAsync();

            switch (type)
            {
                case AppConsts.ThisWeek:
                    {
                        return ThisWeek(product, productCost, expense);


                    }
                case AppConsts.ThisMonth:
                    {

                        return ThisMonth(product, productCost, expense);

                    }
                case AppConsts.ThisYear:
                    {
                        return ThisYear(product, productCost, expense);
                    }
                case AppConsts.AllYear:
                    {
                        return AllYear(product, productCost, expense);
                    }
                default:
                    {
                        return null;
                    }
            }


        }




        public async Task<DashboardListDto> GetDashboardList()
        {
            var dashboardList = new DashboardListDto();
            var product = await _productSaleRepository.GetAll()
              .Include(i => i.ShopProduct)
              .ToListAsync();
            var productCost = await _shopProductRepository.GetAll().ToListAsync();
            var expense = await _expenseRepository.GetAll().ToListAsync();


            var thisWeek = ThisWeek(product, productCost, expense);
            var thisMonth = ThisMonth(product, productCost, expense);
            var thisYear = ThisYear(product, productCost, expense);
            var today = Today(product, productCost, expense);
            var yesterday = Yesterday(product, productCost, expense);

            var sale = new SaleDto();
            sale.Today = today.Sale;
            sale.Yesterday = yesterday.Sale;
            sale.ThisWeek = thisWeek.Sum(i => i.Sale);
            sale.ThisMonth = thisMonth.Sum(i => i.Sale);
            sale.ThisYear = thisYear.Sum(i => i.Sale);

            var costing = new SaleDto();
            costing.Today = today.ProductCost; ;
            costing.Yesterday = yesterday.ProductCost;
            costing.ThisWeek = thisWeek.Sum(i => i.ProductCost);
            costing.ThisMonth = thisMonth.Sum(i => i.ProductCost);
            costing.ThisYear = thisYear.Sum(i => i.ProductCost);

            var expenses = new SaleDto();
            expenses.Today = today.Expense; ;
            expenses.Yesterday = yesterday.Expense;
            expenses.ThisWeek = thisWeek.Sum(i => i.Expense);
            expenses.ThisMonth = thisMonth.Sum(i => i.Expense);
            expenses.ThisYear = thisYear.Sum(i => i.Expense);

            var profit = new SaleDto();
            profit.Today = today.Profit; ;
            profit.Yesterday = yesterday.Profit;
            profit.ThisWeek = thisWeek.Sum(i => i.Profit);
            profit.ThisMonth = thisMonth.Sum(i => i.Profit);
            profit.ThisYear = thisYear.Sum(i => i.Profit);

            dashboardList.Sale = sale;
            dashboardList.Costing = costing;
            dashboardList.Expenses = expenses;
            dashboardList.Profit = profit;
            return dashboardList;

        }

        private ProductSaleGraphDto Today(List<ProductSell> product, List<ShopProduct> productCost, List<Expense> expense)
        {
            var today = DateTime.Now.Date;

            var result = new ProductSaleGraphDto();
            result.Sale = product.Where(i => i.CreationTime.Date == today).Sum(i => i.SellingRate);
            result.Profit = product.Where(i => i.CreationTime.Date == today).Sum(i => i.SellingRate) -
                product.Where(i => i.CreationTime.Date == today).Sum(i => i.ShopProduct.WholeSaleRate.Value) ;
            result.Expense = expense.Where(i => i.CreationTime.Date == today).Sum(i => i.Cost);
            result.ProductCost = productCost.Where(i => i.CreationTime.Date == today).Sum(i => i.WholeSaleRate.Value);
            return result;
        }

        private ProductSaleGraphDto Yesterday(List<ProductSell> product, List<ShopProduct> productCost, List<Expense> expense)
        {
            var yesterday = DateTime.Today.AddDays(-1).Date;

            var result = new ProductSaleGraphDto();
            result.Sale = product.Where(i => i.CreationTime.Date == yesterday).Sum(i => i.SellingRate);
            result.Profit = product.Where(i => i.CreationTime.Date == yesterday).Sum(i => i.SellingRate) -
                product.Where(i => i.CreationTime.Date == yesterday).Sum(i => i.ShopProduct.WholeSaleRate.Value); ;
            result.Expense = expense.Where(i => i.CreationTime.Date == yesterday).Sum(i => i.Cost);
            result.ProductCost = productCost.Where(i => i.CreationTime.Date == yesterday).Sum(i => i.WholeSaleRate.Value);
            return result;
        }


        private List<ProductSaleGraphDto> ThisWeek(List<ProductSell> product, List<ShopProduct> productCost, List<Expense> expense)
        {


            var result = new List<ProductSaleGraphDto>();
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
                    product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.ShopProduct.WholeSaleRate.Value); ;
                sale.Expense = expense.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.Cost);
                sale.ProductCost = productCost.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.WholeSaleRate.Value);
                result.Add(sale);
            }
            return result;

        }

        private List<ProductSaleGraphDto> ThisMonth(List<ProductSell> product, List<ShopProduct> productCost, List<Expense> expense)
        {
            var result = new List<ProductSaleGraphDto>();
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
                    product.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.ShopProduct.WholeSaleRate.Value);
                sale.Expense = expense.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.Cost);
                result.Add(sale);
                sale.ProductCost = productCost.Where(i => i.CreationTime.Date == item.Date).Sum(i => i.WholeSaleRate.Value);
            }
            return result;
        }

        private List<ProductSaleGraphDto> ThisYear(List<ProductSell> product, List<ShopProduct> productCost, List<Expense> expense)
        {
            var result = new List<ProductSaleGraphDto>();
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
                    product.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.ShopProduct.WholeSaleRate.Value);
                sale.Expense = expense.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.Cost);
                result.Add(sale);
                sale.ProductCost = productCost.Where(i => i.CreationTime.Month == month.Value).Sum(i => i.WholeSaleRate.Value);
                result.Add(sale);
            }
            return result;
        }

        private List<ProductSaleGraphDto> AllYear(List<ProductSell> product, List<ShopProduct> productCost, List<Expense> expense)
        {
            var result = new List<ProductSaleGraphDto>();
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
                    product.Where(i => i.CreationTime.Year == year).Sum(i => i.ShopProduct.WholeSaleRate.Value);
                sale.Expense = expense.Where(i => i.CreationTime.Year == year).Sum(i => i.Cost);
                result.Add(sale);
                sale.ProductCost = productCost.Where(i => i.CreationTime.Year == year).Sum(i => i.WholeSaleRate.Value);
                result.Add(sale);
            }
            return result;

        }
    }
}

