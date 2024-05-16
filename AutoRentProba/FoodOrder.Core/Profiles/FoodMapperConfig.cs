using AutoMapper;
using CarRent.Core.Models.Car;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarRent.Core.Profiles
{
    public class CarMapperConfig : Profile
    {
        public CarMapperConfig()
        {
            CreateMap<Data.Entities.Car, Models.Car.CarDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<Models.Car.CreateCarDto, Data.Entities.Car>();
            CreateMap<Data.Entities.CarCategory, Models.Car.CarCategoryDto>();
        }
    }
}
