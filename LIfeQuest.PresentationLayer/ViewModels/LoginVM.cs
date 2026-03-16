using System.ComponentModel.DataAnnotations;

namespace LIfeQuest.PL.ViewModels
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string PlainPassword { get; set; } = string.Empty;
    }
}
