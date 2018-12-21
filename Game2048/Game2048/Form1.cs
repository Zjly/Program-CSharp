using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game2048 {
	public partial class Form1 : Form {
		GameService game;
        public Form1() {
			InitializeComponent();
	        game = new GameService(4, this);
            game.AddNewNumber();
			game.DrawGame();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
	        Keys k = e.KeyCode;
	        bool move = false;
	        switch (k) {
		        case Keys.Down:
			        if (game.MoveDown()) {
				        move = true;
			        }

			        break;
		        case Keys.Left:
			        if (game.MoveLeft()) {
				        move = true;
			        }

			        break;
		        case Keys.Right:
			        if (game.MoveRight()) {
				        move = true;
			        }

			        break;
		        case Keys.Up:
			        if (game.MoveUp()) {
				        move = true;
			        }

			        break;
	        }

	        if (move) {
		        game.AddNewNumber();
		        game.DrawGame();
	        }
        }
    }
}