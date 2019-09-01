using AutoMapper;
using System;
using vecihi.database.model;
using vecihi.infrastructure;

namespace vecihi.domain.Modules
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeAddDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
            CreateMap<Employee, EmployeeListDto>();
            CreateMap<Employee, EmployeeCardDto>();
            CreateMap<Employee, Autocomplete<Guid>>()
                .ForMember(
                    dest => dest.Search,
                    opt => opt.MapFrom(src => (src.Email ?? "") + (src.Name ?? "") + (src.Phone ?? "")))
                .ForMember(
                    dest => dest.Text,
                    opt => opt.MapFrom(src => src.Name));
        }
    }
}