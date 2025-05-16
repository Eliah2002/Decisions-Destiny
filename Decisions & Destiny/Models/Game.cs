namespace Decisions___Destiny.Models
{
	internal class Game
	{
		public Game(string gameName)
		{
			GameName = gameName;
			Console.Clear();
		}
		string GameName { get; set; }
	}
}
