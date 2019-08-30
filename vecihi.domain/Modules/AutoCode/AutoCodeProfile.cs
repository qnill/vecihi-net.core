using AutoMapper;
using vecihi.database.model;

namespace vecihi.domain.Modules
{
    public class AutoCodeProfile : Profile
    {
        public AutoCodeProfile()
        {
            CreateMap<AutoCodeAddDto, AutoCodeModel>();
            CreateMap<AutoCodeUpdateDto, AutoCodeModel>();
            CreateMap<AutoCodeModel, AutoCodeGetDto>();
        }
    }
}
