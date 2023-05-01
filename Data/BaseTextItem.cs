namespace judo_univ_rennes.Data
{
    public class BaseTextItem
    {
        public int Id { get; set; }
        public int Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public virtual ApiUser ApiUser { get; set; }

    }
}
