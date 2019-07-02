using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ManagementSystem.ManagementService;

namespace ManagementSystem {
    public partial class AddDialog : Form {
        public AddDialog() {
            InitializeComponent();

            dataGridView1.DataSource = GetAllAthletes();
        }

		// 添加运动员信息
        private void button1_Click(object sender, EventArgs e) {
			Athletes athlete = new Athletes();
			athlete.IDNumber = textBox1.Text;
			athlete.Name = textBox2.Text;
			athlete.Sex = textBox3.Text;
			athlete.Age = Convert.ToInt32(textBox4.Text);
			athlete.AthleteNumber = Convert.ToInt32(textBox5.Text);
			athlete.Team = textBox6.Text;
			athlete.Event = textBox7.Text;
			AddAthlete(athlete);
			dataGridView1.DataSource = GetAllAthletes();
        }
    }
}
