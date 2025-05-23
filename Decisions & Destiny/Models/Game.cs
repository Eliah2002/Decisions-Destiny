using System.Text.Json;

namespace Decisions___Destiny.Models
{
	public class Game
	{
		public Dictionary<string, Scene> Scenes { get; set; }
		public HashSet<string> Flags { get; set; }
		public string CurrentSceneID { get; set; }

		public Game(string jsonPath)
		{
			Console.Clear();

			string jsonContent = File.ReadAllText(jsonPath);
			List<Scene> Scenes = JsonSerializer.Deserialize<List<Scene>>(jsonContent);

			Scenes.FirstOrDefault().StartScene();
			Console.ReadLine();
		}
	}
}
