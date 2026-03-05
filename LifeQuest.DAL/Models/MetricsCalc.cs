using System.ComponentModel.DataAnnotations.Schema;

namespace LifeQuest.Models
{
    public class MetricsCalc
    {
        public int id { get; set; }
        public int SuccessRate { get; set; }
        public int RiskPattern { get; set; }
        public int ConfidenceAccuracy { get; set; }
        public int OverConfidenceIndex { get; set; }
        [ForeignKey("Decision")]
        public int DecisionId { get; set; }
        public Decision Decision { get; set; }
    }
}
