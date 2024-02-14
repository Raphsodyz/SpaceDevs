using System.ComponentModel.DataAnnotations;

namespace Tests.Helper
{
    public static class ValidationHelper
    {
        public static IList<ValidationResult> ValidateObject<T>(ref T obj)
        {
            var validate = new List<ValidationResult>();
            var context = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, context, validate, true);
            return validate;
        }
    }
}