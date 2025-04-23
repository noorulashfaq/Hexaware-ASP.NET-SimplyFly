using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SimplyFlyServer.Misc
{
    public class EmailValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string email = value as string ?? "";
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // ✉️ Basic pattern for standard email format
            var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, pattern);
        }
    }
}
