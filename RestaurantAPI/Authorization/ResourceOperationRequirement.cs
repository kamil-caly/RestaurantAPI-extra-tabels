using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation operation { get; }

        public ResourceOperationRequirement(ResourceOperation operation)
        {
            this.operation = operation;
        }
    }
}
