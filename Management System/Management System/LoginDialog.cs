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
    public partial class LoginDialog : Form {
        public LoginDialog() {
            InitializeComponent();
        }

		// 登录系统
        private void button1_Click(object sender, EventArgs e) {
	        Hide(); // 暂且将其隐藏

			SearchDialog search = new SearchDialog();
			search.ShowDialog();

			Dispose();
        }
    }
}
