using AutoMapper;
using vecihi.database.model;

namespace vecihi.domain.Modules
{
    public class AutoCodeLogProfile : Profile
    {
        public AutoCodeLogProfile()
        {
            CreateMap<AutoCodeLog, AutoCodeLogListDto>()
                .ForMember(
                   dest => dest.Code,
                   opt => opt.MapFrom(src => src.AutoCode.CodeFormat.Replace("{0}", src.CodeNumber.ToString())));
        }
    }
}