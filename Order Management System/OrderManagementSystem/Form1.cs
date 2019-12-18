using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OrderManagementSystem.OrderService;
using static OrderManagementSystem.OrderDatabaseService;

namespace OrderManagementSystem {
	/// <summary>
	/// 订单管理系统主界面
	/// </summary>
	public partial class Form1 : Form {
		/// <summary>
		/// 订单列表
		/// </summary>
		public static List<OrderDetails> order = new List<OrderDetails>();

		/// <summary>
		/// 只读性空订单 用于刷新dataGridView中的绑定数据
		/// </summary>
		public readonly List<OrderDetails> orderNull = new List<OrderDetails>();

		/// <summary>
		/// 关键字
		/// </summary>
		public string KeyWord { get; set; }

		public Form1() {
			InitializeComponent();

			// 绑定order
			bindingSource1.DataSource = GetAllOrders();

			// 将keyword和查询框绑定
			QueryText.DataBindings.Add("Text", this, "KeyWord");
		}


		/// <summary>
		/// 查询订单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QueryButton_Click(object sender, EventArgs e) {
			// 查询的属性
			string attribute = QueryComboBox.Text;

			// 结果订单
			List<OrderDetails> result = new List<OrderDetails>();

			// 通过不同属性得到不同查询结果
			switch(attribute) {
				case "订单号":
					result = SearchOrderNumber(KeyWord);
					break;
				case "购买人":
					result = SearchCustomer(KeyWord);
					break;
				case "商品名":
					result = SearchTradeName(KeyWord);
					break;
				case "总价大于":
					result = SearchTotalPriceMoreThan(KeyWord);
					break;
			}


			if(result.Count == 0) {
				// 无返回结果
				MessageBox.Show("查无此项!");
			} else {
				// 设置内容为返回的查询结果
				bindingSource1.DataSource = result;
			}
		}

		/// <summary>
		/// 删除订单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeleteButton_Click(object sender, EventArgs e) {
			// 得到选定的待删除的订单
			OrderDetails selectOrder = (OrderDetails) DeleteComboBox1.SelectedItem;

			// 确认框
			DialogResult result =
				MessageBox.Show("确定删除订单[" + selectOrder + "]吗？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if(result == DialogResult.Yes) {
				// 删除订单操作
				DeleteOrder(selectOrder.OrderNumber);

				// 重新得到现有订单并显示
				bindingSource1.DataSource = GetAllOrders();
			}
		}

		/// <summary>
		/// 删除货物
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeleteButton2_Click(object sender, EventArgs e) {
			// 得到选定的订单
			OrderDetails selectOrder = (OrderDetails) DeleteComboBox1.SelectedItem;

			// 得到选定的待删除的货物
			Goods selectGoods = (Goods) DeleteComboBox2.SelectedItem;

			// 确认框
			DialogResult result =
				MessageBox.Show("确定删除货物[" + selectGoods + "]吗？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if(result == DialogResult.Yes) {
				// 删除货物操作
				DeleteGoods(selectOrder.OrderNumber, selectGoods.GoodsNumber);

				// 如若删除该货物后该订单为空则删除该订单
				if(GetOrder(selectOrder.OrderNumber).Goods.Count == 0) {
					DeleteOrder(selectOrder.OrderNumber);
				}

				// 重新得到现有订单并显示
				bindingSource1.DataSource = GetAllOrders();
			}
		}

		/// <summary>
		/// 显示所有订单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DisplayButton_Click(object sender, EventArgs e) {
			// 重新得到现有订单并显示
			bindingSource1.DataSource = GetAllOrders();
		}

		/// <summary>
		/// 将订单导出到文件保存
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveButton_Click(object sender, EventArgs e) {
			ExportDataToXml(@"..\..\..\.\files\OrderListXml.xml");
		}

		/// <summary>
		/// 添加订单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddButton_Click(object sender, EventArgs e) {
			// 新建一个add的窗口并显示
			Add add = new Add();
			add.ShowDialog();

			// 关闭窗口后重新得到现有订单并显示
			bindingSource1.DataSource = GetAllOrders();
		}

		/// <summary>
		/// 添加货物
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddButton2_Click(object sender, EventArgs e) {
			// 新建一个add的窗口并显示，并将选中的订单（待添加货物所属订单）传参
			Add add = new Add((OrderDetails) DeleteComboBox1.SelectedItem);
			add.ShowDialog();

			// 关闭窗口后重新得到现有订单并显示
			bindingSource1.DataSource = GetAllOrders();
		}

		/// <summary>
		/// 修改订单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReviseButton_Click(object sender, EventArgs e) {
			Revise revise = new Revise(GetAllOrders());
			revise.ShowDialog();
			bindingSource1.DataSource = GetAllOrders();
		}

		/// <summary>
		/// 刷新dataGridView中数据
		/// </summary>
		public void RefreshBinding() {
			bindingSource1.DataSource = orderNull;
			bindingSource1.DataSource = order;
		}

		/// <summary>
		/// 导出为HTML文档
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e) {
			XslTransform(order, @"..\..\OrderXSLT.xslt");
		}
	}
}