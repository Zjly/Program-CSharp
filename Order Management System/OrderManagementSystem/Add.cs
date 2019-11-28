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
	public partial class Add : Form {
		public static List<OrderDetails> order = new List<OrderDetails>();
		public readonly List<OrderDetails> orderNull = new List<OrderDetails>();
		private Form1 form1;

		public Add() {
			InitializeComponent();
		}

		// 传入form1参数
		public Add(Form1 form1) {
			InitializeComponent();
			this.form1 = form1;
			AddBindingSource.DataSource = order;
		}

		// 添加货物
		public Add(Form1 form1, OrderDetails selectOrder) {
			InitializeComponent();
			AddBindingSource.DataSource = order;
			this.form1 = form1;
			dateTimePicker1.Text = selectOrder.OrderTime.ToString();
			textBox2.Text = selectOrder.OrderNumber;
			textBox7.Text = selectOrder.CustomerName;
			textBox8.Text = selectOrder.PhoneNumber;
		}

		// 添加各条目订单数据 组合成可供识别导入的string
		private void button1_Click(object sender, EventArgs e) {
			string s = "";
			s += dateTimePicker1.Text + " ";
			s += textBox2.Text + " ";
			s += textBox3.Text + " ";
			s += textBox4.Text + " ";
			s += textBox5.Text + " ";
			s += textBox6.Text + " ";
			s += textBox7.Text + " ";
			s += textBox8.Text;
			// 添加订单
			AddOrder(ref order, s);
			AddBindingSource.DataSource = orderNull;
			AddBindingSource.DataSource = order;
		}

		// 显示帮助
		private void button2_Click(object sender, EventArgs e) {
			MessageBox.Show
			("添加订单:\n" + "方法1: 在输入框中按正确格式输入各条目内容后，点击添加按钮；\n" +
			 "方法2: 在表格内双击空行的单元格，输入各条目内容；\n" +
			 "方法3: 在一次性导入中按照\"购买时间 订单号 商品名 类型" +
			 " 数量 单价 购买人 联系方式\"格式输入后，点击添加按钮.\n" +
			 "注: \n若要在同一订单下添加货物，则默认为同一时间、同一订单号和同一顾客，否则加入失败；\n" +
			 "订单号格式为8位日期+三位数字，联系方式格式为手机号或区号+本地号码", "帮助");
		}

		// 一次性导入订单
		private void button3_Click(object sender, EventArgs e) {
			AddOrder(ref order, textBox1.Text);
			AddBindingSource.DataSource = orderNull;
			AddBindingSource.DataSource = order;
		}

		// 保存添加
		private void SaveButton_Click(object sender, EventArgs e) {
			foreach(var orderDetails in order) {
				AddOrder(orderDetails);
			}

			Close();
		}

		// 关闭窗口
		private void Add_FormClosed(object sender, FormClosedEventArgs e) {
			order = new List<OrderDetails>();
		}
	}
}