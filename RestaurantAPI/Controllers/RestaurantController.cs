using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext dbContext;
        private readonly IMapper mapper;

        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        { 
            var restaurants = dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Chef)
                .Include(r => r.Dishes)
                .ToList();

            var restaurantsDtos = mapper.Map<List<RestaurantDto>>(restaurants);

            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Chef)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                return NotFound();
            }

            var restaurantDto = mapper.Map<RestaurantDto>(restaurant);
            return Ok(restaurantDto);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurant = mapper.Map<Restaurant>(dto);
            dbContext.Restaurants.Add(restaurant);
            dbContext.SaveChanges();

            return Created($"/api/restaurant/{restaurant.Id}", null);
        }
    }
}
