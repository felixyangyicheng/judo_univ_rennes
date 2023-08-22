
namespace judo_univ_rennes.Dtos.IndexMarkdowns
{
	public class IndexMarkdownDto
	{
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
        public string Title { get; set; }

        public string? Type { get; set; }


        public string? Version { get; set; }
    }
}

