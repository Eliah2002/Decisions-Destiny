using Decisions___Destiny.Helpers;
using Decisions___Destiny.Models;

namespace Decisions___Destiny
{
	/// <summary>
	/// Hauptklasse des Spiels. Koordiniert Spielstart, Menüführung und Speicherverwaltung.
	/// </summary>
	public class DecisionsAndDestiny
	{
		private static DecisionsAndDestiny? singleton;
		public static DecisionsAndDestiny Singleton
		{
			get
			{
				if (singleton == null)
					singleton = new DecisionsAndDestiny();
				return singleton;
			}
			set { singleton = value; }
		}

		// Pfad zum JSON-Ordner (3 Ordner über /bin/Debug/net7.0 hinaus)
		private readonly string baseJSONPath = Path.Combine(
			Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName,
			"JSON"
		);

		// Ausgewähltes Spiel (Verzeichnisname)
		internal string SelectedGameName { get; set; } = string.Empty;

		// Absoluter Pfad zum aktuell gewählten Spielverzeichnis
		internal string SelectedGameFolderPath => Path.Combine(baseJSONPath, SelectedGameName);

		// Absoluter Pfad zur json-Datei des aktuell gewählten Spielverzeichnis
		internal string SelectedGameJSONPath => Path.Combine(SelectedGameFolderPath, $"{SelectedGameName}.json");

		// Absoluter Pfad zu den Spielständen des aktuell gewählten Spielverzeichnis
		internal string SelectedGameScoresFolderPath => Path.Combine(SelectedGameFolderPath, "GespeicherteSpiele");

		// Hauptmenü-Steuerung
		private bool programIsRunning;
		public Menu? MainMenu { get; private set; }

		/// <summary>
		/// Startet das Hauptmenü und die Spielsteuerung.
		/// </summary>
		public void Start()
		{
			Console.CursorVisible = false;

			if (!Directory.Exists(baseJSONPath))
			{
				Printer.PrintError("JSON-Ordner wurde nicht gefunden.");
				return;
			}

			programIsRunning = true;

			while (programIsRunning)
			{
				Console.Clear();

				var gameFolders = GetDirectories(baseJSONPath);
				if (gameFolders.Count == 0)
				{
					Printer.PrintError("Keine Spiele gefunden.");
					return;
				}

				var menuItems = gameFolders
					.Select((gameName, index) =>
						new MenuItem(index == 0, gameName, () =>
						{
							SelectedGameName = gameName;
							ShowGameOptions();
						}))
					.ToList();

				menuItems.Add(new MenuItem(false, "Beenden", () => programIsRunning = false));

				MainMenu = new Menu(menuItems);
				var selected = MainMenu.Run();
				selected.Action?.Invoke();
			}
		}

		/// <summary>
		/// Zeigt verfügbaren Spieleordner zur Auswahl an.
		/// </summary>
		private void ShowGameOptions()
		{
			bool inSubMenu = true;

			while (inSubMenu)
			{
				Console.Clear();
				var menuItems = new List<MenuItem>
				{
					new MenuItem(true, "Neues Spiel starten", () =>
					{
						inSubMenu = false;
						StartNewGame();
					}),
					new MenuItem(false, "Spiel laden", () =>
					{
						inSubMenu = false;
						StartLoadedGame();
					}),
					new MenuItem(false, "Zurück", () =>
					{
						inSubMenu = false;
					})
				};

				var subMenu = new Menu(menuItems);
				var selected = subMenu.Run();
				selected.Action?.Invoke();
			}
		}


		/// <summary>
		/// Startet ein neues Spiel durch Laden der Hauptspiel-JSON.
		/// </summary>
		private void StartNewGame()
		{
			if (!File.Exists(SelectedGameJSONPath))
			{
				Printer.PrintError("Spieldatei nicht gefunden.");
				return;
			}

			Game.Singleton.Start(SelectedGameJSONPath);
		}

		/// <summary>
		/// Zeigt gespeicherte Spielstände zur Auswahl an.
		/// </summary>
		private void StartLoadedGame()
		{
			var scores = Directory.GetFiles(SelectedGameScoresFolderPath, "*.json");
			List<string> scoreNames = new List<string>();
			foreach (var score in scores)
			{
				string scoreName = Path.GetFileNameWithoutExtension(score);
				scoreNames.Add(scoreName);
			}


			if (scoreNames.Count == 0)
			{
				Printer.PrintError("Keine gespeicherten Spielstände gefunden.\nBitte starte zuerst ein neues Spiel.");
				return;
			}

			RunMenuLoop(scoreNames, StartLoadedGame, "Zurück");
		}

		/// <summary>
		/// Startet ein Spiel aus einem gespeicherten Spielstand.
		/// </summary>
		private void StartLoadedGame(string scoreName)
		{
			string saveFile = Path.Combine(SelectedGameScoresFolderPath, $"{scoreName}.json");

			if (!File.Exists(saveFile))
			{
				Printer.PrintError("Spielstand nicht gefunden.");
				return;
			}

			SaveSystem.LoadGame(Game.Singleton, saveFile);
			Game.Singleton.Start(SelectedGameJSONPath, Game.Singleton.CurrentSceneID);
		}

		/// <summary>
		/// Führt eine Menüschleife aus mit beliebigen Einträgen.
		/// </summary>
		private void RunMenuLoop(List<string> items, Action<string> itemSelected, string backTitle)
		{
			bool inSelection = true;

			while (inSelection)
			{
				Console.Clear();

				var menuItems = items
					.Select((name, index) => new MenuItem(index == 0, name, () =>
					{
						itemSelected(name);
						inSelection = false;
					}))
					.ToList();

				// Option "Zurück"
				menuItems.Add(new MenuItem(false, backTitle, () => inSelection = false));

				MainMenu = new Menu(menuItems);
				var selected = MainMenu.Run();
				selected.Action?.Invoke();
			}
		}

		/// <summary>
		/// Holt Verzeichnisnamen aus einem Pfad.
		/// </summary>
		private List<string> GetDirectories(string path) =>
			Directory.Exists(path)
				? Directory.GetDirectories(path)
					.Select(Path.GetFileName)
					.Where(name => !string.IsNullOrEmpty(name))
					.ToList()
				: new List<string>();
	}
}
