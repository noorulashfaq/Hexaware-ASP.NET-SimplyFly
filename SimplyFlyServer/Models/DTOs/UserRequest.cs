using SimplyFlyServer.Misc;

namespace SimplyFlyServer.Models.DTOs
{
    public class UserRequest
    {

        [EmailValidation(ErrorMessage = "Invalid entry for Username")]
        public string? UserName { get; set; } = string.Empty;
        [NameValidation(ErrorMessage = "Invalid entry for name")]
        public string? FirstName { get; set; } = string.Empty;
        [NameValidation(ErrorMessage = "Invalid entry for name")]
        public string? LastName { get; set; } = string.Empty;
        public string? Password { get; set; }
        [PhoneNumberValidation(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
    }
}
