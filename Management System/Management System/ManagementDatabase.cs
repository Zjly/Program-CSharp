using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ManagementSystem {
	class ManagementDatabase : DbContext {
		public ManagementDatabase() : base("ManagementDatabase") { }
		public DbSet<Athletes> Athletes { get; set; }
	}
}