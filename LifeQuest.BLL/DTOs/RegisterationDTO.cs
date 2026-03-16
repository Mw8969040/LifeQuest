using LifeQuest.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace LIfeQuest.BLL.DTOs
{
    public class RegisterationDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string PlainPassword { get; set; } = string.Empty;

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public ICollection<string> Countries { get; set; } 
        public string Country { get; set; } 
    }
}
