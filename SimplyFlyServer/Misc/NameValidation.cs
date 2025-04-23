using System.ComponentModel.DataAnnotations;

namespace SimplyFlyServer.Misc
{
    public class NameValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string strValue = value as string ?? "";
            if (string.IsNullOrEmpty(strValue))
            {
                return false;
            }
            // Check if the string contains only letters and spaces
            foreach (char c in strValue)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
