using FluentAssertions;
using FluentValidation.TestHelper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;

namespace RestaurantAPI.IntegrationTests.Validators
{
    public class RestaurantQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<RestaurantQuery>()
            {
                new RestaurantQuery()
                {
                    pageNumber = 1,
                    pageSize = 10,
                },

                new RestaurantQuery()
                {
                    pageNumber = 2,
                    pageSize = 15,
                },

                new RestaurantQuery()
                {
                    pageNumber = 22,
                    pageSize = 5,
                    SortBy = nameof(Restaurant.Name)
                },

                new RestaurantQuery()
                {
                    pageNumber = 12,
                    pageSize = 15,
                    SortBy = nameof(Restaurant.Category)
                },
            };

            return list.Select(q => new object[] { q });
        }

        public static IEnumerable<object[]> GetSampleInValidData()
        {
            var list = new List<RestaurantQuery>()
            {
                new RestaurantQuery()
                {
                    pageNumber = 1,
                    pageSize = 11,
                },

                new RestaurantQuery()
                {
                    pageNumber = 2,
                    pageSize = 122,
                },

                new RestaurantQuery()
                {
                    pageNumber = 22,
                    pageSize = 4,
                    SortBy = nameof(Restaurant.HasDelivery)
                },

                new RestaurantQuery()
                {
                    pageNumber = 12,
                    pageSize = 15,
                    SortBy = nameof(Restaurant.Dishes)
                },
            };

            return list.Select(q => new object[] { q });
        }

        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForCorrectModel_ReturnSuccess(RestaurantQuery model)
        {
            // arrange 

            var validator = new RestaurantQueryValidator();
            
            // act 

            var result = validator.TestValidate(model); 

            // assert 

            result.ShouldNotHaveAnyValidationErrors();
        }


        [Theory]
        [MemberData(nameof(GetSampleInValidData))]
        public void Validate_ForInCorrectModel_ReturnFailure(RestaurantQuery model)
        {
            // arrange 

            var validator = new RestaurantQueryValidator();

            // act 

            var result = validator.TestValidate(model);

            // assert 

            result.ShouldHaveAnyValidationError();
        }
    }
}
