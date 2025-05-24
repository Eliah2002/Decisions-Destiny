using Decisions___Destiny.Helpers;

namespace Decisions___Destiny.Models
{
	/// <summary>
	/// Repräsentiert einen einzelnen Menüeintrag.
	/// </summary>
	public class MenuItem
	{
		/// <summary>
		/// True, wenn es sich um ein spezielles Menüelement handelt, z. B. zum Speichern oder für Escape-Verhalten.
		/// </summary>
		internal bool IsSafeMenuItem { get; set; } = false;

		internal bool Selected { get; set; }

		internal string? Title { get; set; }

		/// <summary>
		/// Die auszuführende Aktion, wenn der Eintrag ausgewählt wird.
		/// </summary>
		internal Action? Action { get; set; }

		public MenuItem(bool selected, string title, Action action)
		{
			Selected = selected;
			Title = title;
			Action = action;
		}

		/// <summary>
		/// Erstellt ein spezielles „Save“-Menüelement, das ein Untermenü aufruft.
		/// </summary>
		public MenuItem(bool isSafeMenuItem)
		{
			IsSafeMenuItem = isSafeMenuItem;
			Action = ShowSaveMenu;
		}

		/// <summary>
		/// Zeigt ein Untermenü zum Speichern an.
		/// </summary>
		private void ShowSaveMenu()
		{
			bool inSelection = true;

			while (inSelection)
			{
				Console.Clear();

				var menuItems = new List<MenuItem>()
				{
					new MenuItem(true,  "Speichern",        () => { inSelection = false; Save(true); }),
					new MenuItem(false, "Nicht speichern",  () => { inSelection = false; Save(false); }),
					new MenuItem(false, "Zurück",           () => inSelection = false)
				};

				var saveMenu = new Menu(menuItems);
				var selected = saveMenu.Run();
				selected.Action?.Invoke();
			}
		}

		/// <summary>
		/// Speichert das Spiel (optional) und startet das Hauptmenü neu.
		/// </summary>
		private void Save(bool save)
		{
			if (save)
			{
				Console.Write("Gib einen Namen für den Spielstand ein: ");
				string? saveName = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(saveName))
					saveName = "StandardSpielstand";

				SaveSystem.SaveGame(Game.Singleton, saveName);
			}

			// Zurück zum Startmenü
			DecisionsAndDestiny.Singleton.Start();
		}
	}
}