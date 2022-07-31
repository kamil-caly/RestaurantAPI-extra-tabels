using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode))
                .ForMember(m => m.ChefFullName, c => c.MapFrom(s => s.Chef.FullName))
                .ForMember(m => m.ChefRank, c => c.MapFrom(s => s.Chef.Rank));

            CreateMap<Dish, DishDto>();
            CreateMap<Ingredients, IngredientsDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(m => m.Address, c => c.MapFrom(dto => new Address()
                    { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }))
                .ForMember(m => m.Chef, c => c.MapFrom(dto => new Chef()
                    { FullName = dto.ChefFullName, Rank = dto.ChefRank }));
                
        }
    }
}
