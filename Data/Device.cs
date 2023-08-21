using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace judo_univ_rennes.Data
{
    [Table("device")]

    public class Device
	{
        [Column("id")]
        public Guid Id { get; set; }
        public virtual ApiUser ApiUser { get; set; }
    }
}

