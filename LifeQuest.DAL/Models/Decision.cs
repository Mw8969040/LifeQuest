using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Models
{
    public class Decision
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsConfident { get; set; }
        [Required]
        public bool IsSuccess { get; set; }
        public MetricsCalc MetricsCalcs { get; set; }

    }
}
