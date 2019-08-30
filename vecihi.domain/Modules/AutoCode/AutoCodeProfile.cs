using AutoMapper;
using vecihi.database.model;

namespace vecihi.domain.Modules
{
    public class AutoCodeProfile : Profile
    {
        public AutoCodeProfile()
        {
            CreateMap<AutoCodeAddDto, AutoCode>();
            CreateMap<AutoCodeUpdateDto, AutoCode>();
            CreateMap<AutoCode, AutoCodeGetDto>();
        }
    }
}
