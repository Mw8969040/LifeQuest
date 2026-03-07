using System.ComponentModel.DataAnnotations;

namespace LifeQuest.DAL.Models
{
    public class Decision : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsConfident { get; set; }

        [Required]
        public bool IsSuccess { get; set; }

        public MetricsCalc? Metrics { get; set; }
    }
}
