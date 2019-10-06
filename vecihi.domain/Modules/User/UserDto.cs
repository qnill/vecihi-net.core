using System.ComponentModel.DataAnnotations;

namespace vecihi.domain.Modules
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class RemindPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class SendEmailForActivationDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }

    public class ConfirmEmailDto
    {
        [Required]
        public string Token { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
