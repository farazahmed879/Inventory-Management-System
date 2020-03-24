using AutoMapper;

namespace InventoryManagementSystem.Products.Dto
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }

    }
}
