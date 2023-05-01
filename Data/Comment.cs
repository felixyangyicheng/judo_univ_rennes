namespace judo_univ_rennes.Data
{
    public class Comment:BaseTextItem
    {
        public virtual Post Post { get; set; }
    }
}
