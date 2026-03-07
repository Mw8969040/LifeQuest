using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeQuest.DAL.Models
{
    public class UserChallenge : BaseEntity
    {
        public int UserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        public int ChallengeId { get; set; }

        public Challenge? Challenge { get; set; }

        public bool IsSuccess { get; set; }

        [RegularExpression("^(Ended|InProgress|NotStarted)$",
            ErrorMessage = "Status must be Ended, InProgress, or NotStarted")]
        public string Status { get; set; } = string.Empty;

        [ForeignKey("Badge")]
        public int BadgeId { get; set; }

        public Badges? Badge { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
