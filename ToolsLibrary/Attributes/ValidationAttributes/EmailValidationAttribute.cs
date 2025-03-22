using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolsLibrary.Attributes.ValidationAttributes
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        private static readonly Regex EmailRegex = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|net|org|edu|gov|io|co|biz|info|me)$", RegexOptions.IgnoreCase);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return new ValidationResult("Email is required.");
                }

                if (!EmailRegex.IsMatch(email))
                {
                    return new ValidationResult("Email format invalid.");
                }
            }

            return ValidationResult.Success;
        }
    }
}