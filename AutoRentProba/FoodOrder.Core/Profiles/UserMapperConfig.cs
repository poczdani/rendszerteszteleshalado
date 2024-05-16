using AutoMapper;
using CarRent.Core.Models.User;
using CarRent.Data.Entities;

namespace CarRent.Core.Profiles
{
    public class UserMapperConfig : Profile
    {
        public UserMapperConfig()
        {
            CreateMap<UserRegisterDto, User>();
        }
    }
}
