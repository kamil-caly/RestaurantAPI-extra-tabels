using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirment>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            this.logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirment requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c 
                => c.Type == "DateOfBirth").Value);

            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            logger.LogInformation($"user: {userEmail} with date of birth: [{dateOfBirth}]");

            if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
            {
                logger.LogInformation("Authorization succedded");
                context.Succeed(requirement);
            }
            else
            {
                logger.LogInformation("Authorization failed");
            }

            return Task.CompletedTask;
        }
    }
}
