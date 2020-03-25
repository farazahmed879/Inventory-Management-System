using AutoMapper;
using InventoryManagementSystem.Shop;

namespace InventoryManagementSystem.ShopProducts.Dto
{
    public class ShopProductProfile : Profile
    {
        public ShopProductProfile()
        {
            CreateMap<ShopProductDto, ShopProduct>().ReverseMap();
        }

    }
}
