using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IIngredientService
    {
        List<IngredientDto> GetAll(int restaurantId, int dishId);
    }
    public class IngredientService : IIngredientService
    {
        private readonly RestaurantDbContext dbContext;
        private readonly IMapper mapper;
        public IngredientService(RestaurantDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public List<IngredientDto> GetAll(int restaurantId, int dishId)
        {
            var dish = GetDishById(dishId, restaurantId);

            var ingredients = dbContext
                .Ingredients
                .Where(i => i.DishId == dish.Id);

            var ingredientsDto = mapper.Map<List<IngredientDto>>(ingredients);
            return ingredientsDto;
        }

        private Dish GetDishById(int dishId, int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = dbContext
                .Dishes
                .Include(d => d.Ingredients)
                .FirstOrDefault(r => r.Id == dishId && r.RestaurantId == restaurant.Id);

            if (dish is null)
                throw new NotFoundException("Dish not found");

            return dish;
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = dbContext
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }
    }

    
}
