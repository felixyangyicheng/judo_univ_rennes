using System.ComponentModel.DataAnnotations;

namespace judo_univ_rennes.Dtos.User
{
    public class UserEmailDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
