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

        internal string SelectedGamePath
        {
            get
            {
				return Path.Combine(baseJSONPath, SelectedGameName);

            }
        }

		private bool ProgramIsRunning { get; set; }
		internal string SelectedGameName { get; set; } = String.Empty;
        public Menu? MainMenu { get; set; }

		public void Start()
		{
			Console.CursorVisible = false;

			if (Directory.Exists(baseJSONPath) == false)
			{
				Console.WriteLine("JSON-Ordner wurde nicht gefunden.");
				return;
			}

			ProgramIsRunning = true;

			while (ProgramIsRunning)
			{
				Console.Clear();
				MainMenu = new Menu(new List<MenuItem>()
				{
					new MenuItem(true, "Neues Spiel starten", () => ShowGameSelection(StartNewGame)),
					new MenuItem(false, "Lade Spiel", () => ShowGameSelection(StartLoadedGame)),
					new MenuItem(false, "Abbrechen", () => ProgramIsRunning = false)
				});

				var selected = MainMenu.Run();
				selected.Action();
			}
		}

		private void ShowGameSelection(Action onGameSelected)
		{
			var games = GetGames();

			if (games.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Keine Spiele gefunden!");
				Console.ResetColor();
				Console.ReadKey();
				return;
			}

			bool inSelection = true;
			while (inSelection)
			{
				Console.Clear();

				List<MenuItem> items = games
					.Select((name, index) =>
						new MenuItem(index == 0, name, () =>
						{
							SelectedGameName = name;
							onGameSelected();
							inSelection = false;
						}))
					.ToList();

				items.Add(new MenuItem(false, "Zurück", () => inSelection = false));

				MainMenu = new Menu(items);
				var selected = MainMenu.Run();
				selected.Action();
			}
		}

		private List<string?> GetGames()
		{
			if (!Directory.Exists(baseJSONPath))
			{
				return new List<string?>();
			}
			return Directory.GetDirectories(baseJSONPath).Select(Path.GetFileName).Where(name => name != null).ToList()!;
		}

		private List<string> GetScores()
		{
			var gamePath = Path.Combine(baseJSONPath, SelectedGameName);
			if (!Directory.Exists(gamePath))
			{
				return new List<string>();
			}
			return Directory.GetDirectories(gamePath).Select(Path.GetFileName).Where(name => name != null).ToList()!;
		}

		private void StartNewGame()
		{
			var game = new Game();
		}

		private void StartLoadedGame()
		{
			var scores = GetScores();

			if (scores.Count == 0)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Keine gespeicherten Spielstände gefunden.");
				Console.WriteLine("Bitte starte zuerst ein neues Spiel.");
				Console.ResetColor();
				Console.ReadKey();
				return;
			}

			bool inSelection = true;
			while (inSelection)
			{
				Console.Clear();

				List<MenuItem> items = scores
					.Select((score, index) =>
						new MenuItem(index == 0, score, () =>
						{
							StartLoadedGame(score);
							inSelection = false;
						}))
					.ToList();

				items.Add(new MenuItem(false, "Zurück", () => inSelection = false));

				MainMenu = new Menu(items);
				var selected = MainMenu.Run();
				selected.Action();
			}
		}

		private void StartLoadedGame(string scoreName)
		{
			var game = new Game(scoreName);
			//TODO
		}
	}
}
