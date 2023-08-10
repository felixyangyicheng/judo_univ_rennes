using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace judo_univ_rennes.Data
{
    public class BaseTextItem
    {
        public int Id { get; set; }
        [Column("Content", TypeName = "text")]
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public virtual ApiUser ApiUser { get; set; }

    }
}
