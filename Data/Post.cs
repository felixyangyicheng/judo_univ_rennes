namespace judo_univ_rennes.Data
{
    public class Post:BaseTextItem
    {

        public string Title { get; set; }

        public virtual IList<Comment> Comments { get; set; }

    }
}
