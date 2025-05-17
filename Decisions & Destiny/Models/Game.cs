namespace Decisions___Destiny.Models
{
	public class Game
	{
		public Dictionary<string, Scene> Scenes { get; set; }
		public HashSet<string> Flags { get; set; }
		public string CurrentSceneID { get; set; }

        public Game()
		{

			Console.Clear();
		}


	}
}
