using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SimplyFlyServer.Misc
{
    public class PhoneNumberValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string phone = value as string ?? "";
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            var pattern = @"^\+?[0-9]{10,13}$";
            return Regex.IsMatch(phone, pattern);
        }
    }
}
