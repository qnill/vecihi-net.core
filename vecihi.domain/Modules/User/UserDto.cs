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
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }

    public class RemindPasswordDto
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
