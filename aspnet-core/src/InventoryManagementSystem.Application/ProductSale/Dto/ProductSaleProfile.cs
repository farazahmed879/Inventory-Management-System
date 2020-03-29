using AutoMapper;
using InventoryManagementSystem.Shop;

namespace InventoryManagementSystem.ProductSales.Dto
{
    public class ProductSaleProfile : Profile
    {
        public ProductSaleProfile()
        {
            CreateMap<ProductSaleDto, ProductSell>().ReverseMap();
        }

    }
}
