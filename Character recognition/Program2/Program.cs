using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Program2 {
	internal class Program {
        public static void Main(params string[] args) {
			GetTemplate();
		}

		private static void GetTemplate() {
			string path = @"../../image/test image/";
			var files = Directory.GetFiles(path, "*.jpg"); // 找到其中的图片文件
			foreach(var file in files) {
				// 将code更改为文件名(去掉前后的路径信息)
				string code = file.Replace(@"../../image/test image/", "");
				code = code.Replace(".jpg", "");
				Recognition(file, code);
            }
        }

        private static void Recognition(string path, string code) {
			Bitmap image = new Bitmap(path); // 获取图片
	        byte[,] matrix = new byte[image.Width, image.Height]; // 保存图片的矩阵
			int[] result = new int[4]; // 四个字母所填充的数字
			ToGray(image); // 图片灰度化
			Binaryzation(image); // 图片二值化
			SaveToMatrix(image, matrix); // 将图片存入矩阵
			NoiseRemovalProcessing(image, matrix); // 去除图片噪点
			Fill(matrix, result); // 将验证码中四个字母填充上不同的数字
			Divide(matrix, result, code); // 分割四个字符
		}

		// 图片灰度化
		private static void ToGray(Bitmap image) {
			for(int i = 0; i < image.Width; i++) {
				for(int j = 0; j < image.Height; j++) {
					Color pixelColor = image.GetPixel(i, j);
					// 通过一定方式将RGB值转化为灰度值
					int grey = (int) (0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);
					Color newColor = Color.FromArgb(grey, grey, grey);
					image.SetPixel(i, j, newColor);
				}
			}
		}

		// 图片二值化
		private static void Binaryzation(Bitmap image) {
			int[] histogram = new int[256];
			int minGrayValue = 255, maxGrayValue = 0;
			//求取直方图
			for(int i = 0; i < image.Width; i++) {
				for(int j = 0; j < image.Height; j++) {
					Color pixelColor = image.GetPixel(i, j);
					histogram[pixelColor.R]++;
					if(pixelColor.R > maxGrayValue) maxGrayValue = pixelColor.R;
					if(pixelColor.R < minGrayValue) minGrayValue = pixelColor.R;
				}
			}

			//迭代计算阈值
			int threshold = -1;
			int newThreshold = (minGrayValue + maxGrayValue) / 2;
			for(int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++) {
				threshold = newThreshold;
				int lP1 = 0;
				int lP2 = 0;
				int lS1 = 0;
				int lS2 = 0;
				//求两个区域的灰度的平均值
				for(int i = minGrayValue; i < threshold; i++) {
					lP1 += histogram[i] * i;
					lS1 += histogram[i];
				}

				int mean1GrayValue = (lP1 / lS1);
				for(int i = threshold + 1; i < maxGrayValue; i++) {
					lP2 += histogram[i] * i;
					lS2 += histogram[i];
				}

				int mean2GrayValue = (lP2 / lS2);
				newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
			}

			//计算二值化
			for(int i = 0; i < image.Width; i++) {
				for(int j = 0; j < image.Height; j++) {
					Color pixelColor = image.GetPixel(i, j);
					if(pixelColor.R > threshold) {
						image.SetPixel(i, j, Color.FromArgb(255, 255, 255));
					} else {
						image.SetPixel(i, j, Color.FromArgb(0, 0, 0));
					}
				}
			}
		}

		// 将图片存入矩阵
		private static void SaveToMatrix(Bitmap image, byte[,] matrix) {
			for(int i = 0; i < image.Width; i++) {
				for(int j = 0; j < image.Height; j++) {
					matrix[i, j] = image.GetPixel(i, j).R;
				}
			}
		}

		// 将矩阵导入图片
		private static void SaveToImage(Bitmap image, byte[,] matrix) {
			Color color = new Color();
			for(int i = 0; i < image.Width; i++) {
				for(int j = 0; j < image.Height; j++) {
					color = Color.FromArgb(matrix[i, j], matrix[i, j], matrix[i, j]);
					image.SetPixel(i, j, color);
				}
			}
		}

		// 矩阵保存到txt
		private static void SaveToFile(string path, byte[,] matrix) {
			try {
				StreamWriter writer = new StreamWriter(path);
				for(int j = 0; j < matrix.GetLength(1); j++) {
					for(int i = 0; i < matrix.GetLength(0); i++) {
						if(matrix[i, j] == 0) {
							writer.Write("■");
						} else {
							writer.Write("□");
						}
					}

					writer.WriteLine();
				}

				writer.Close();
			} catch(Exception e) {
				Console.WriteLine(e);
			}
		}

		// 控制台上显示矩阵
		private static void Display(byte[,] matrix) {
			for(int j = 0; j < matrix.GetLength(1); j++) {
				for(int i = 0; i < matrix.GetLength(0); i++) {
					if(matrix[i, j] == 0) {
						Console.Write("■");
					} else {
						Console.Write("□");
					}
				}

				Console.WriteLine();
			}
		}

		// 去除图片噪点，threshold为阈值[1, 8]，越小去除效果越强
		private static void RemoveNoise(Bitmap image, byte[,] matrix, int threshold) {
			int nValue, nCount;
			int nWidth = matrix.GetLength(0);
			int nHeight = matrix.GetLength(1);

			// 横向边框设定
			for(int i = 0; i < nWidth; i++) {
				matrix[i, 0] = 255;
				matrix[i, nHeight - 1] = 255;
			}

			// 纵向边框设定
			for(int i = 0; i < nHeight; i++) {
				matrix[0, i] = 255;
				matrix[nWidth - 1, i] = 255;
			}

			for(int j = 1; j < nHeight - 1; j++) {
				for(int i = 1; i < nWidth - 1; i++) {
					nValue = image.GetPixel(i, j).R;
					// 黑点周围噪点去除
					if(nValue == 0) {
						nCount = 0;
						for(int m = i - 1; m <= i + 1; m++)
							for(int n = j - 1; n <= j + 1; n++) {
								if(image.GetPixel(m, n).R == 255) {
									nCount++;
								}
							}

						if(nCount >= threshold) {
							matrix[i, j] = 255;
						}
					}
					// 白点周围噪点去除
					else {
						nCount = 0;
						for(int m = i - 1; m <= i + 1; m++)
							for(int n = j - 1; n <= j + 1; n++) {
								if(image.GetPixel(m, n).R == 0) {
									nCount++;
								}
							}

						if(nCount >= threshold) {
							matrix[i, j] = 0;
						}
					}
				}
			}
		}

		// 对噪点进行多次处理
		private static void NoiseRemovalProcessing(Bitmap image, byte[,] matrix) {
			RemoveNoise(image, matrix, 7);
			SaveToImage(image, matrix);
			RemoveNoise(image, matrix, 7);
		}

		private static int num = 0; // 黑点的个数，通过与一个阈值的比较，确定是噪点还是字母

		// 分割验证码
		private static void Fill(byte[,] matrix, int[] result) {
			int nWidth = matrix.GetLength(0);
			int nHeight = matrix.GetLength(1);
			byte color = 0; // 待填充颜色
			int resultNum = 0;
			// 对向量黑点进行填充数字
			for(int i = 0; i < nWidth; i++) {
				for(int j = 0; j < nHeight; j++) {
					if(matrix[i, j] == 0) {
						num = 0;
						color++;
						FillCharacter(matrix, i, j, color);
						if(num > 20) {
							result[resultNum] = color;
							resultNum++;
						}
					}
				}
			}
		}

		// 漫水填充算法
		private static void FillCharacter(byte[,] matrix, int x, int y, byte color) {
			if(matrix[x, y] == 0) {
				matrix[x, y] = color;
				num++;
				FillCharacter(matrix, x - 1, y, color);
				FillCharacter(matrix, x + 1, y, color);
				FillCharacter(matrix, x, y - 1, color);
				FillCharacter(matrix, x, y + 1, color);
			}
		}

		// 分割字符
		private static void Divide(byte[,] matrix, int[] result, string code) {
			for(int k = 0; k < result.Length; k++) {
				int color = result[k];
				DivideToImage(matrix, color, k + 1, code[k]);
			}
		}

		// 分割字符并放入图片中
		private static void DivideToImage(byte[,] matrix, int color, int count, char letter) {
			int nWidth = matrix.GetLength(0);
			int nHeight = matrix.GetLength(1);
			int left = nWidth, right = 0, up = nHeight, down = 0;
			// 找到边界
			for(int i = 0; i < nWidth; i++) {
				for(int j = 0; j < nHeight; j++) {
					if(matrix[i, j] == color) {
						if(i < left) {
							left = i;
						}

						if(i > right) {
							right = i;
						}

						if(j < up) {
							up = j;
						}

						if(j > down) {
							down = j;
						}
					}
				}
			}

			int width = right - left + 1;
			int height = down - up + 1;

			Bitmap image = new Bitmap(width, height); // 图片
			byte[,] newMatrix = new byte[width, height];
			for(int i = 0; i < width; i++) {
				for(int j = 0; j < height; j++) {
					image.SetPixel(i, j, Color.White);
					newMatrix[i, j] = 255;
				}
			}

			for(int i = 0; i < width; i++) {
				for(int j = 0; j < height; j++) {
					if(matrix[i + left, j + up] == color) {
						image.SetPixel(i, j, Color.Black);
						newMatrix[i, j] = 0;
					}
				}
			}

			RotateImage(image); // 将图片旋转为正
			Image resultImage = image.GetThumbnailImage(16, 16, () => { return false; }, IntPtr.Zero); // 将图像统一调整为16*16

//            string path = @"../../image\template\" + letter + ".jpg";
//			resultImage.Save(path);

			string path = @"../../image/template/" + letter + ".txt";
			string s = MatrixToString(matrix); // 该字母的特征字符串
			File.WriteAllText(path, s, Encoding.UTF8);
		}

		private static void RotateImage(Image image) {
			//...
		}

        // 可以将字符图片等分为16个子区域，每个区域是4*4的
        // 统计每个区域内部黑色像素（字符像素）的个数，这样得到16个数值
        private static string MatrixToString(byte[,] matrix) {
			int nWidth = matrix.GetLength(0);
			int nHeight = matrix.GetLength(1);
	        
	        int[] resultInt = new int[16];

			for (int width = 0; width < nWidth; width++) {
				for (int height = 0; height < nHeight; height++) {
					int index = width / 4 + height / 4 * 4;
					if(matrix[width, height] == 0) {
						resultInt[index]++;
					}
				}
			}

	        string result = "";
	        for(int i = 0; i < resultInt.Length; i++) {
		        result += resultInt[i] + " ";
	        }

	        return result;       
        }
	}
}
