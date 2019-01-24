namespace FightTheLandlord {
	/// <summary>
	/// 牌分类处理
	/// </summary>
	public static class CardClassifyService {
		/// <summary>
		/// 对出牌类型进行分类
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		internal static string ClassifyCards(string cards) {
			// 出牌长度 方便下一步分类
			int count = cards.Length;

			// 根据出牌数目来进行分类判断
			switch(count) {
				case 1:
					return OneCardProcessing(cards);
				case 2:
					return TwoCardsProcessing(cards);
				case 3:
					return ThreeCardsProcessing(cards);
				case 4:
					return FourCardsProcessing(cards);
				default:
					return FiveOrMoreCardsProcessing(cards);
			}
		}

		/// <summary>
		/// 出牌数为单张卡处理 可能为单牌(例: 3)
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string OneCardProcessing(string cards) {
			return "single";
		}

		/// <summary>
		/// 出牌数为两张卡处理 可能为对子(例: 33)或者王炸(例: BS)
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string TwoCardsProcessing(string cards) {
			// 对子
			if(cards[0] == cards[1]) {
				return "pair";
			}

			// 王炸
			if(cards[0] == 'S' && cards[1] == 'B') {
				return "rocket";
			}

			// 其余情况则不合法
			return "invalid";
		}

		/// <summary>
		/// 出牌数为三张卡处理 可能为三牌(例: 333)
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string ThreeCardsProcessing(string cards) {
			// 判断是否为三牌
			if(cards[0] == cards[1] && cards[1] == cards[2]) {
				return "triple";
			}

			// 否则为不合法
			return "invalid";
		}

		/// <summary>
		/// 出牌数为四张卡处理 可能为炸弹(例: 3333)或者三带一(例: 3334)
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string FourCardsProcessing(string cards) {
			// 首先判断是否为炸弹
			if(cards[0] == cards[1] && cards[1] == cards[2] && cards[2] == cards[3]) {
				return "bomb";
			}

			// 其次判断是否为三带一(两种情况 3334 3444)
			if(cards[0] == cards[1] && cards[1] == cards[2] || cards[1] == cards[2] && cards[2] == cards[3]) {
				return "triple with single";
			}

			// 其余情况则不合法
			return "invalid";
		}

		/// <summary>
		/// 五张及以上卡判断类型
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string FiveOrMoreCardsProcessing(string cards) {
			// 三带二判断(33344 33444)
			if(cards.Length == 5 &&
			   cards[0] == cards[1] && cards[1] == cards[2] && cards[3] == cards[4] ||
			   cards[0] == cards[1] && cards[2] == cards[3] && cards[3] == cards[4]) {
				return "triple with double";
			}

			// 四带二判断(3333xx x3333x xx3333)
			if(cards.Length == 6 &&
			   cards[0] == cards[1] && cards[1] == cards[2] && cards[2] == cards[3] ||
			   cards[1] == cards[2] && cards[2] == cards[3] && cards[3] == cards[4] ||
			   cards[2] == cards[3] && cards[3] == cards[4] && cards[4] == cards[5]) {
				return "fourfold with double";
			}

			// 飞机判断(33344456 333444555678)

			// 顺子判断
			return StraightProcessing(cards);
		}

		/// <summary>
		/// 顺子判断
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>是否为顺子 若是顺子则给出类型</returns>
		private static string StraightProcessing(string cards) {
			string singleStraightMode = "34567890JQKA";
			string doubleStraightMode = "3344556677889900JJQQKKAA";
			string tripleStraightMode = "333444555666777888999000JJJQQQKKKAAA";

			// 单顺子
			if(singleStraightMode.Contains(cards)) {
				return "single straight";
			}

			// 双顺子
			if(doubleStraightMode.Contains(cards)) {
				return "double straight";
			}

			// 三顺子
			if(tripleStraightMode.Contains(cards)) {
				return "triple straight";
			}

			return "invalid";
		}
	}
}