using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem {
	public class Athletes {
		[Key]
		public string IDNumber { get; set; }

		public string Name { get; set; }
		public string Sex { get; set; }
		public int Age { get; set; }
		public int AthleteNumber { get; set; }
		public string Team { get; set; }
		public string Event { get; set; }

		public override string ToString() {
			return $"编号: {AthleteNumber} 姓名: {Name} 项目: {Event} 代表队: {Team}";
		}
	}
}