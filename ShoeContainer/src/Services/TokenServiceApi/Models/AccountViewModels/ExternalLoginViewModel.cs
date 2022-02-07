using System.ComponentModel.DataAnnotations;

namespace ShoesOnContainers.Services.TokenServiceApi.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
