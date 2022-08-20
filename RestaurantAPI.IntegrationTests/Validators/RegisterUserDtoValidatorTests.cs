using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;

namespace RestaurantAPI.IntegrationTests.Validators
{
    public class RegisterUserDtoValidatorTests
    {
        private RestaurantDbContext dbContext;

        public RegisterUserDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<RestaurantDbContext>();
            builder.UseInMemoryDatabase("TestDb");

            this.dbContext = new RestaurantDbContext(builder.Options);
            this.Seed();
        }

        private void Seed()
        {
            var testUserts = new List<User>()
            {
                new User()
                {
                    Email = "test@test.com"
                },

                new User()
                {
                    Email = "test3@test.com"
                }
            };

            dbContext.Users.AddRange(testUserts);
            dbContext.SaveChanges();
        }

        private static IEnumerable<object[]> GetValidRegiserUserDtoModels()
        {
            var list = new List<RegisterUserDto>()
            {
                new RegisterUserDto()
                {
                    Email = "test@test123.com",
                    Password = "pass123",
                    ConfirmPassword = "pass123"
                },

                new RegisterUserDto()
                {
                    Email = "test@test2.com",
                    Password = "pass123456",
                    ConfirmPassword = "pass123456"
                },

                new RegisterUserDto()
                {
                    Email = "test@lastEmail.com",
                    Password = "AnyPassword",
                    ConfirmPassword = "AnyPassword"
                },
            };

            return list.Select(q => new object[] { q });
        }

        private static IEnumerable<object[]> GetInValidRegiserUserDtoModels()
        {
            var list = new List<RegisterUserDto>()
            {
                new RegisterUserDto()
                {
                    Email = "test@test45.com",
                    Password = "pass",
                    ConfirmPassword = "pass"
                },

                new RegisterUserDto()
                {
                    Email = "test@test.com",
                    Password = "pass123456",
                    ConfirmPassword = "pass123456"
                },

                new RegisterUserDto()
                {
                    Email = "test@lastEmail.com",
                    Password = "AnyPassword1",
                    ConfirmPassword = "AnyPassword"
                },
            };

            return list.Select(q => new object[] { q });
        }

        [Theory]
        [MemberData(nameof(GetValidRegiserUserDtoModels))]
        public void Validate_ForValidModel_ReturnsSuccess(RegisterUserDto model)
        {
            var validator = new RegisterUserDtoValidator(dbContext);

            // act 

            var result = validator.TestValidate(model);

            // assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetInValidRegiserUserDtoModels))]
        public void Validate_ForInValidModel_ReturnsSuccess(RegisterUserDto model)
        {
            var validator = new RegisterUserDtoValidator(dbContext);

            // act 

            var result = validator.TestValidate(model);

            // assert

            result.ShouldHaveAnyValidationError();
        }
    }
}
