using System.Text;

namespace FightTheLandlord {
	/// <summary>
	/// 卡牌基本操作
	/// </summary>
	public static class CardService {
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