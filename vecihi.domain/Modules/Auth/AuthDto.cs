using System.ComponentModel.DataAnnotations;

namespace vecihi.domain.Modules
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}