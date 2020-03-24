using AutoMapper;
using InventoryManagementSystem.Products;

namespace InventoryManagementSystem.Types.Dto
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<TypeDto, Type>().ReverseMap();
        }

    }
}
