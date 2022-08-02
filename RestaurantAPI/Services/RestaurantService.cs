using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
        RestaurantDto GetById(int id);
        public void Delete(int id);
        public void Update(UpdateRestaurantDto dto, int id);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<RestaurantService> logger;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;

        public RestaurantService(RestaurantDbContext dbContext, 
            IMapper mapper, 
            ILogger<RestaurantService> logger,
            IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Chef)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var result = mapper.Map<RestaurantDto>(restaurant);
            return result;
        }

        public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
        {
            var baseQuery = dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Chef)
                .Include(r => r.Dishes)

                .Where(r => query.searchPhrase == null
                    || (r.Name.ToLower().Contains(query.searchPhrase)
                    || r.Description.ToLower().Contains(query.searchPhrase)));


            var restaurants = baseQuery
                .Skip(query.pageSize * (query.pageNumber - 1))
                .Take(query.pageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var restaurantDtos = mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new PagedResult<RestaurantDto>(restaurantDtos, totalItemsCount, query.pageSize, query.pageNumber);

            return result;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = userContextService.GetUserId;
            dbContext.Restaurants.Add(restaurant);
            dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Delete(int id)
        {
            logger.LogError($"Restaurant with id: {id} Delete action invoked");

            var restaurant = dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = authorizationService.AuthorizeAsync(
                userContextService.User,
                restaurant,
                 new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            dbContext.Restaurants.Remove(restaurant);
            dbContext.SaveChanges();
        }

        public void Update(UpdateRestaurantDto dto, int id)
        {
            var restaurant = dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = authorizationService.AuthorizeAsync(
                userContextService.User,
                restaurant,
                 new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            dbContext.SaveChanges();
        }
    }
}
