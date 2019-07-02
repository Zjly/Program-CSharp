using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementSystem {
    public partial class AthletesDialog : Form {
        public AthletesDialog() {
            InitializeComponent();
        }

        public AthletesDialog(Athletes athletes) {
	        InitializeComponent();

            label9.Text = athletes.Name;
	        label10.Text = athletes.Age.ToString();
	        label11.Text = athletes.IDNumber;
	        label12.Text = athletes.Team;
	        label13.Text = athletes.Event;
	        label14.Text = athletes.Sex;
	        label15.Text = athletes.AthleteNumber.ToString();
        }
    }
}
