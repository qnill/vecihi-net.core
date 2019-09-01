using AutoMapper;
using System;

namespace vecihi.infrastructure
{
    public class AutocompleteProfile : Profile
    {
        public AutocompleteProfile()
        {
            CreateMap<Autocomplete<Guid>, AutocompleteDto<Guid>>().ReverseMap();
        }
    }
}
