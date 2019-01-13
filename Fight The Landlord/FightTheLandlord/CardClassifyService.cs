namespace FightTheLandlord {
	/// <summary>
	/// 牌分类处理类
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
					return "";
			}
		}

		/// <summary>
		/// 出牌数为单张卡处理
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string OneCardProcessing(string cards) {
			return "single";
		}

		/// <summary>
		/// 出牌数为两张卡处理
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string TwoCardsProcessing(string cards) {
			return "";
		}

		/// <summary>
		/// 出牌数为三张卡处理
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string ThreeCardsProcessing(string cards) {
			return "";
		}

		/// <summary>
		/// 出牌数为四张卡处理
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>出牌类型</returns>
		private static string FourCardsProcessing(string cards) {
			return "";
		}

		/// <summary>
		/// 出牌合法性判断 是否有无效牌
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>是否合法</returns>
		internal static bool IsValid(string cards) {
			// 若有非法卡牌出现则中断并返回错误
			for(int i = 0; i < cards.Length; i++) {
				switch(cards[i]) {
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					case '0':
					case 'J':
					case 'Q':
					case 'K':
					case 'A':
					case '2':
					case 'S':
					case 'B':
						break;
					default:
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 对卡牌顺序进行排序 以便后续操作
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>排序完毕的卡牌</returns>
		internal static string Sort(string cards) {
			// 对卡牌进行按ascii码排序
			for(int i = 0; i < cards.Length - 1; i++) {
				for(int j = i + 1; j < cards.Length; j++) {
					if(cards[i] > cards[j]) {
						char p = cards[i];
						cards = cards.Remove(i, 1).Insert(i, cards[j].ToString());
						cards = cards.Remove(j, 1).Insert(j, p.ToString());
					}
				}
			}

			return cards;
		}
	}
}