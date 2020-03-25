using AutoMapper;
using InventoryManagementSystem.Shop;

namespace InventoryManagementSystem.ProductSells.Dto
{
    public class ProductSellProfile : Profile
    {
        public ProductSellProfile()
        {
            CreateMap<ProductSellDto, ProductSell>().ReverseMap();
        }

    }
}
