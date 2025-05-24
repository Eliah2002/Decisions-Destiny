using Decisions___Destiny.Enums;
using Decisions___Destiny.Helpers;
using System.Text.Json;

namespace Decisions___Destiny.Models
{
	/// <summary>
	/// Repräsentiert den aktuellen Zustand des Spiels (Szenen, Flags, Fortschritt).
	/// </summary>
	public class Game
	{
		public static Game Singleton { get; set; } = new Game();

		/// <summary>
		/// Alle Szenen des Spiels, identifiziert durch eine eindeutige ID.
		/// </summary>
		public Dictionary<string, Scene> Scenes { get; set; } = new();

		/// <summary>
		/// Gesammelte Flags (z. B. Entscheidungen oder Zustände), die den Spielfortschritt beeinflussen.
		/// </summary>
		public HashSet<string> Flags { get; set; } = new();

		public string CurrentSceneID { get; set; } = "";

		/// <summary>
		/// Startet das Spiel, lädt Szenen aus einer JSON-Datei und beginnt die erste Szene.
		/// </summary>
		public void Start(string chapterPath, string? currentSceneID = null)
		{
			Console.Clear();

			// JSON-Datei lesen und deserialisieren
			//ELIAH
			var chapters = Directory.GetFiles(chapterPath, "*.json");

			List<Scene>? sceneList = new List<Scene>();
			foreach (var chapter in chapters)
			{
				string jsonContent = File.ReadAllText(chapter);
				var ttt = JsonSerializer.Deserialize<List<Scene>>(jsonContent);
				sceneList.AddRange(JsonSerializer.Deserialize<List<Scene>>(jsonContent));
			}

			// In ein Dictionary umwandeln (für schnellen Zugriff per ID)
			Scenes = sceneList.ToDictionary(s => s.ID, s => s);

			// Startszene festlegen (erste in der Liste)
			if (string.IsNullOrEmpty(currentSceneID))
			{
				var firstScene = sceneList.FirstOrDefault(scene => scene.ID.Equals("start", StringComparison.OrdinalIgnoreCase))
					 ?? sceneList.FirstOrDefault();

				CurrentSceneID = firstScene.ID;
			}
			else
			{
				CurrentSceneID = currentSceneID;
			}

			// Spiel ausführen
			Run();
		}

		/// <summary>
		/// Führt das Spiel aus, indem es die Szenen entsprechend der Spielerentscheidungen durchläuft.
		/// </summary>
		private void Run()
		{
			IntroScreen.PlayIntro(DecisionsAndDestiny.Singleton.SelectedGameName);

			while (Scenes.ContainsKey(CurrentSceneID))
			{
				Scene currentScene = Scenes[CurrentSceneID];

				// Bricht ab, wenn das Szenenergebnis etwas anderes als "Continue" zurückliefert
				if (currentScene.StartScene(this) != SceneResult.Continue)
					break;
			}
		}
	}
}
