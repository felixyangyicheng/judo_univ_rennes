using System.ComponentModel.DataAnnotations;

namespace judo_univ_rennes.Dtos.User
{
    public class UserLoginDto : UserEmailDto
    {

        [Required]
        public string Password { get; set; }
    }
}
