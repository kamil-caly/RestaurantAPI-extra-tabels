using System.ComponentModel.DataAnnotations;
namespace RestaurantAPI.Entities
{
    public class Chef
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        [Range(1, 5)]
        public int Rank { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}