using AutoMapper;
using vecihi.database.model;

namespace vecihi.domain.Modules
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>();
        }
    }
}