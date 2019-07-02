using System;
using static FightTheLandlord.CardGetService;
using static FightTheLandlord.CardService;

namespace FightTheLandlord {
	/// <summary>
	/// 游戏主体
	/// </summary>
	public static class MainGame {
		private static string lastCards = ""; // 上家出的牌
		private static string landlordCards = ""; // 地主牌
		private static string[] player = {"", "", ""};
		private static int landlord = 1; // 地主玩家序号
		private static int currentPlayer = -1; // 当前玩家

		/// <summary>
		/// Main函数
		/// </summary>
		public static void Main() {
			SetUp();
			Run();
		}

		/// <summary>
		/// 游戏开始前准备
		/// </summary>
		private static void SetUp() {
			DealCards(ref player[0], ref player[1], ref player[2], ref landlordCards);
			SelectLandlord();
		}

		/// <summary>
		/// 抢地主
		/// </summary>
		private static void SelectLandlord() {
			Random random = new Random();
			currentPlayer = random.Next(3);

			// 抢地主过程...
			for(int i = 0; i < 3; i++) {
				if(currentPlayer == 3) {
					currentPlayer -= 3;
				}
				
				ShowCurrentPlayer();
			}

			player[landlord] += landlordCards;
			player[landlord] = Sort(player[landlord]);
		}

		/// <summary>
		/// 游戏主体
		/// </summary>
		private static void Run() {
			
		}

		/// <summary>
		/// 显示当前用户
		/// </summary>
		private static void ShowCurrentPlayer() {
			Console.Write($"当前玩家为Player{currentPlayer}: ");
		}
		
		
	}
}