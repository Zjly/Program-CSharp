using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem {
	public class OrderDatabase : DbContext {
		public OrderDatabase() : base("OrderDatabase") { }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<Goods> Goods { get; set; }
	}
}