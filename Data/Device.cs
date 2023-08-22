
namespace judo_univ_rennes.Data
{
    [Table("device")]

    public class Device
	{
        [Column("id")]
        public Guid Id { get; set; }
        [ForeignKey("ApiUser")]

        [Column("userId")]
        public string ApiUserId { get; set; }
        public virtual ApiUser ApiUser { get; set; }
    }
}

