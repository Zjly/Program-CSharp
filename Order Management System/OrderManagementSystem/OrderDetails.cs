using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem {
	[Serializable]
	public class OrderDetails {
		[Key]
		public string OrderNumber { get; set; }

		public DateTime OrderTime { get; set; }
		public string CustomerName { get; set; }
		public string PhoneNumber { get; set; }
		public List<Goods> Goods { get; set; }

		public OrderDetails() {
			Goods = new List<Goods>();
		}

		public override string ToString() {
			return $"订单号:{OrderNumber} 客户名:{CustomerName} 联系方式:{PhoneNumber}";
		}
	}
}