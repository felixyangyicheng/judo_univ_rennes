
namespace judo_univ_rennes.Data
{
    [Table("comment")]

    public class Comment:BaseTextItem
    {
        public virtual Post Post { get; set; }
    }
}
