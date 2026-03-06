using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeQuest.Models
{
    public class UserChallenge
    {
        public int UserId { get; set; }   // String matches ApplicationUser.Id
        public ApplicationUser ApplicationUser { get; set; }

        public int ChallangeId { get; set; }
        public Challenge Challange { get; set; }

        public bool IsSuccess { get; set; }

        [RegularExpression("^(Ended|InProgress|NotStarted)$",
            ErrorMessage = "Status must be Ended, InProgress, or NotStarted")]
        public string Status { get; set; }

        [ForeignKey("Badge")]
        public int BadgeId { get; set; }
        public Badges Badge { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
