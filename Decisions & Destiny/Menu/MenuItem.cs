using Decisions___Destiny;
using Decisions___Destiny.Models;
using System.Text.Json;

public class MenuItem
{
	internal bool IsSafeMenuItem { get; set; } = false;
	internal bool Selected { get; set; }
	internal string? Title { get; set; }
	internal Action? Action { get; set; }

	public MenuItem(bool selected, string title, Action action)
	{
		Selected = selected;
		Title = title;
		Action = action;
	}

	public MenuItem(bool isSafeMenuItem)
	{
		IsSafeMenuItem = true;

		Action = ShowSaveMenu;
	}

	private void ShowSaveMenu()
	{
		bool inSelection = true;

		while (inSelection)
		{
			Console.Clear();

			var menuItems = new List<MenuItem>()
			{
				new MenuItem(true, "Speichern", () => { inSelection = false; Save(true); }),
				new MenuItem(false, "Nicht Speichern", () => { inSelection = false; Save(false); }),
				new MenuItem(false, "Zurück", () => inSelection = false)
			};
			var MainMenu = new Menu(menuItems);
			var selected = MainMenu.Run();
			selected.Action?.Invoke();
		}
	}

	private void Save(bool save)
	{
		if (save)
		{
			//TODO speicherstand benennen lassen
			string jsonSaveString = JsonSerializer.Serialize(Game.Singleton, new JsonSerializerOptions { WriteIndented = true });
			string path = Path.Combine(DecisionsAndDestiny.Singleton.SelectedGameFolderPath, "GespeicherteSpiele", "Spielstand 1.json");
			File.WriteAllText(path, jsonSaveString);
		}
		DecisionsAndDestiny.Singleton.Start();
	}
}