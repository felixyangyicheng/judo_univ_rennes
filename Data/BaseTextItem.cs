using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ApiUser ApiUser { get; set; }

    }
}
