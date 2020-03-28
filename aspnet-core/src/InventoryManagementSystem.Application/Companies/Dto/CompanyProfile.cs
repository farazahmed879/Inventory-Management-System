using AutoMapper;

namespace InventoryManagementSystem.Companies.Dto
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CompanyDto, Company>().ReverseMap();
        }

    }
}
