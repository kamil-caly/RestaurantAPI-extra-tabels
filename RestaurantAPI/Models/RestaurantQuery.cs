namespace RestaurantAPI.Models
{
    public class RestaurantQuery
    {
        public string searchPhrase { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 1;
    }
}
