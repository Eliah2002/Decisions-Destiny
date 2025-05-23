using System.Text.Json;

namespace Decisions___Destiny.Models
{
	public class Game
	{
		public Dictionary<string, Scene> Scenes { get; set; } = new();
		public HashSet<string> Flags { get; set; } = new();
		public string CurrentSceneID { get; set; } = "";

		public Game(string jsonPath)
		{
			Console.Clear();
			string jsonContent = File.ReadAllText(jsonPath);

			List<Scene>? sceneList = JsonSerializer.Deserialize<List<Scene>>(jsonContent);
			Scenes = sceneList.ToDictionary(s => s.ID, s => s);
			CurrentSceneID = sceneList.First().ID;

			Run();
		}
		private void Run()
		{
			while (Scenes.ContainsKey(CurrentSceneID))
			{
				Scene currentScene = Scenes[CurrentSceneID];
				currentScene.StartScene(this);
			}
		}
	}
}
