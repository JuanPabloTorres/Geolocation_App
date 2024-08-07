using System.ComponentModel.DataAnnotations;

namespace ToolsLibrary.Attributes.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ConfirmPasswordAttribute : ValidationAttribute
    {
        private readonly string _originalPropertyName;

        public ConfirmPasswordAttribute(string originalPropertyName)
        {
            _originalPropertyName = originalPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(_originalPropertyName);

            if (propertyInfo == null)
            {
                return new ValidationResult($"Property with name {_originalPropertyName} not found.");
            }

            var originalValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (!object.Equals(originalValue, value))
            {
                return new ValidationResult($"The {_originalPropertyName} and confirmation do not match.");
            }

            return ValidationResult.Success;
        }
    }
}