using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OracleClient;
using static ManagementSystem.ManagementService;

namespace ManagementSystem {
	public partial class SearchDialog : Form {
		public SearchDialog() {
			InitializeComponent();

			bindingSource1.DataSource = GetAllAthletes();
		}

		// 查询按钮
		private void button1_Click(object sender, EventArgs e) {
			string attribute = comboBox1.Text;
			string queryWords = textBox1.Text;
			List<Athletes> result = ManagementService.SearchAthletes(attribute, queryWords);

			if(result.Count == 0) {
				MessageBox.Show("查无此项!");
			} else {
				bindingSource1.DataSource = result;
			}
		}

		// 显示全部按钮
		private void button2_Click(object sender, EventArgs e) {
			bindingSource1.DataSource = GetAllAthletes();
		}

		// 显示运动员信息
		private void button3_Click(object sender, EventArgs e) {
			Athletes athlete = (Athletes) comboBox2.SelectedItem;
			AthletesDialog athletesDialog = new AthletesDialog(athlete);
			athletesDialog.ShowDialog();
		}
	
		// 删除运动员
        private void button4_Click(object sender, EventArgs e) {
	        Athletes athletes = (Athletes) comboBox2.SelectedItem;
			DeleteAthlete(athletes.IDNumber);
			bindingSource1.DataSource = GetAllAthletes();
			bindingSource1.DataSource = GetAllAthletes();
        }

		// 显示修改框
        private void button5_Click(object sender, EventArgs e) {
			ReviseDialog reviseDialog = new ReviseDialog();
			reviseDialog.ShowDialog();
			bindingSource1.DataSource = GetAllAthletes();
        }

		// 显示添加框
        private void button6_Click(object sender, EventArgs e) {
			AddDialog addDialog = new AddDialog();
			addDialog.ShowDialog();
			bindingSource1.DataSource = GetAllAthletes();
        }
    }
}