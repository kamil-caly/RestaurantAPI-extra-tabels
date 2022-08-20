using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;
using System.Net;

namespace RestaurantAPI.IntegrationTests.DishTests
{
    public class DishControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> factory;
        private HttpClient client;

        public DishControllerTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service
                            => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));

                        if (dbContextOptions != null)
                            services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(option =>
                            option.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<RestaurantDbContext>(options
                            => options.UseInMemoryDatabase("RestaurantDb"));
                    });
                });

            this.client = this.factory.CreateClient(); 
        }

        private void SeedDishes(Dish dish, int restaurantId)
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();

            if (scopeFactory != null)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();

                    var restaurant = dbContext
                        .Restaurants
                        .FirstOrDefault(r => r.Id == restaurantId);

                    if (dbContext != null && restaurant != null)
                    {
                        dish.RestaurantId = restaurant.Id;
                        dbContext.Dishes.Add(dish);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        private static IEnumerable<object[]> GetCorrectCreatedDishDtoModels()
        {
            var list = new List<CreateDishDto>()
            {
                new CreateDishDto()
                {
                    Name = "TestDish"
                },

                new CreateDishDto()
                {
                    Name = "TestDish2",
                    Description = "TestDescription",
                },

                new CreateDishDto()
                {
                    Name = "TestDish",
                    Description = "TestDescription2",
                    Price = 22.34M
                },
            };

            return list.Select(q => new object[] { q });
        }

        private static IEnumerable<object[]> GetInCorrectCreatedDishDtoModels()
        {
            var list = new List<CreateDishDto>()
            {
                new CreateDishDto()
                {
                    Description = "TestWrong"
                },

                new CreateDishDto()
                {
                    Description = "TestDescriptionWrong",
                    Price = 22.34M
                },

                new CreateDishDto()
                {

                },
            };

            return list.Select(q => new object[] { q });
        }

        private static int GetRandomCorrectRestaurantId()
        {
            return new Random().Next(1,3);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async Task GetAll_WithCorrectRestaurantsId_ReturnsOkResult(string restaurantId)
        {
            // arrange

            // act 

            var response = await client.GetAsync($"api/restaurant/{restaurantId}/dish");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("134")]
        [InlineData("222")]
        public async Task GetAll_WithInCorrectRestaurantsId_ReturnsNotFound(string restaurantId)
        {
            // arrange

            // act 

            var response = await client.GetAsync($"api/restaurant/{restaurantId}/dish");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("1", "1")]
        [InlineData("2", "3")]
        public async Task GetDish_WithCorrectRestaurantsAndDishId_ReturnsOK(string restaurantId, string dishId)
        {
            // arrange

            // act 

            var response = await client.GetAsync($"api/restaurant/{restaurantId}/dish/{dishId}");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("1", "123")]
        [InlineData("2", "333")]
        public async Task GetDish_WithInCorrectRestaurantIdAndInCorrectDishId_ReturnsNotFound(string restaurantId, string dishId)
        {
            // arrange

            // act 

            var response = await client.GetAsync($"api/restaurant/{restaurantId}/dish/{dishId}");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(GetCorrectCreatedDishDtoModels))]
        public async Task CreateDish_WithValidModel_ReturnsCreatedStatus(CreateDishDto model)
        {
            // arrange

            var httpContent = model.ToJsonHttpContent();

            // act

            var response = await client.PostAsync($"api/restaurant/{GetRandomCorrectRestaurantId()}/dish", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(GetInCorrectCreatedDishDtoModels))]
        public async Task CreateDish_WithInValidModel_ReturnsBadRequest(CreateDishDto model)
        {
            // arrange

            var httpContent = model.ToJsonHttpContent();

            // act

            var response = await client.PostAsync($"api/restaurant/{GetRandomCorrectRestaurantId()}/dish", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Headers.Location.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAllDishes_ForValidRestaurantId_ReturnsNoContent()
        {
            // arrange 

            var dish = new Dish()
            {
                RestaurantId = 1,
                Name = "Test"
            };

            SeedDishes(dish, dish.RestaurantId);

            // act 

            var response = await client.DeleteAsync($"api/restaurant/{dish.RestaurantId}/dish");

            // assert 

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteAllDishes_ForInValidRestaurantId_ReturnsNotFound()
        {
            // act 

            var response = await client.DeleteAsync($"api/restaurant/456/dish");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(GetCorrectCreatedDishDtoModels))]
        public async Task DeleteDish_ForCorrectRestaurantIdAndDishId_ReturnsNoContent(CreateDishDto model)
        {
            // arrange 

            var dish = new Dish()
            {
                RestaurantId = GetRandomCorrectRestaurantId(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price
            };

            SeedDishes(dish, dish.RestaurantId);

            // act 

            var response = await client.DeleteAsync($"api/restaurant/{dish.RestaurantId}/dish/{dish.Id}");

            // assert 

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteDish_ForInCorrectRestaurantIdAndCorrectDishId_ReturnsNotFound()
        {
            // arrange 

            // act 

            var response = await client.DeleteAsync($"api/restaurant/345/dish/1");

            // assert 

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteDish_ForCorrectRestaurantIdAndInCorrectDishId_ReturnsNotFound()
        {
            // arrange 

            // act 

            var response = await client.DeleteAsync($"api/restaurant/{GetRandomCorrectRestaurantId()}/dish/345");

            // assert 

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteDish_ForInCorrectRestaurantIdAndInCorrectDishId_ReturnsNotFound()
        {
            // arrange 

            // act 

            var response = await client.DeleteAsync($"api/restaurant/342/dish/345");

            // assert 

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
