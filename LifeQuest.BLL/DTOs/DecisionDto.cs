using System;

namespace LifeQuest.BLL.DTOs
{
    public class DecisionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsConfident { get; set; }
        public bool IsSuccess { get; set; }
        public string RiskLevel { get; set; } = "Medium";
        public int UserId { get; set; }
        public MetricsCalcDto? Metrics { get; set; }
    }
}
