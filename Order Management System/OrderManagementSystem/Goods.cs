using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem {
	public class Goods {
		[Key]
		/** 货物编号 */
		public string GoodsNumber { get; set; }

		public string TradeName { get; set; }
		public string Type { get; set; }
		public double UnitPrice { get; set; }
		public int Count { get; set; }
		public double TotalPrice { get; set; }

		public Goods() {
			GoodsNumber = Guid.NewGuid().ToString();
		}

		public override string ToString() {
			return $"商品名:{TradeName} 类型:{Type} 单价:{UnitPrice}";
		}
	}
}