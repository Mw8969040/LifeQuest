using System.ComponentModel.DataAnnotations.Schema;

namespace LifeQuest.Models
{
    public class DailyLog
    {
        public int id { get; set; }
        //Duration per one challange
        public int Duration { get; set; }
        //Points per one Challange
        public int Points { get; set; }
        public int CurrentProgress { get; set; }

        [ForeignKey("Challange")]
        public int ChallangeId { get; set; }
        public Challenge Challange { get; set; }
        public string Notes { get; set; }
    }
}
