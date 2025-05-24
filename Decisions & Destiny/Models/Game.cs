using System.Text.Json;
using System.Text.Json.Serialization;

namespace Decisions___Destiny.Models
{
	public class Game
	{
		public static Game Singleton { get; set; } = new Game();

		[JsonIgnore]
		public Dictionary<string, Scene> Scenes { get; set; } = new();
		public HashSet<string> Flags { get; set; } = new();
		public string CurrentSceneID { get; set; } = "";

		public void Start(string jsonPath)
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
			bool close = false;
			while (Scenes.ContainsKey(CurrentSceneID) && !close)
			{
				Scene currentScene = Scenes[CurrentSceneID];
				close = currentScene.StartScene(this);
			}
		}
	}
}
