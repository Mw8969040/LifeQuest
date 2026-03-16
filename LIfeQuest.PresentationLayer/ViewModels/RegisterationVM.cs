using LifeQuest.DAL.Models;
using LIfeQuest.BLL.DTOs;
using System.ComponentModel.DataAnnotations;

namespace LIfeQuest.PL.ViewModels
{
    public class RegisterationVM
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

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;


        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public ICollection<string> Countries { get; set; }
        public string Country { get; set; }

        public RegisterationDTO registerationDTO { get; set; }
    }
}
