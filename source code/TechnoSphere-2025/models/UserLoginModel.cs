using System.ComponentModel.DataAnnotations;
using TechnoSphere_2025.Properties;

namespace TechnoSphere_2025.models
{
    public class UserLoginModel
    {
        [Required(
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorEnterEmail))]
        [EmailAddress(
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorInvalidEmail))]
        public string Email { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorEnterPassword))]
        public string Password { get; set; } = string.Empty;
    }
}