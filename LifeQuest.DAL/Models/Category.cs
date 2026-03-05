using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Challenge> Challanges { get; set; }
    }
}
