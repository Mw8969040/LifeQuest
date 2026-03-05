using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeQuest.Models
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("User")] //shared PK
        public int UserId { get; set; } 
        [MaxLength(200)]
        public string Bio { get; set; }
        public int TotalPoints { get; set; } // All Challanges Points
        public int SuccessRate { get; set; } // Is Successed From UserChallange (No.SuccessedChallanges/TotalChallnges)*100
        [ForeignKey("Level")]
        public int LevelId { get; set; }
        public Level MyProperty { get; set; }
        public Level Level { get; set; }
        public ApplicationUser User { get; set; }

    }
}
