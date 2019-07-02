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
    public partial class ReviseDialog : Form {
        public ReviseDialog() {
            InitializeComponent();

            bindingSource1.DataSource = GetAllAthletes();
        }

        private void button1_Click(object sender, EventArgs e) {
	        string attribute = comboBox2.Text;
	        string text = textBox1.Text;
	        Athletes athletes = (Athletes)comboBox1.SelectedItem;
			ReviseAthlete(athletes.IDNumber, attribute, text);
			bindingSource1.DataSource = GetAllAthletes();
        }
    }
}
