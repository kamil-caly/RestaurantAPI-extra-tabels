using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Seed()
        {
            if (dbContext.Database.CanConnect())
            {
                if (dbContext.Database.IsRelational())
                {
                    var pendingMigrations = dbContext.Database.GetPendingMigrations();
                    if (pendingMigrations != null && pendingMigrations.Any())
                    {
                        dbContext.Database.Migrate();
                    }
                }

                if (!dbContext.Restaurants.Any())
                {
                    dbContext.Restaurants.AddRange(GetRestaurant());
                    dbContext.SaveChanges();
                }

                if (!dbContext.Roles.Any())
                {
                    dbContext.Roles.AddRange(this.GetRoles());
                    dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Restaurant> GetRestaurant()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "Fast Food Restaurant",
                    ContactEmail = "contactKFC@email.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Price = 10.33M,
                            Ingredients = new List<Ingredients>()
                            {
                                new Ingredients()
                                {
                                    Name = "Chicken"
                                },

                                new Ingredients()
                                {
                                    Name = "Sauce"
                                }
                            }
                        },

                        new Dish()
                        {
                            Name = "Chrisps",
                            Price = 5.10M,
                            Ingredients = new List<Ingredients>()
                            {
                                new Ingredients()
                                {
                                    Name = "Potato"
                                },

                                new Ingredients()
                                {
                                    Name = "Salt"
                                }
                            }
                        },
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "33-444"
                    },
                    Chef = new Chef()
                    {
                        FullName = "Mirosław Staropolski",
                        Rank = 4,
                        ContactNumber = "111222333"
                    }

                },

                new Restaurant()
                {
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description = "Fast McDonald Food Restaurant",
                    ContactEmail = "contactMcDonald@email.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "ChessBurger",
                            Price = 10.33M,
                            Ingredients = new List<Ingredients>()
                            {
                                new Ingredients()
                                {
                                    Name = "Roll"
                                },

                                new Ingredients()
                                {
                                    Name = "Salat"
                                },

                                new Ingredients()
                                {
                                    Name = "Chess"
                                },

                                new Ingredients()
                                {
                                    Name = "Mild Sauce",
                                    Description = "Mild Souce with Tomato and Carrot"
                                }

                            }
                        }
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Miedziana 6",
                        PostalCode = "33-321"
                    },
                    Chef = new Chef()
                    {
                        FullName = "Jan Kowalski",
                        Rank = 3,
                        ContactNumber = "123456789"
                    }
                }
            };
            return restaurants;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }
    }
}
