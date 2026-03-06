using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Models
{
    public class Badges
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        //to gain Badge
        public int Points { get; set; }
        public string Image { get; set; }

        public List<UserChallenge> UserChallanges { get; set; }

    }
}
