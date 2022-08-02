namespace RestaurantAPI.Models
{
    public class RestaurantQuery
    {
        public string searchPhrase { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; } 
    }
}
