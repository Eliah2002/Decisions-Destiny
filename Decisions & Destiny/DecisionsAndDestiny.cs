using Decisions___Destiny.Helpers;
using Decisions___Destiny.Models;

namespace Decisions___Destiny
{
	public class DecisionsAndDestiny
	{
		private static DecisionsAndDestiny? singleton;
		public static DecisionsAndDestiny Singleton
		{
			get
			{
				if (singleton == null)
				{
					singleton = new DecisionsAndDestiny();
				}
				return singleton;
			}
			set { singleton = value; }
		}

		// three folders up because during runtime in bin\Debug\net7.0
		private readonly string baseJSONPath = Path.Combine(
			Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName,
			"JSON"
		);

		internal string SelectedGameName { get; set; } = String.Empty;
		internal string SelectedGameFolderPath => Path.Combine(baseJSONPath, SelectedGameName);

		private bool programIsRunning;
		public Menu? MainMenu { get; private set; }

		public void Start()
		{
			Console.CursorVisible = false;

			if (Directory.Exists(baseJSONPath) == false)
			{
				Printer.PrintError("JSON-Ordner wurde nicht gefunden.");
				return;
			}

			programIsRunning = true;

			while (programIsRunning)
			{
				Console.Clear();
				MainMenu = new Menu(new List<MenuItem>()
				{
					new MenuItem(true, "Neues Spiel starten", () => ShowGameSelection(StartNewGame)),
					new MenuItem(false, "Lade Spiel", () => ShowGameSelection(StartLoadedGame)),
					new MenuItem(false, "Beenden", () => programIsRunning = false)
				});

				var selected = MainMenu.Run();
				selected.Action?.Invoke();
			}
		}

		private void ShowGameSelection(Action onGameSelected)
		{
			var games = GetDirectories(baseJSONPath);

			if (games.Count == 0)
			{
				Printer.PrintError("Keine Spiele gefunden!");
				return;
			}

			RunMenuLoop(
				items: games,
				itemSelected: name =>
				{
					SelectedGameName = name;
					onGameSelected();
				},
				backTitle: "Zurück"
			);
		}

		private void StartNewGame()
		{
			string gameFile = Path.Combine(SelectedGameFolderPath, $"{SelectedGameName}.json");
			if (!File.Exists(gameFile))
			{
				Printer.PrintError("Spieldatei nicht gefunden.");
				return;
			}

			Game.Singleton.Start(gameFile);
		}

		private void StartLoadedGame()
		{
			var scores = GetDirectories(Path.Combine(baseJSONPath, SelectedGameName));

			if (scores.Count == 0)
			{
				Printer.PrintError("Keine gespeicherten Spielstände gefunden.\nBitte starte zuerst ein neues Spiel.");
				return;
			}

			RunMenuLoop(
				items: scores,
				itemSelected: score =>
				{
					StartLoadedGame(score);
				},
				backTitle: "Zurück"
			);
		}

		private void StartLoadedGame(string scoreName)
		{
			string saveFile = Path.Combine(SelectedGameFolderPath, scoreName, "save.json");
			if (!File.Exists(saveFile))
			{
				Printer.PrintError("Spielstand nicht gefunden.");
				return;
			}

			Game.Singleton.Start(saveFile);
			// TODO: Start game loop with loaded data
		}

		private void RunMenuLoop(List<string> items, Action<string> itemSelected, string backTitle)
		{
			bool inSelection = true;

			while (inSelection)
			{
				Console.Clear();

				var menuItems = items
					.Select((name, index) =>
						new MenuItem(index == 0, name, () =>
						{
							itemSelected(name);
							inSelection = false;
						}))
					.ToList();

				menuItems.Add(new MenuItem(false, backTitle, () => inSelection = false));

				MainMenu = new Menu(menuItems);
				var selected = MainMenu.Run();
				selected.Action?.Invoke();
			}
		}

		private List<string> GetDirectories(string path) =>
			Directory.Exists(path)
				? Directory.GetDirectories(path).Select(Path.GetFileName).Where(name => !string.IsNullOrEmpty(name)).ToList()!
				: new List<string>();
	}
}
