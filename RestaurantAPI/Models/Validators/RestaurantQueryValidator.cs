using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = {nameof(Restaurant.Name),
            nameof(Restaurant.Description), nameof(Restaurant.Category)};
        public RestaurantQueryValidator()
        {
            RuleFor(r => r.pageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(r => r.pageSize)
                .Custom((value, context) =>
                {
                    if (!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure("PageSize", $"PageSize must be in " +
                            $"[{string.Join(",", allowedPageSizes)}");
                    }
                });

            RuleFor(r => r.SortBy)
                .Custom((value, context) =>
                {
                    if (!allowedSortByColumnNames.Contains(value) && !string.IsNullOrEmpty(value))
                    {
                        context.AddFailure("SortBy", $"SortBy must be Empty or one with Values: " +
                            $"Name, Description or Category");
                    }
                });
        }
    }
}
