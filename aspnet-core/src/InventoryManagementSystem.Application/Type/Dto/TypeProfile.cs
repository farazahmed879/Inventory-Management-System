using AutoMapper;
using InventoryManagementSystem.Products;
using Planner.Types.Dto;

namespace Planner.Types.Dto
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<TypeDto, Type>().ReverseMap();
        }

    }
}
