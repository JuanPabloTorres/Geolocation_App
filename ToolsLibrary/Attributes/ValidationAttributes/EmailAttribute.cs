using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ToolsLibrary.Attributes.ValidationAttributes
{
    public class EmailAttribute : ValidationAttribute
    {
        private const string EmailRegexPattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return true;

            string email = value.ToString();
            return Regex.IsMatch(email, EmailRegexPattern);
        }
    }
}