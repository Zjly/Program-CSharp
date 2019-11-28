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
	/// <summary>
	/// 订单管理系统添加界面
	/// </summary>
	public partial class Add : Form {
		/// <summary>
		/// 订单列表
		/// </summary>
		public static List<OrderDetails> order = new List<OrderDetails>();

		/// <summary>
		/// 只读性空订单 用于刷新dataGridView中的绑定数据
		/// </summary>
		public readonly List<OrderDetails> orderNull = new List<OrderDetails>();

		/// <summary>
		/// 主界面类对象
		/// </summary>
		private Form1 form1;

		/// <summary>
		/// 默认构造函数
		/// </summary>
		public Add() {
			InitializeComponent();
		}

		/// <summary>
		/// 构造函数，传入form1参数
		/// </summary>
		/// <param name="form1">主界面类对象</param>
		public Add(Form1 form1) {
			InitializeComponent();
			this.form1 = form1;

			// 显示添加订单的信息
			AddBindingSource.DataSource = order;
		}

		/// <summary>
		/// 构造函数，传入添加货物所需的订单信息
		/// </summary>
		/// <param name="form1">主界面类对象</param>
		/// <param name="selectOrder">待添加货物的订单</param>
		public Add(Form1 form1, OrderDetails selectOrder) {
			InitializeComponent();
			AddBindingSource.DataSource = order;
			this.form1 = form1;

			// 导入货物所属订单的信息
			dateTimePicker1.Text = selectOrder.OrderTime.ToString();
			textBox2.Text = selectOrder.OrderNumber;
			textBox7.Text = selectOrder.CustomerName;
			textBox8.Text = selectOrder.PhoneNumber;
		}

		/// <summary>
		/// 添加订单
		/// 添加各条目订单数据 组合成可供识别导入的string
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e) {
			// 待添加订单信息拼合
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
			AddOrder(order, s);

			// 重新绑定显示订单信息
			RefreshBinding();
		}

		/// <summary>
		/// 显示帮助
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e) {
			MessageBox.Show
			("添加订单:\n" + "方法1: 在输入框中按正确格式输入各条目内容后，点击添加按钮；\n" +
			 "方法2: 在表格内双击空行的单元格，输入各条目内容；\n" +
			 "方法3: 在一次性导入中按照\"购买时间 订单号 商品名 类型" +
			 " 数量 单价 购买人 联系方式\"格式输入后，点击添加按钮.\n" +
			 "注: \n若要在同一订单下添加货物，则默认为同一时间、同一订单号和同一顾客，否则加入失败；\n" +
			 "订单号格式为8位日期+三位数字，联系方式格式为手机号或区号+本地号码", "帮助");
		}

		/// <summary>
		/// 一次性导入订单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e) {
			// 添加订单
			AddOrder(order, textBox1.Text);

			// 重新绑定显示订单信息
            RefreshBinding();
		}

		/// <summary>
		/// 保存订单的添加
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveButton_Click(object sender, EventArgs e) {
			// 将当前order添加到数据库中
			foreach(var orderDetails in order) {
				AddOrder(orderDetails);
			}

			Close();
		}

		/// <summary>
		/// 刷新dataGridView中数据
		/// </summary>
		public void RefreshBinding() {
			AddBindingSource.DataSource = orderNull;
			AddBindingSource.DataSource = order;
		}

		/// <summary>
		/// 关闭窗口
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Add_FormClosed(object sender, FormClosedEventArgs e) {
			order = new List<OrderDetails>();
		}
	}
}