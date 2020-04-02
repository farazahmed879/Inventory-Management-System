using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.Dashboards.Dto;
using InventoryManagementSystem.ProductSales.Dto;
using InventoryManagementSystem.Types.Dto;

namespace InventoryManagementSystem.Dashboards
{
    public interface IDashboardService : IApplicationService
    {

        Task<DashboardListDto> GetDashboardList();
        Task<List<ProductSaleGraphDto>> GetProductGraph(string type, DateTime date);
    }
}
