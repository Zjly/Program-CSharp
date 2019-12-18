using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManagementSystem {
	public static class OrderDatabaseService {
		/// <summary>
		/// 添加订单到数据库
		/// </summary>
		/// <param name="order">订单对象</param>
		public static void AddOrderToDatabase(OrderDetails order) {
			using(var db = new OrderDatabase()) {
				db.OrderDetails.Add(order);
				db.SaveChanges();
			}
		}

		/// <summary>
		/// 获取所有订单
		/// </summary>
		/// <returns>订单列表</returns>
		public static List<OrderDetails> GetAllOrders() {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").ToList<OrderDetails>();
			}
		}

		/// <summary>
		/// 删除订单
		/// </summary>
		/// <param name="orderNumber">订单号</param>
		public static void DeleteOrder(string orderNumber) {
			OrderDetails order;
			using(var db = new OrderDatabase()) {
				order = db.OrderDetails.Include("Goods").SingleOrDefault(o => o.OrderNumber == orderNumber);
				db.Goods.RemoveRange(order.Goods);
				db.OrderDetails.Remove(order);
				db.SaveChanges();
			}
		}

		/// <summary>
		/// 删除货物
		/// </summary>
		/// <param name="orderNumber">订单号</param>
		/// <param name="goodsNumber">货物号</param>
		public static void DeleteGoods(string orderNumber, string goodsNumber) {
			using(var db = new OrderDatabase()) {
				var order = db.OrderDetails.Include("Goods").Where(o => o.OrderNumber == orderNumber).ToList();
				var goods = order[0].Goods.Where(g => g.GoodsNumber == goodsNumber);
				db.Goods.RemoveRange(goods);
				db.SaveChanges();
			}
		}

		/// <summary>
		/// 根据订单号获取订单
		/// </summary>
		/// <param name="orderNumber">订单号</param>
		/// <returns>订单对象</returns>
		public static OrderDetails GetOrder(string orderNumber) {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").SingleOrDefault(o => o.OrderNumber == orderNumber);
			}
		}

		/// <summary>
		/// 根据订单号查找
		/// </summary>
		/// <param name="s">部分订单号</param>
		/// <returns>订单列表</returns>
		public static List<OrderDetails> SearchOrderNumber(string s) {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").Where(o => o.OrderNumber.Contains(s)).ToList();
			}
		}

		/// <summary>
		/// 根据客户查找
		/// </summary>
		/// <param name="s">客户名</param>
		/// <returns>订单列表</returns>
		public static List<OrderDetails> SearchCustomer(string s) {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").Where(o => o.CustomerName.Contains(s)).ToList();
			}
		}

		/// <summary>
		/// 根据商品名查找
		/// </summary>
		/// <param name="s">商品名</param>
		/// <returns></returns>
		public static List<OrderDetails> SearchTradeName(string s) {
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods")
					.Where(o => o.Goods.Where(g => g.TradeName.Contains(s)).Count() > 0).ToList();
			}
		}

		/// <summary>
		/// 根据总价查找
		/// </summary>
		/// <param name="s">总价大于的数字</param>
		/// <returns>订单列表</returns>
		public static List<OrderDetails> SearchTotalPriceMoreThan(string s) {
			double d = Convert.ToDouble(s);
			using(var db = new OrderDatabase()) {
				return db.OrderDetails.Include("Goods").Where(o => o.Goods.Where(g => g.TotalPrice > d).Count() > 0)
					.ToList();
			}
		}

		/// <summary>
		/// 添加订单
		/// </summary>
		/// <param name="order">待添加的订单项</param>
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

		/// <summary>
		/// 更新订单
		/// </summary>
		/// <param name="order"></param>
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