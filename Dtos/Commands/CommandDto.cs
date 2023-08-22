
namespace judo_univ_rennes.Dtos.Commands
{
    public class CommandDto
    {

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
   

        public DateTime UpdatedOn { get; set; }
        public string ApiUserName { get; set; }
        public string ApiUserId { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public bool Closed { get; set; }
    }
}
