using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeQuest.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public bool isPublic { get; set; }
        [RegularExpression("^(Easy|Meduim|Hard)$")]
        public string Difficulty { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId {  get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<DailyLog> DailyLogs { get; set; }
    }
}
