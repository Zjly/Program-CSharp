using static FightTheLandlord.CardClassifyService;

namespace FightTheLandlord {
	/// <summary>
	/// 卡牌类
	/// </summary>
	public class Cards {
		/// <summary>
		/// 具体所出牌
		/// </summary>
		internal string cards;
		
		/// <summary>
		/// 牌属类型 不合法则为空
		/// </summary>
		internal string type;

		/// <summary>
		/// 构造函数 对卡牌进行赋值并分类
		/// </summary>
		/// <param name="cards">所出的牌</param>
		internal Cards(string cards) {
			// 全部转大写
			cards = cards.ToUpper();
			this.cards = Sort(cards);
			
			// 对合法性进行判断后进行赋值分类
			if(IsValid(cards)) {
				type = ClassifyCards(this.cards);
			} else {
				type = "invalid";
			}
		}
	}
}