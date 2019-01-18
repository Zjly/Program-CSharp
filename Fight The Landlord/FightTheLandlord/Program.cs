using System;

namespace FightTheLandlord {
	internal class Program {
		public static void Main(string[] args) {
			MainGame game = new MainGame();
			
			Cards cards = new Cards("");
			Console.WriteLine(cards.type);
		}
	}
}