using System;
using System.Text;

namespace FightTheLandlord {
	/// <summary>
	/// 发牌类
	/// </summary>
	public static class CardGetService {
		private const string AllCards = "33334444555566667777888899990000JJJJQQQQKKKKAAAA2222SB";

		/// <summary>
		/// 得到三副发好的牌
		/// </summary>
		/// <param name="cards1">第一个玩家的牌</param>
		/// <param name="cards2">第二个玩家的牌</param>
		/// <param name="cards3">第三个玩家的牌</param>
		/// <param name="landlordCards">三张地主牌</param>
		internal static void DealCards(ref string cards1, ref string cards2, ref string cards3, ref string landlordCards) {
			if(cards1 == null || cards2 == null || cards3 == null || landlordCards == null) {
				throw new ArgumentNullException();
			}

			StringBuilder cardsLibrary = new StringBuilder(AllCards); // 卡牌库
			Random random = new Random();
			int remain = AllCards.Length; // 剩余的牌

			// 三名玩家的牌
			StringBuilder c1 = new StringBuilder();
			StringBuilder c2 = new StringBuilder();
			StringBuilder c3 = new StringBuilder();

			// 每个玩家获取17张牌 得到一个随机数 抽取该牌 将该牌从卡牌库里移除
			for(int i = 0; i < 17; i++) {
				int num1 = random.Next(remain--);
				c1.Append(cardsLibrary[num1]);
				cardsLibrary.Remove(num1, 1);
				
				int num2 = random.Next(remain--);
				c2.Append(cardsLibrary[num2]);
				cardsLibrary.Remove(num2, 1);
				
				int num3 = random.Next(remain--);
				c3.Append(cardsLibrary[num3]);
				cardsLibrary.Remove(num3, 1);
			}

			// 对获得卡牌排序后赋值
			cards1 = CardService.Sort(c1.ToString());
			cards2 = CardService.Sort(c2.ToString());
			cards3 = CardService.Sort(c3.ToString());
			landlordCards = CardService.Sort(cardsLibrary.ToString());
		}
	}
}