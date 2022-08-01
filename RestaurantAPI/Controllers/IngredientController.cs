using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish/{dishId}/Ingredient")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        [HttpGet]
        public ActionResult<List<IngredientDto>> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var ingredients = ingredientService.GetAll(restaurantId, dishId);

            return Ok(ingredients);
        }
    }
}
