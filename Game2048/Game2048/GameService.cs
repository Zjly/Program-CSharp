using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game2048 {
	public class GameService {
		private int[,] game;
		private int size;
		private Form1 form1;

		public GameService(int size, Form1 form1) {
			game = new int[size, size];
			this.size = size;
			this.form1 = form1;
		}

		// 绘制游戏棋盘
		public void DrawGame() {
			int[] index = new int[16];
			for(int i = 0; i < size; i++) {
				for(int j = 0; j < size; j++) {
					int gameNum = game[i, j];
					int num = 0;
					if(gameNum != 0) {
						while (gameNum != 1) {
							gameNum /= 2;
							num++;
						}
                    }

					index[i * 4 + j] = num;
				}
			}

			form1.pictureBox2.Image = form1.imageList1.Images[index[0]];
			form1.pictureBox3.Image = form1.imageList1.Images[index[1]];
			form1.pictureBox4.Image = form1.imageList1.Images[index[2]];
			form1.pictureBox5.Image = form1.imageList1.Images[index[3]];
			form1.pictureBox6.Image = form1.imageList1.Images[index[4]];
			form1.pictureBox7.Image = form1.imageList1.Images[index[5]];
			form1.pictureBox8.Image = form1.imageList1.Images[index[6]];
			form1.pictureBox9.Image = form1.imageList1.Images[index[7]];
			form1.pictureBox10.Image = form1.imageList1.Images[index[8]];
			form1.pictureBox11.Image = form1.imageList1.Images[index[9]];
			form1.pictureBox12.Image = form1.imageList1.Images[index[10]];
			form1.pictureBox13.Image = form1.imageList1.Images[index[11]];
			form1.pictureBox14.Image = form1.imageList1.Images[index[12]];
			form1.pictureBox15.Image = form1.imageList1.Images[index[13]];
			form1.pictureBox16.Image = form1.imageList1.Images[index[14]];
			form1.pictureBox17.Image = form1.imageList1.Images[index[15]];
        }

		// 随机找到一个格子 判断是否为空 不空则循环 空则随机加入2或4
		public void AddNewNumber() {
			int addRow, addColumn;
			Random random = new Random();
			// 是否添加成功
			bool addSuccess = false;
			do {
				addRow = random.Next(0, 4);
				addColumn = random.Next(0, 4);
				if(game[addRow, addColumn] == 0) {
					// 随机在格子中加入2或4
					game[addRow, addColumn] = GetRandom2Or4();
					addSuccess = true;
				}
			} while(!addSuccess);
		}

		private int GetRandom2Or4() {
			double proportionOfTwo = 0.9;
			Random random = new Random();
			if(random.NextDouble() < proportionOfTwo) {
				return 2;
			}

			return 4;
		}

		public bool MoveLeft() {
			bool move = false;
			// 进行搜索
			for(int i = 0; i < size; i++) {
				// 从左到右两两比较
				for(int j = 0; j < size - 1; j++) {
					for(int k = j + 1; k < size; k++) {
						// 相等则合并
						if(game[i, j] != 0 && game[i, k] != 0) {
							if(game[i, j] == game[i, k]) {
								game[i, j] *= 2;
								game[i, k] = 0;
								move = true;
							}

							break;
						}
					}
				}

				// 移动部分
				for(int j = 0; j < size - 1; j++) {
					for(int k = j + 1; k < size; k++) {
						if(game[i, j] == 0 && game[i, k] != 0) {
							game[i, j] = game[i, k];
							game[i, k] = 0;
							move = true;
						}
					}
				}
			}

			return move;
		}

		public bool MoveRight() {
			bool move = false;
			// 进行行搜索
			for(int i = 0; i < size; i++) {
				// 两两比较
				for(int j = size - 1; j > 0; j--) {
					for(int k = j - 1; k >= 0; k--) {
						// 相等则合并
						if(game[i, j] != 0 && game[i, k] != 0) {
							if(game[i, j] == game[i, k]) {
								game[i, j] *= 2;
								game[i, k] = 0;
								move = true;
							}

							break;
						}
					}
				}

				// 移动部分
				for(int j = size - 1; j > 0; j--) {
					for(int k = j - 1; k >= 0; k--) {
						if(game[i, j] == 0 && game[i, k] != 0) {
							game[i, j] = game[i, k];
							game[i, k] = 0;
							move = true;
						}
					}
				}
			}

			return move;
		}

		public bool MoveUp() {
			bool move = false;
			// 进行搜索
			for(int i = 0; i < size; i++) {
				for(int j = 0; j < size - 1; j++) {
					for(int k = j + 1; k < size; k++) {
						// 相等则合并
						if(game[j, i] != 0 && game[k, i] != 0) {
							if(game[j, i] == game[k, i]) {
								game[j, i] *= 2;
								game[k, i] = 0;
								move = true;
							}

							break;
						}
					}
				}

				// 移动部分
				for(int j = 0; j < size - 1; j++) {
					for(int k = j + 1; k < size; k++) {
						if(game[j, i] == 0 && game[k, i] != 0) {
							game[j, i] = game[k, i];
							game[k, i] = 0;
							move = true;
						}
					}
				}
			}

			return move;
		}

		public bool MoveDown() {
			bool move = false;
			// 进行行搜索
			for(int i = 0; i < size; i++) {
				// 两两比较
				for(int j = size - 1; j > 0; j--) {
					for(int k = j - 1; k >= 0; k--) {
						// 相等则合并
						if(game[j, i] != 0 && game[k, i] != 0) {
							if(game[j, i] == game[k, i]) {
								game[j, i] *= 2;
								game[k, i] = 0;
								move = true;
							}

							break;
						}
					}
				}

				// 移动部分
				for(int j = size - 1; j > 0; j--) {
					for(int k = j - 1; k >= 0; k--) {
						if(game[j, i] == 0 && game[k, i] != 0) {
							game[j, i] = game[k, i];
							game[k, i] = 0;
							move = true;
						}
					}
				}
			}

			return move;
		}
    }
}