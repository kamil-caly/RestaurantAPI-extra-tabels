using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;
using System.Net;

namespace RestaurantAPI.IntegrationTests.RestaurantTests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient client;
        private WebApplicationFactory<Program> factory;

        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
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

        private void SeedRestaurant(Restaurant restaurant)
        {
            var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();

            if (scopeFactory != null)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();

                    if (dbContext != null)
                    {
                        dbContext.Restaurants.Add(restaurant);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=10&pageNumber=2")]
        [InlineData("pageSize=15&pageNumber=33")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParams)
        {
            // arrange

            // act 

            var response = await client.GetAsync($"/api/restaurant?{queryParams}");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("pageSize=55&pageNumber=1")]
        [InlineData("pageSize=0&pageNumber=2")]
        [InlineData("pageSize=1&pageNumber=33")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetAll_WithInvalidQueryParameters_ReturnsBadRequest(string queryParams)
        {
            // arrange

            // act 

            var response = await client.GetAsync($"/api/restaurant?{queryParams}");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateRestaurant_WithValidModel_ReturnsCreatedStaus()
        {
            // arrange

            var model = new CreateRestaurantDto()
            {
                Name = "TestName",
                City = "TestCity",
                Street = "TestStreet",
                ChefFullName = "TestChefFullName",
                ChefRank = 5
            };

            var httpContent = model.ToJsonHttpContent();

            // act

            var response = await client.PostAsync("/api/restaurant", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateRestaurant_WithInvalidModel_ReturnsBadRequest()
        {
            // arrange

            var model = new CreateRestaurantDto()
            {
                ContactEmail = "Test@email.com",
                Description = "TestDescription",
                ContactNumber = "111 222 333"
            };

            var httpContent = model.ToJsonHttpContent();

            // act

            var response = await client.PostAsync("/api/restaurant", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ForRestaurantOwner_ReturnsNoContent()
        {
            // arrange 

            var restaurant = new Restaurant()
            {
                CreatedById = 1,
                Name = "Test"
            };

            SeedRestaurant(restaurant);

            // act 

            var response = await client.DeleteAsync($"/api/restaurant/{restaurant.Id}");

            // assert 

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

        [Fact]
        public async Task Delete_ForNonExistingRestaurant_ReturnsNotFound()
        {
            // act 

            var response = await client.DeleteAsync("/api/restaurant/345");

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForNonRestaurantOwer_ReturnsForbidden()
        {
            // arrange 

            var restaurant = new Restaurant()
            {
                CreatedById = 45,
                Name = "Test"
            };

            SeedRestaurant(restaurant);

            // act 

            var response = await client.DeleteAsync($"/api/restaurant/{restaurant.Id}");

            // assert 

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}

