using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace OrderManagementSystem {
	public static class OrderService {
		// 从程序中导入数据
		public static void AddOrder(ref List<OrderDetails> list, string line) {
			OrderDetails orderDetails = new OrderDetails();
			// 分割字符串
			line = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(line, "	");
			string[] values = line.Split('	');

			// 导入数据
			try {
				// 判断订单号是否正确
				if(!OrderNumberCorrect(Convert.ToDateTime(values[0]), values[1])) {
					throw new Exception("订单号格式错误，详细格式请查看帮助!");
				}

				// 判断联系方式是否正确
				if(!PhoneNumberCorrect(values[7])) {
					throw new Exception("联系方式格式错误，详细格式请查看帮助!");
				}

				List<OrderDetails> orderNumberDuplication = SearchOrderNumber(list, values[1], 2);
				// 同一订单下添加货物 同一订单默认为同一时间同一订单号同一顾客 否则加入失败
				if(orderNumberDuplication.Count != 0) {
					bool sameOrder =
						orderNumberDuplication[0].OrderTime == Convert.ToDateTime(values[0]) &&
						orderNumberDuplication[0].CustomerName == values[6] &&
						orderNumberDuplication[0].PhoneNumber == values[7];
					if(!sameOrder) {
						throw new Exception("在同一订单中添加货物功能: \n若要在同一订单下添加货物，则默认为同一时间、同一订单号和同一顾客，否则加入失败!");
					}

					InputDataUnderSameOrder(orderNumberDuplication[0], values);
				}
				// 不同订单下添加货物
				else {
					InputData(orderDetails, values);
					list.Add(orderDetails);
				}
			} catch(Exception e) {
				MessageBox.Show("失败原因为:\n" + e, "导入失败");
			}
		}

		// 导入数据具体操作 各条目导入
		private static void InputData(OrderDetails orderDetails, string[] values) {
			if(values.Length < 8) {
				throw new Exception("存在未填写的条目或条目格式错误, 详细格式请查看帮助!");
			}

			orderDetails.OrderTime = Convert.ToDateTime(values[0]);
			orderDetails.OrderNumber = values[1];

			Goods goods = new Goods();
			goods.TradeName = values[2];
			goods.Type = values[3];
			goods.UnitPrice = Convert.ToDouble(values[4]);
			goods.Count = Convert.ToInt32(values[5]);
			goods.TotalPrice = goods.UnitPrice * goods.Count;
			orderDetails.CustomerName = values[6];
			orderDetails.PhoneNumber = values[7];
			orderDetails.Goods.Add(goods);
		}

		// 同一订单内导入
		private static void InputDataUnderSameOrder(OrderDetails orderDetails, string[] values) {
			if(values.Length < 8) {
				throw new Exception("存在未填写的条目或条目格式错误, 详细格式请查看帮助!");
			}

			Goods goods = new Goods();
			goods.TradeName = values[2];
			goods.Type = values[3];
			goods.UnitPrice = Convert.ToDouble(values[4]);
			goods.Count = Convert.ToInt32(values[5]);
			goods.TotalPrice = goods.UnitPrice * goods.Count;
			orderDetails.CustomerName = values[6];
			orderDetails.PhoneNumber = values[7];
			orderDetails.Goods.Add(goods);
		}

		// 反序列化xml文件并导入
		public static void ImportDataFromXml(ref List<OrderDetails> list, string file) {
			if(!file.Contains(".xml")) {
				MessageBox.Show("导入文件格式错误");
				return;
			}

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<OrderDetails>));
			using(FileStream fileStream = new FileStream(file, FileMode.Open)) {
				list = (List<OrderDetails>) xmlSerializer.Deserialize(fileStream);
			}
		}

		// 将订单序列化为xml文件
		public static void ExportDataToXml(List<OrderDetails> list, string file) {
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<OrderDetails>));
			using(FileStream fileStream = new FileStream(file, FileMode.Create)) {
				xmlSerializer.Serialize(fileStream, list);
			}
		}

		// 删除订单
		public static void Delete(ref List<OrderDetails> list, OrderDetails orderDetails) {
			list.Remove(orderDetails);
		}

		// 删除订单中货物
		public static void DeleteGoods(OrderDetails orderDetails, Goods goods) {
			orderDetails.Goods.Remove(goods);
		}

		// 修改订单
		public static void ReviseOrder(List<OrderDetails> list, int index1, int index2, string attribute, string s) {
			// 获取要修改的条目并修改
			try {
				switch(attribute) {
					case "购买时间":
						list[index1].OrderTime = Convert.ToDateTime(s);
						break;
					case "订单号":
						// 判断订单号是否正确
						if(!OrderNumberCorrect(list[index1].OrderTime, s)) {
							throw new Exception("订单号格式错误，详细格式请查看帮助!");
						}

						list[index1].OrderNumber = s;
						break;
					case "商品名":
						list[index1].Goods[index2].TradeName = s;
						break;
					case "类型":
						list[index1].Goods[index2].Type = s;
						break;
					case "数量":
						list[index1].Goods[index2].Count = Int32.Parse(s);
						list[index1].Goods[index2].TotalPrice =
							list[index1].Goods[index2].UnitPrice * list[index1].Goods[index2].Count;
						break;
					case "单价":
						list[index1].Goods[index2].UnitPrice = Double.Parse(s);
						list[index1].Goods[index2].TotalPrice =
							list[index1].Goods[index2].UnitPrice * list[index1].Goods[index2].Count;
						break;
					case "客户信息":
						// 判断联系方式是否正确
						string[] values = s.Split('	');
						if(!PhoneNumberCorrect(values[1])) {
							throw new Exception("联系方式格式错误，详细格式请查看帮助!");
						}

						list[index1].CustomerName = values[0];
						list[index1].PhoneNumber = values[1];
						break;
				}
			} catch(Exception e) {
				MessageBox.Show("失败原因为:\n" + e, "修改失败");
			}
		}

		// 根据订单号搜索
		public static List<OrderDetails> SearchOrderNumber(List<OrderDetails> list, string s, int mode) {
			IEnumerable<OrderDetails> result = new List<OrderDetails>();
			// mode为搜索模式，1为模糊匹配，2为精确搜索
			switch(mode) {
				case 1:
					result = list.Where(n => n.OrderNumber.Contains(s));
					break;
				case 2:
					result = list.Where(n => n.OrderNumber == s);
					break;
			}

			return result.ToList();
		}

		// 根据顾客姓名搜索
		public static List<OrderDetails> SearchCustomer(List<OrderDetails> list, string s) {
			IEnumerable<OrderDetails> result = list.Where(n => n.CustomerName.Contains(s));
			return result.ToList();
		}

		// 根据商品名搜索
		public static List<OrderDetails> SearchTradeName(List<OrderDetails> list, string s) {
			// 找到符合条件的订单
			IEnumerable<OrderDetails> result =
				list.Where(n => n.Goods.Where(m => m.TradeName.Contains(s)).Count() > 0);
			List<OrderDetails> resultList = result.ToList();
			// 删除订单中不符合条件的货物
			for(int i = 0; i < resultList.Count; i++) {
				OrderDetails order = resultList[i];
				for(int j = 0; j < order.Goods.Count; j++) {
					Goods goods = order.Goods[j];
					if(!goods.TradeName.Contains(s)) {
						DeleteGoods(order, goods);
						j--;
					}
				}
			}

			// 最后返回的订单中的货物全部是满足条件的
			return resultList;
		}

		// 根据类型搜索
		public static List<OrderDetails> SearchType(List<OrderDetails> list, string s) {
			IEnumerable<OrderDetails> result = list.Where(n => n.Goods.Where(m => m.Type.Contains(s)).Count() > 0);
			List<OrderDetails> resultList = result.ToList();
			for(int i = 0; i < resultList.Count; i++) {
				OrderDetails order = resultList[i];
				for(int j = 0; j < order.Goods.Count; j++) {
					Goods goods = order.Goods[j];
					if(!goods.Type.Contains(s)) {
						DeleteGoods(order, goods);
						j--;
					}
				}
			}

			return resultList;
		}

		// 搜索价格大于某个数字的货物
		public static List<OrderDetails> SearchPriceMoreThan(List<OrderDetails> list, string s) {
			double price;
			try {
				price = Convert.ToDouble(s);
			} catch(Exception) {
				MessageBox.Show("价格格式错误!");
				price = 0;
			}

			IEnumerable<OrderDetails> result =
				list.Where(n => n.Goods.Where(m => (m.TotalPrice >= price)).Count() > 0);
			List<OrderDetails> resultList = result.ToList();
			for(int i = 0; i < resultList.Count; i++) {
				OrderDetails order = resultList[i];
				for(int j = 0; j < order.Goods.Count; j++) {
					Goods goods = order.Goods[j];
					if(goods.TotalPrice < price) {
						DeleteGoods(order, goods);
						j--;
					}
				}
			}

			return resultList;
		}

		// 将两个list合并
		public static void Combine(ref List<OrderDetails> list1, List<OrderDetails> list2) {
			for(int i = 0; i < list2.Count; i++) {
				List<OrderDetails> orderNumberDuplication = SearchOrderNumber(list1, list2[i].OrderNumber, 2);
				// 如果有订单号相同的则添加到该订单中
				if(orderNumberDuplication.Count != 0) {
					for(int j = 0; j < list2[i].Goods.Count; j++) {
						list2[i].Goods[j].GoodsNumber =
							orderNumberDuplication[0].Goods[orderNumberDuplication[0].Goods.Count - 1].GoodsNumber +
							1; // 确定货物序号
						DeepCopy(orderNumberDuplication[0], list2[i].Goods[j]);
					}
				}
				// 不同则新建订单
				else {
					DeepCopy(ref list1, list2[i]);
				}
			}
		}

		// 将list2深拷贝到list1中
		public static void DeepCopy(ref List<OrderDetails> list1, List<OrderDetails> list2) {
			foreach(OrderDetails n in list2) {
				OrderDetails orderDetails = new OrderDetails();
				orderDetails.OrderTime = n.OrderTime;
				orderDetails.OrderNumber = n.OrderNumber;
				orderDetails.CustomerName = n.CustomerName;
				orderDetails.PhoneNumber = n.PhoneNumber;
				foreach(Goods m in n.Goods) {
					Goods goods = new Goods();
					goods.GoodsNumber = m.GoodsNumber;
					goods.TradeName = m.TradeName;
					goods.Type = m.Type;
					goods.UnitPrice = m.UnitPrice;
					goods.Count = m.Count;
					goods.TotalPrice = m.TotalPrice;
					orderDetails.Goods.Add(goods);
				}

				list1.Add(orderDetails);
			}
		}

		// 将一条订单深拷贝到list1中
		public static void DeepCopy(ref List<OrderDetails> list1, OrderDetails order) {
			OrderDetails orderDetails = new OrderDetails();
			orderDetails.OrderTime = order.OrderTime;
			orderDetails.OrderNumber = order.OrderNumber;
			orderDetails.CustomerName = order.CustomerName;
			orderDetails.PhoneNumber = order.PhoneNumber;
			foreach(Goods m in order.Goods) {
				Goods goods = new Goods();
				goods.GoodsNumber = m.GoodsNumber;
				goods.TradeName = m.TradeName;
				goods.Type = m.Type;
				goods.UnitPrice = m.UnitPrice;
				goods.Count = m.Count;
				goods.TotalPrice = m.TotalPrice;
				orderDetails.Goods.Add(goods);
			}

			list1.Add(orderDetails);
		}

		// 将一件货物深拷贝到订单中
		public static void DeepCopy(OrderDetails order, Goods m) {
			Goods goods = new Goods();
			goods.GoodsNumber = m.GoodsNumber;
			goods.TradeName = m.TradeName;
			goods.Type = m.Type;
			goods.UnitPrice = m.UnitPrice;
			goods.Count = m.Count;
			goods.TotalPrice = m.TotalPrice;
			order.Goods.Add(goods);
		}

		// 检查订单号是否符合要求 标准为yyyymmdd+三位数字
		private static bool OrderNumberCorrect(DateTime dateTime, string s) {
			// 提取订单时间中的日期信息
			string year = dateTime.Year.ToString();
			string month = dateTime.Month.ToString();
			string day = dateTime.Day.ToString();
			// 转化为yyyymmdd的标准格式
			month = month.Length == 1 ? "0" + month : month;
			day = day.Length == 1 ? "0" + day : day;
			// 建立模式字符串
			string pattern = "^";
			pattern += $"{year}{month}{day}";
			pattern += "[0-9]{3}$";
			// 正则表达式匹配
			if(Regex.IsMatch(s, pattern)) {
				return true;
			}

			return false;
		}

		// 检查电话号码是否符合要求 区号+7位或8位电话号码或是手机号
		private static bool PhoneNumberCorrect(string s) {
			string pattern1 = "^1[0-9]{10}$";
			string pattern2 = "^0[0-9]{1,3}-[0-9]{7,8}$";
			if(Regex.IsMatch(s, pattern1) || Regex.IsMatch(s, pattern2)) {
				return true;
			}

			return false;
		}

		// xslt转化
		public static void XslTransform(List<OrderDetails> list, string xslt) {
			ExportDataToXml(list, "OrderListXmlToXSLT.xml");
			try {
				XmlDocument doc = new XmlDocument();
				doc.Load("OrderListXmlToXSLT.xml");

				XPathNavigator nav = doc.CreateNavigator();
				nav.MoveToRoot();

				XslTransform xt = new XslTransform();
				xt.Load(xslt);

				FileStream outFileStream = File.Create(@"..\..\Order.html");
				XmlTextWriter writer = new XmlTextWriter(outFileStream, System.Text.Encoding.UTF8);

				xt.Transform(nav, null, writer);

				outFileStream.Close();
				//System.Diagnostics.Process.Start(@"..\..\Order.html");
			} catch(XmlException e) {
				MessageBox.Show("XML异常: \n" + e);
			} catch(XsltException e) {
				MessageBox.Show("XSLT异常: \n" + e);
			}
		}
	}
}