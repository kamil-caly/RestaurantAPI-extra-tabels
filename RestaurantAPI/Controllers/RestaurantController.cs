using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext dbContext;

        public RestaurantController(RestaurantDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var restaurants = dbContext
                .Restaurants
                .ToList();

            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get([FromRoute] int id)
        {
            var restaurant = dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }
    }
}
