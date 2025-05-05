using System.ComponentModel.DataAnnotations;

namespace TechnoSphere_2025.managers
{
    internal class ValidationManager
    {
        public static List<ValidationResult> Validate(object obj)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(obj);
            Validator.TryValidateObject(obj, ctx, results, true);
            return results;
        }
    }
}