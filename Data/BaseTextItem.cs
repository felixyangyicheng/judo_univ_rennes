using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace judo_univ_rennes.Data
{
    public class BaseTextItem
    {

        [Column("id")]

        public int Id { get; set; }
        [Column("content", TypeName = "text")]
        public string Content { get; set; }
        [Column("createdOn")]

        public DateTime CreatedOn { get; set; }
        [Column("updatedOn")]

        public DateTime UpdatedOn { get; set; }
        [Column("userId")]
        [ForeignKey("ApiUser")]

        public string ApiUserId { get; set; }
        public virtual ApiUser ApiUser { get; set; }

    }
}
