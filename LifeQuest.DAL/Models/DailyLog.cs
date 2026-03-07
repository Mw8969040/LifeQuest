using System.ComponentModel.DataAnnotations.Schema;

namespace LifeQuest.DAL.Models
{
    public class DailyLog : BaseEntity
    {
        // Duration per one challenge
        public int Duration { get; set; }

        // Points per one challenge
        public int Points { get; set; }

        public int CurrentProgress { get; set; }

        [ForeignKey("Challenge")]
        public int ChallengeId { get; set; }

        public Challenge? Challenge { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
