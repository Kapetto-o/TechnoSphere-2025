using System.ComponentModel.DataAnnotations;
using TechnoSphere_2025.Properties;

namespace TechnoSphere_2025.models
{
    public class UserRegistrationModel
    {
        [Required(
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorEnterUsername))]
        [StringLength(32,
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorUsernameLength))]
        [RegularExpression(@"^[a-zA-Z0-9_]+$",
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorUsernamePattern))]
        public string Username { get; set; } = string.Empty;

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
        [StringLength(100, MinimumLength = 6,
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorPasswordLength))]
        public string Password { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorRepeatPassword))]
        [Compare(nameof(Password),
            ErrorMessageResourceType = typeof(ErrorValidation),
            ErrorMessageResourceName = nameof(ErrorValidation.ErrorPasswordMismatch))]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}