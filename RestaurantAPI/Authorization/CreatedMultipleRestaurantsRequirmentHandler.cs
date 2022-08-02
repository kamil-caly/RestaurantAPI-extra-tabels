using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirmentHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirment>
    {
        private readonly RestaurantDbContext _context;

        public CreatedMultipleRestaurantsRequirmentHandler(RestaurantDbContext context)
        {
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            CreatedMultipleRestaurantsRequirment requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c 
                => c.Type == ClaimTypes.NameIdentifier).Value);

            var createdRestaurantsCount = _context
                .Restaurants
                .Count(r => r.CreatedById == userId);

            if (createdRestaurantsCount >= requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
