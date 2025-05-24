using Decisions___Destiny.Enums;


namespace Decisions___Destiny.Models
{
	public class Menu
	{
		/// <summary>
		/// Alle Menüeinträge, die in diesem Menü angezeigt werden.
		/// </summary>
		public List<MenuItem> MenuItems { get; }

		/// <summary>
		/// Konstruktor: Initialisiert das Menü und wählt den ersten Eintrag standardmäßig aus.
		/// </summary>
		public Menu(List<MenuItem> menuItems)
		{
			MenuItems = menuItems;

			// Wählt automatisch den ersten Eintrag aus, falls keiner ausgewählt ist
			if (MenuItems.Count > 0 && !MenuItems.Any(i => i.Selected))
			{
				MenuItems[0].Selected = true;
			}
		}

		/// <summary>
		/// Startet das Menü (ohne Szenentext) mit Escape-Exit und Eingabe-Handling.
		/// </summary>
		public MenuItem Run()
		{
			while (true)
			{
				Console.Clear();
				DisplayMenu(highlightExit: true);

				var key = Console.ReadKey(true).Key;
				var command = ParseInput(key);

				switch (command)
				{
					case MenuCommand.MoveDown:
						MoveSelection(1);
						break;
					case MenuCommand.MoveUp:
						MoveSelection(-1);
						break;
					case MenuCommand.Select:
						return MenuItems.First(i => i.Selected);
					case MenuCommand.Escape:
						return MenuItems.Last(); // Letzter Eintrag ist Exit
				}
			}
		}

		/// <summary>
		/// Startet ein Szenenmenü mit zusätzlichem Szenentext über dem Menü.
		/// </summary>
		public MenuItem RunScene(string sceneText)
		{
			while (true)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine(sceneText);
				Console.ResetColor();
				Console.WriteLine();

				DisplayMenu(highlightExit: false);

				var key = Console.ReadKey(true).Key;
				var command = ParseInput(key);

				if (command == MenuCommand.Escape)
				{
					// Rückgabe eines speziellen Menüeintrags wie "Speichern"
					var safeItem = MenuItems.FirstOrDefault(i => i.IsSafeMenuItem);
					if (safeItem != null)
					{
						return safeItem;
					}
				}

				switch (command)
				{
					case MenuCommand.MoveDown:
						MoveSelection(1);
						break;
					case MenuCommand.MoveUp:
						MoveSelection(-1);
						break;
					case MenuCommand.Select:
						return MenuItems.First(i => i.Selected);
				}
			}
		}

		/// <summary>
		/// Zeigt die Menüeinträge im Terminal an.
		/// </summary>
		private void DisplayMenu(bool highlightExit)
		{
			for (int i = 0; i < MenuItems.Count; i++)
			{
				var item = MenuItems[i];

				// Spezielle Einträge (z. B. Speichern) nicht anzeigen
				if (item.IsSafeMenuItem)
					continue;

				// Auswahlmarker
				Console.Write(item.Selected ? ">" : " ");

				// Farbe für Exit-Eintrag am Ende
				if (!item.Selected && highlightExit && i == MenuItems.Count - 1)
				{
					Console.ForegroundColor = ConsoleColor.DarkRed;
				}
				else if (item.Selected)
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
				}

				Console.WriteLine($"{i + 1}. {item.Title}");
				Console.ResetColor();
			}
		}

		/// <summary>
		/// Ändert die aktuelle Auswahl im Menü.
		/// </summary>
		private void MoveSelection(int direction)
		{
			int currentIndex = MenuItems.FindIndex(i => i.Selected);
			if (currentIndex < 0) return;

			MenuItems[currentIndex].Selected = false;

			// Index modulo, damit Liste rotiert
			int newIndex = (currentIndex + direction + MenuItems.Count) % MenuItems.Count;
			MenuItems[newIndex].Selected = true;
		}

		/// <summary>
		/// Wandelt Tastendruck in Menüaktion um.
		/// </summary>
		private MenuCommand ParseInput(ConsoleKey key) =>
			key switch
			{
				ConsoleKey.Tab or ConsoleKey.DownArrow or ConsoleKey.S => MenuCommand.MoveDown,
				ConsoleKey.UpArrow or ConsoleKey.W => MenuCommand.MoveUp,
				ConsoleKey.Enter or ConsoleKey.Spacebar => MenuCommand.Select,
				ConsoleKey.Escape => MenuCommand.Escape,
				_ => MenuCommand.None
			};
	}
}