using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirment : IAuthorizationRequirement
    {
        public int MinimumRestaurantsCreated { get; set; }
        public CreatedMultipleRestaurantsRequirment(int minimumRestaurantsCreated)
        {
            MinimumRestaurantsCreated = minimumRestaurantsCreated;
        }
    }
}
