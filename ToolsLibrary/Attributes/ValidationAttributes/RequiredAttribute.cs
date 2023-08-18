using System.ComponentModel.DataAnnotations;

namespace ToolsLibrary.Attributes.ValidationAttributes
{
    public class RequiredAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return true;

            return false;
        }

    }
}
