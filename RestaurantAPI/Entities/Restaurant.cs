namespace RestaurantAPI.Entities
{
    public class Restaurant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public virtual Address Address { get; set; }
        public int AddressId { get; set; }
        public virtual Chef Chef { get; set; }
        public int ChefId { get; set; }

        public virtual List<Dish> Dishes { get; set; } 
    }
}
