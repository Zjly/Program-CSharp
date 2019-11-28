using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OrderManagementSystem.OrderDatabaseService;
using static OrderManagementSystem.OrderService;

namespace OrderManagementSystem {
	public partial class Revise : Form {
		public static List<OrderDetails> order = new List<OrderDetails>();
		public readonly List<OrderDetails> orderNull = new List<OrderDetails>();
		private Form1 form1;

		public Revise() {
			InitializeComponent();
		}

		public Revise(Form1 form1) {
			InitializeComponent();
			DeepCopy(ref order, Form1.order);
			ReviseBindingSource.DataSource = order;
			this.form1 = form1;
		}

		public Revise(List<OrderDetails> orderList) {
			InitializeComponent();
			ReviseBindingSource.DataSource = orderList;
			order = orderList;
		}

		// 进行修改
		private void button1_Click(object sender, EventArgs e) {
			int index1 = dataGridView1.CurrentRow.Index;
			int index2 = dataGridView2.CurrentRow.Index;
			string attribute = comboBox2.Text;
			string s = textBox1.Text;
			ReviseOrder(order, index1, index2, attribute, s);
			// 更新绑定
			ReviseBindingSource.DataSource = orderNull;
			ReviseBindingSource.DataSource = order;
		}

		// 帮助提示
		private void button2_Click(object sender, EventArgs e) {
			MessageBox.Show
			("修改订单:\n" + "方法1: 选择要修改的订单并选择修改条目后，在输入框中输入修改的内容后" +
			 "，点击修改按钮；\n" + "方法2: 在表格内双击要修改的的单元格，输入修改的内容.", "帮助");
		}

		// 保存修改 将修改完成的订单深拷贝到原订单上
		private void button3_Click(object sender, EventArgs e) {
			foreach(OrderDetails orderDetails in order) {
				UpdateOrder(orderDetails);
			}

			Close();
		}

		// 显示要修改的内容
		private void AttributeBinding() {
			label4.DataBindings.Clear();
			string s = comboBox2.Text;
			switch(s) {
				case "购买时间":
					label4.DataBindings.Add("Text", ReviseBindingSource, "OrderTime");
					break;
				case "订单号":
					label4.DataBindings.Add("Text", ReviseBindingSource, "OrderNumber");
					break;
				case "商品名":
					label4.DataBindings.Add("Text", bindingSource1, "TradeName");
					break;
				case "类型":
					label4.DataBindings.Add("Text", bindingSource1, "Type");
					break;
				case "数量":
					label4.DataBindings.Add("Text", bindingSource1, "Count");
					break;
				case "单价":
					label4.DataBindings.Add("Text", bindingSource1, "UnitPrice");
					break;
				case "顾客姓名":
					label4.DataBindings.Add("Text", ReviseBindingSource, "CustomerName");
					break;
				case "联系方式":
					label4.DataBindings.Add("Text", ReviseBindingSource, "PhoneNumber");
					break;
			}
		}

		// 选择改变后调用函数 改变修改内容的显示
		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
			AttributeBinding();
		}

		// 关闭窗口
		private void Revise_FormClosed(object sender, FormClosedEventArgs e) {
			order = new List<OrderDetails>();
		}
	}
}