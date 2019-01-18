using System.Text;

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

			// 飞机判断(33344456)
			

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
			// 将出牌中的每个字母导入到数组中
			int[] array = new int[16];
			for(int i = 0; i < cards.Length; i++) {
				array[GetCardInt(cards[i])]++;
			}

			// 将数组中的按照数字存成字符串并返回
			return GetSortedCards(array);
		}

		/// <summary>
		/// 根据字符得到对应卡牌的数组序号
		/// </summary>
		/// <param name="card">单张卡牌</param>
		/// <returns>对应的数组序号</returns>
		private static int GetCardInt(char card) {
			switch(card) {
				case '3':
					return 3;
				case '4':
					return 4;
				case '5':
					return 5;
				case '6':
					return 6;
				case '7':
					return 7;
				case '8':
					return 8;
				case '9':
					return 9;
				case '0':
					return 10;
				case 'J':
					return 11;
				case 'Q':
					return 12;
				case 'K':
					return 13;
				case 'A':
					return 1;
				case '2':
					return 2;
				case 'S':
					return 14;
				case 'B':
					return 15;
			}

			return -1;
		}

		/// <summary>
		/// 通过数组排列字符串
		/// </summary>
		/// <param name="array">牌所储存的数组</param>
		/// <returns>按照扑克大小存储的字符串</returns>
		private static string GetSortedCards(int[] array) {
			StringBuilder cards = new StringBuilder("");

			// 向字符串中添加3-9
			for(int i = 3; i < 10; i++) {
				for(int j = 0; j < array[i]; j++) {
					cards.Append(i);
				}
			}

			// 添加10
			for(int i = 0; i < array[10]; i++) {
				cards.Append(0);
			}

			// 添加J
			for(int i = 0; i < array[11]; i++) {
				cards.Append('J');
			}

			// 添加Q
			for(int i = 0; i < array[12]; i++) {
				cards.Append('Q');
			}

			// 添加K
			for(int i = 0; i < array[13]; i++) {
				cards.Append('K');
			}

			// 添加A
			for(int i = 0; i < array[1]; i++) {
				cards.Append('A');
			}

			// 添加2
			for(int i = 0; i < array[2]; i++) {
				cards.Append('2');
			}

			// 添加S
			for(int i = 0; i < array[14]; i++) {
				cards.Append('S');
			}

			// 添加B
			for(int i = 0; i < array[15]; i++) {
				cards.Append('B');
			}

			return cards.ToString();
		}
	}
}