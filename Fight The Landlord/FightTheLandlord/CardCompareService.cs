namespace FightTheLandlord {
	/// <summary>
	/// 牌大小比较类 比较大小后确定是否能大过上家的牌
	/// </summary>
	public static class CardCompareService {
		/// <summary>
		/// 单张牌
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>是否比上家的牌大</returns>
		internal static void SingleCard(string cards) { }

		/// <summary>
		/// 对子牌
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>是否比上家的牌大</returns>
		internal static void PairCards(string cards) { }
		
		/// <summary>
		/// 王炸
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>是否比上家的牌大</returns>
		internal static void Rocket(string cards) { }
		
		/// <summary>
		/// 三张牌
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>是否比上家的牌大</returns>
		internal static void TripleCards(string cards) { }
		
		/// <summary>
		/// 三带一
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>是否比上家的牌大</returns>
		internal static void TripleWithSingle(string cards) { }
		
		/// <summary>
		/// 炸弹
		/// </summary>
		/// <param name="cards">所出的牌</param>
		/// <returns>是否比上家的牌大</returns>
		internal static void Bomb(string cards) { }
	}
}