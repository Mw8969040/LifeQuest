using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LifeQuest.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public UserProfile UserProfile { get; set; } 
    }
}
