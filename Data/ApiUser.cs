using Microsoft.AspNetCore.Identity;

namespace judo_univ_rennes.Data
{
    public class ApiUser : IdentityUser
    {
        public string? Civilite { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PostCode { get; set; }
        public string? AddressNumber { get; set; }
        public string? Extention { get; set; }
        public string? StreetType { get; set; }
        public string? StreetName { get; set; }

        public string? City { get; set; }
        public string? Country { get; set; }
        public DateTime? Birthday { get; set; }
        public virtual IList<Command> Commands { get; set; }
        public virtual IList<Post> Posts { get; set; }
        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<Device> Devices { get; set; }
        //public ICollection<SignalRConnection> SignalRConnections { get; set; }
        //public virtual ICollection<ConversationRoom> Rooms { get; set; }

    }
}
