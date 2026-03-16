using System.ComponentModel.DataAnnotations;

namespace LIfeQuest.PL.ViewModels
{
    public class ForgotPasswordViM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
