using System;
namespace judo_univ_rennes.Data
{
	public class Device
	{
		public Guid Id { get; set; }
        public virtual ApiUser ApiUser { get; set; }
    }
}

