using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Baidu.Aip.Ocr;

namespace Program1 {
	class Program {
		static void Main(string[] args) {
			var client = GetClient();

			string result = GetResult(client, @"C:\Users\94247\Desktop\车牌.jpg");

			string[] strings = ExtractWord(result);

			SaveToFile(@"C:\Users\94247\Desktop\test.txt", strings);
		}

		// 获取百度AI识别客户端
		static Ocr GetClient() {
			var APP_ID = "14991582";
			var API_KEY = "lgonG76jRV7MsGGrV74VcyIv";
			var SECRET_KEY = "Qy8P97wnmUGLw2LSuBkRxT0KwuKHOpGi";

			var client = new Ocr(API_KEY, SECRET_KEY);
			client.Timeout = 60000;

			return client;
		}

		// 获取识别结果
		static string GetResult(Ocr client, string path) {
			var image = File.ReadAllBytes(path); // 将图片转化为数组
			try {
				var result = client.GeneralBasic(image); // 获取结果
				return result.ToString();
			} catch(Exception e) {
				Console.WriteLine(e);
			}

			return null;
		}

		// 识别结果提取
		static string[] ExtractWord(string s) {
			// 获取图中文字数目
			string countS = "\"words_result_num\": (.*?),";
			MatchCollection numMatches = new Regex(countS).Matches(s);
			int position = numMatches[0].Value.IndexOf(':') + 1;
			countS = numMatches[0].Value.Substring(position).Trim('"', '\\', '#', '>', ' ', '\'', ',');
			int count = Convert.ToInt32(countS);

			// 将结果存入数组
			string[] result = new string[count];
			int num = 0;
			string strRef = "\"words\": \"(.*?)\"";
			MatchCollection matches = new Regex(strRef).Matches(s); // 获取匹配信息
			foreach(Match match in matches) {
				int index = match.Value.IndexOf(':') + 1;
				string word = match.Value.Substring(index).Trim('"', '\\', '#', '>', ' ', '\''); // 得到识别出的文字
				result[num++] = word;
			}

			return result;
		}

		// 保存结果到文件
		static void SaveToFile(string path, string[] strings) {
			try {
				StreamWriter writer = new StreamWriter(path);
				foreach(string s in strings) {
					writer.WriteLine(s);
				}
				writer.Close();
			} catch(Exception e) {
				Console.WriteLine(e);
			}
		}
	}
}