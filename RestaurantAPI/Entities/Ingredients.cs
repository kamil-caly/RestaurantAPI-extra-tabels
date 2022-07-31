namespace RestaurantAPI.Entities
{
    public class Ingredients
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Dish Dish { get; set; }
        public int DishId { get; set; }
    }
}