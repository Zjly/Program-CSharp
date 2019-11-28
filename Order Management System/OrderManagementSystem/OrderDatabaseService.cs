using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManagementSystem {
	public static class OrderDatabaseService {
		// 分割字符串并组合成订单
		public static void AddOrder(string line) {
			// 分割字符串
			line = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(line, "	");
			string[] values = line.Split('	');

			if(values.Length < 8) {
				throw new Exception("存在未填写的条目或条目格式错误, 详细格式请查看帮助!");
			}

			try {
				OrderDetails orderDetails = GetOrder(values[1]);
				if(orderDetails == null) {
					orderDetails = AddNewOrder(values);
					AddOrderToDatabase(orderDetails);
				} else {
					AddGoods(orderDetails, values);
				}
			} catch(Exception e) {
				MessageBox.Show("失败原因为:\n" + e, "导入失败");
			}
		}

		// 添加新的订单号
		private static OrderDetails AddNewOrder(string[] values) {
			OrderDetails orderDetails = new OrderDetails();
			orderDetails.OrderTime = Convert.ToDateTime(values[0]);
			orderDetails.OrderNumber = values[1];
			orderDetails.CustomerName = values[6];
			orderDetails.PhoneNumber = values[7];

			Goods goods = new Goods();
			goods.TradeName = values[2];
			goods.Type = values[3];
			goods.UnitPrice = Convert.ToDouble(values[4]);
			goods.Count = Convert.ToInt32(values[5]);
			goods.TotalPrice = goods.UnitPrice * goods.Count;
			orderDetails.Goods.Add(goods);

			return orderDetails;
		}

		// 在同一订单号下添加货物
		private static void AddGoods(OrderDetails orderDetails, string[] values) {
			Goods goods = new Goods();
			goods.TradeName = values[2];
			goods.Type = values[3];
			goods.UnitPrice = Convert.ToDouble(values[4]);
			goods.Count = Convert.ToInt32(values[5]);
			goods.TotalPrice = goods.UnitPrice * goods.Count;

			using(var db = new OrderDatabase()) {
				db.OrderDetails.Attach(orderDetails);
				orderDetails.Goods.Add(goods);
				db.SaveChanges();
			}
		}

		// 添加订单到数据库
		public static void AddOrderToDatabase(OrderDetails order) {
			using(var db = new OrderDatabase()) {
				db.OrderDetails.Add(order);
				db.SaveChanges();
			}
		}

		// 获取所有订单
		public static List<OrderDetails> GetAllOrders() {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").ToList<OrderDetails>();
			}
		}

		// 删除订单
		public static void DeleteOrder(string orderNumber) {
			using(var db = new OrderDatabase()) {
				var order = db.OrderDetails.Include("Goods").SingleOrDefault(o => o.OrderNumber == orderNumber);
				db.Goods.RemoveRange(order.Goods);
				db.OrderDetails.Remove(order);
				db.SaveChanges();
			}
		}

		// 删除货物
		public static void DeleteGoods(string orderNumber, string goodsNumber) {
			using(var db = new OrderDatabase()) {
				var order = db.OrderDetails.Include("Goods").Where(o => o.OrderNumber == orderNumber).ToList();
				var goods = order[0].Goods.Where(g => g.GoodsNumber == goodsNumber);
				db.Goods.RemoveRange(goods);
				db.SaveChanges();
			}
		}

		// 根据订单号获取订单
		public static OrderDetails GetOrder(string orderNumber) {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").SingleOrDefault(o => o.OrderNumber == orderNumber);
			}
		}

		// 根据订单号查找
		public static List<OrderDetails> SearchOrderNumber(string s) {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").Where(o => o.OrderNumber.Contains(s)).ToList();
			}
		}

		// 根据客户查找
		public static List<OrderDetails> SearchCustomer(string s) {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").Where(o => o.CustomerName.Contains(s)).ToList();
			}
		}

		// 根据商品名查找
		public static List<OrderDetails> SearchTradeName(string s) {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods")
					.Where(o => o.Goods.Where(g => g.TradeName.Contains(s)).Count() > 0).ToList();
			}
		}

		// 根据总价查找
		public static List<OrderDetails> SearchTotalPriceMoreThan(string s) {
			double d = Convert.ToDouble(s);
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").Where(o => o.Goods.Where(g => g.TotalPrice > d).Count() > 0)
					.ToList();
			}
		}

		// 添加订单
		public static void AddOrder(OrderDetails order) {
			OrderDetails orderExist = GetOrder(order.OrderNumber);
			if(orderExist == null) {
				AddOrderToDatabase(order);
			} else {
				using(var db = new OrderDatabase()) {
					db.OrderDetails.Attach(orderExist);
					foreach(var goods in order.Goods) {
						orderExist.Goods.Add(goods);
					}

					db.SaveChanges();
				}
			}
		}

		// 修改订单
		public static void UpdateOrder(OrderDetails order) {
			using(var db = new OrderDatabase()) {
				db.OrderDetails.Attach(order);
				db.Entry(order).State = EntityState.Modified;
				order.Goods.ForEach(
					item => db.Entry(item).State = EntityState.Modified);
				db.SaveChanges();
			}
		}
	}
}