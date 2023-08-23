using System;
namespace judo_univ_rennes.Dtos.Posts
{
	public class PostViewModel
	{


        public MarkupString DisplayContent { get; set; }

        public DateTime CreatedOn { get; set; }


        public DateTime UpdatedOn { get; set; }
        public string ApiUserName { get; set; }

        public string Title { get; set; }

        public List<CommentDto> Comments { get; set; }
    }
}

