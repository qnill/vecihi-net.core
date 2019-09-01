using AutoMapper;

namespace vecihi.infrastructure
{
    public class AutocompleteProfile<Type> : Profile
        where Type : struct
    {
        public AutocompleteProfile()
        {
            CreateMap<Autocomplete<Type>, AutocompleteDto<Type>>().ReverseMap();
        }
    }
}
