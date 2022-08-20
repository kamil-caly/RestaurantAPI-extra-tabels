using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Net;

namespace RestaurantAPI.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient client;
        private Mock<IAccountService> accountServiceMock = new Mock<IAccountService>();
        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {
            this.client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service
                            => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));

                        if (dbContextOptions != null)
                            services.Remove(dbContextOptions);

                        services.AddSingleton<IAccountService>(accountServiceMock.Object);

                        services.AddDbContext<RestaurantDbContext>(options
                            => options.UseInMemoryDatabase("RestaurantDb"));
                    });
                }).CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            // arrange

            var registerUser = new RegisterUserDto()
            {
                Email = "test@email.com",
                Password = "password1",
                ConfirmPassword = "password1"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            // act 

            var response = await client.PostAsync("/api/account/register", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUser_ForInValidModel_ReturnsBadRequest()
        {
            // arrange

            var registerUser = new RegisterUserDto()
            {
                Email = "test@email.com",
                Password = "password1",
                ConfirmPassword = "password233"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            // act 

            var response = await client.PostAsync("/api/account/register", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOk()
        {
            // arrange

            accountServiceMock
                .Setup(e => e.GenerateJwt(It.IsAny<LoginDto>()))
                .Returns("jwt");

            var loginDto = new LoginDto()
            {
                Email = "test@test.com",
                Password = "password123"
            };

            var httpContent = loginDto.ToJsonHttpContent();

            // act

            var response = await client.PostAsync("api/account/login", httpContent);

            // assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
