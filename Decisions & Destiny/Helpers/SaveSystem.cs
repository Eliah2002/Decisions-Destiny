using Decisions___Destiny.Models;
using System.Text.Json;

namespace Decisions___Destiny.Helpers
{
	/// <summary>
	/// Diese statische Klasse verwaltet das Speichern und Laden von Spielständen.
	/// </summary>
	public static class SaveSystem
	{
		/// <summary>
		/// Speichert den aktuellen Spielzustand in eine JSON-Datei.
		/// </summary>
		/// <param name="game">Das aktuelle Spielobjekt.</param>
		/// <param name="saveName">Der gewünschte Name des Speicherstandes (ohne Dateiendung).</param>
		public static void SaveGame(Game game, string saveName)
		{
			// Spielzustand in ein DTO (Data Transfer Object) verpacken.
			var saveGame = new SaveGame
			{
				CurrentSceneID = game.CurrentSceneID,
				Flags = game.Flags.ToList()
			};

			// Serialisieren in JSON mit Formatierung
			string saveJson = JsonSerializer.Serialize(saveGame, new JsonSerializerOptions { WriteIndented = true });

			// Pfad zum Speicherordner + Dateiname
			string path = Path.Combine(
				DecisionsAndDestiny.Singleton.SelectedGameScoresFolderPath,
				$"{saveName}.json"
			);

			File.WriteAllText(path, saveJson);
		}

		/// <summary>
		/// Lädt einen gespeicherten Spielstand aus einer JSON-Datei und übernimmt ihn ins Spielobjekt.
		/// </summary>
		/// <param name="game">Das Ziel-Spielobjekt, in das geladen werden soll.</param>
		/// <param name="filePath">Pfad zur Speicherstand-Datei.</param>
		public static void LoadGame(Game game, string filePath)
		{
			string json = File.ReadAllText(filePath);

			// Deserialisierung des Speicherstands
			var saveGame = JsonSerializer.Deserialize<SaveGame>(json);

			// Übernehmen der geladenen Werte in das laufende Spielobjekt
			game.CurrentSceneID = saveGame.CurrentSceneID;
			game.Flags = new HashSet<string>(saveGame.Flags);
		}
	}
}
