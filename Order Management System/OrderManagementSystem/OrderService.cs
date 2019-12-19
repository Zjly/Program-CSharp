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
using static OrderManagementSystem.OrderDatabaseService;

namespace OrderManagementSystem {
	public static class OrderService {
		/// <summary>
		/// 从程序中导入数据
		/// </summary>
		/// <param name="list">订单列表</param>
		/// <param name="line">数据行</param>
		public static void AddOrder(List<OrderDetails> list, string line) {
			OrderDetails orderDetails = new OrderDetails();
			// 分割字符串
			line = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(line, "	");
			string[] values = line.Split('	');

			// 导入数据
			try {
				// 判断订单号是否正确
				if(!OrderNumberCorrect(Convert.ToDateTime(values[0]), values[1])) {
					throw new Exception("订单号格式错误，订单号格式应为8位日期+三位数字");
				}

				// 判断联系方式是否正确
				if(!PhoneNumberCorrect(values[7])) {
					throw new Exception("联系方式格式错误，联系方式格式应为手机号或区号+本地号码");
				}

				List<OrderDetails> orderNumberDuplication = SearchOrderNumber(list, values[1], 2);
				// 同一订单下添加货物 同一订单默认为同一时间同一订单号同一顾客 否则加入失败
				if(orderNumberDuplication.Count != 0) {
					bool sameOrder =
						orderNumberDuplication[0].OrderTime == Convert.ToDateTime(values[0]) &&
						orderNumberDuplication[0].CustomerName == values[6] &&
						orderNumberDuplication[0].PhoneNumber == values[7];
					if(!sameOrder) {
						throw new Exception("若要在同一订单下添加货物，则默认为同一时间、同一订单号和同一顾客，否则加入失败!");
					}

					InputDataUnderSameOrder(orderNumberDuplication[0], values);
				}
				// 不同订单下添加货物
				else {
					InputData(orderDetails, values);
					list.Add(orderDetails);
				}
			} catch(Exception e) {
				MessageBox.Show("订单导入失败:\n" + e.Message);
			}
		}

		/// <summary>
		/// 导入数据具体操作 各条目导入
		/// </summary>
		/// <param name="orderDetails">订单对象</param>
		/// <param name="values">数据数组</param>
		private static void InputData(OrderDetails orderDetails, string[] values) {
			if(values.Length < 8) {
				throw new Exception("存在未填写的条目或条目格式错误");
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

		/// <summary>
		/// 同一订单内导入
		/// </summary>
		/// <param name="orderDetails">订单项</param>
		/// <param name="values">数据数组</param>
		private static void InputDataUnderSameOrder(OrderDetails orderDetails, string[] values) {
			if(values.Length < 8) {
				throw new Exception("存在未填写的条目或条目格式错误");
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

		/// <summary>
		/// 将订单序列化为xml文件
		/// </summary>
		/// <param name="file"></param>
		public static void ExportDataToXml(string file) {
			List<OrderDetails> list = GetAllOrders();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<OrderDetails>));
			using(FileStream fileStream = new FileStream(file, FileMode.Create)) {
				xmlSerializer.Serialize(fileStream, list);
			}
		}

		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="list">订单列表</param>
		/// <param name="index1">索引1</param>
		/// <param name="index2">索引2</param>
		/// <param name="attribute">修改属性</param>
		/// <param name="s">修改值</param>
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
							throw new Exception("订单号格式错误，订单号格式应为8位日期+三位数字");
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
							throw new Exception("联系方式格式错误，联系方式格式应为手机号或区号+本地号码");
						}

						list[index1].CustomerName = values[0];
						list[index1].PhoneNumber = values[1];
						break;
				}
			} catch(Exception e) {
				MessageBox.Show("订单修改失败:\n" + e.Message);
			}
		}

		/// <summary>
		/// 根据订单号搜索
		/// </summary>
		/// <param name="list"></param>
		/// <param name="s"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
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
			ExportDataToXml(@"./OrderListXmlToXSLT.xml");
			try {
				XmlDocument doc = new XmlDocument();
				doc.Load(@"./OrderListXmlToXSLT.xml");

				XPathNavigator nav = doc.CreateNavigator();
				nav.MoveToRoot();

				XslTransform xt = new XslTransform();
				xt.Load(xslt);

				FileStream outFileStream = File.Create(@"./Order.html");
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