using System.ComponentModel.DataAnnotations.Schema;

namespace judo_univ_rennes.Data
{
    [Table("post")]

    public class Post:BaseTextItem
    {
        [Column("title")]

        public string Title { get; set; }

        public virtual IList<Comment> Comments { get; set; }

    }
}
