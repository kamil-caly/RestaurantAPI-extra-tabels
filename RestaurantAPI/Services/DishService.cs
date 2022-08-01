using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById(int restaurantId, int dishId);
        List<DishDto> GetAll(int restaurantId);
    }
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext dbContext;
        private readonly IMapper mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var dishEntity = mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;
            dbContext.Dishes.Add(dishEntity);
            dbContext.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var dish = dbContext
                .Dishes
                .Include(d => d.Ingredients)
                .FirstOrDefault(d => d.Id == dishId);

            if (dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            var dishDto = mapper.Map<DishDto>(dish);
            return dishDto;
            
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = dbContext
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var dishes = dbContext
                .Dishes.Include(d => d.Ingredients)
                .Where(d => d.RestaurantId == restaurant.Id);

            var dishDto = mapper.Map<List<DishDto>>(dishes);
            return dishDto;
        }
    }

    
}
