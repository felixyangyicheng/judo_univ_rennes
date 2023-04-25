namespace judo_univ_rennes.Dtos.User
{
    public class UserProfilDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PostCode { get; set; }
        public string? AddressNumber { get; set; }
        public string? StreetName { get; set; }
        public string? Country { get; set; }
        public DateTime? Birthday { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
