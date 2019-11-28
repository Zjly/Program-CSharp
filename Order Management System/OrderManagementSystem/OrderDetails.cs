using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem {
	/// <summary>
    /// 订单信息类
    /// </summary>
	[Serializable]
	public class OrderDetails {
		/// <summary>
        /// 订单编号
        /// </summary>
		[Key]
		public string OrderNumber { get; set; }

		/// <summary>
        /// 订单时间
        /// </summary>
		public DateTime OrderTime { get; set; }

		/// <summary>
        /// 顾客姓名
        /// </summary>
		public string CustomerName { get; set; }

		/// <summary>
        /// 顾客电话号码
        /// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
        /// 订单所包含的商品List
        /// </summary>
		public List<Goods> Goods { get; set; }

		/// <summary>
        /// 初始化订单
        /// </summary>
		public OrderDetails() {
			// 在初始化订单时自动初始化其包含的商品列表
			Goods = new List<Goods>();
		}

		/// <summary>
		/// 订单类信息显示
		/// </summary>
		/// <returns>订单信息概要</returns>
        public override string ToString() {
			return $"订单号:{OrderNumber} 客户名:{CustomerName} 联系方式:{PhoneNumber}";
		}
	}
}