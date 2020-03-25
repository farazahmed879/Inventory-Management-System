using AutoMapper;
using InventoryManagementSystem.Products;

namespace InventoryManagementSystem.SubTypes.Dto
{
    public class SubTypeProfile : Profile
    {
        public SubTypeProfile()
        {
            CreateMap<SubTypeDto, SubType>().ReverseMap();
        }

    }
}
