public class Menu
{
	public List<MenuItem> MenuItems { get; }

	public Menu(List<MenuItem> menuItems)
	{
		MenuItems = menuItems;

		if (MenuItems.Count > 0 && !MenuItems.Any(i => i.Selected))
		{
			MenuItems[0].Selected = true;
		}
	}

	public MenuItem Run()
	{
		while (true)
		{
			Console.Clear();
			DisplayMenu(highlightExit: true);

			var input = Console.ReadKey(true).Key;
			var command = ParseInput(input);

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
					return MenuItems.Last();//Last is exit
			}
		}
	}

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

			var input = Console.ReadKey(true).Key;
			var command = ParseInput(input);

			if (command == MenuCommand.Escape)
			{
				var saveManuItem = MenuItems.First(i => i.IsSafeMenuItem == true);
				if (saveManuItem != null)
				{
					return saveManuItem;
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

	private void DisplayMenu(bool highlightExit)
	{
		for (int i = 0; i < MenuItems.Count; i++)
		{
			var item = MenuItems[i];

			if (item.IsSafeMenuItem)
			{
				continue;
			}
			if (item.Selected)
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write(">");
			}
			else
			{
				Console.Write(" ");
			}

			if (!item.Selected && highlightExit && i == MenuItems.Count - 1)
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
			}

			Console.WriteLine($"{i + 1}. {item.Title}");
			Console.ResetColor();
		}
	}

	private void MoveSelection(int direction)
	{
		int currentIndex = MenuItems.FindIndex(i => i.Selected);
		if (currentIndex < 0) return;

		MenuItems[currentIndex].Selected = false;
		int newIndex = (currentIndex + direction + MenuItems.Count) % MenuItems.Count;
		MenuItems[newIndex].Selected = true;
	}

	private MenuCommand ParseInput(ConsoleKey key) =>
		key switch
		{
			ConsoleKey.Tab or ConsoleKey.DownArrow or ConsoleKey.S => MenuCommand.MoveDown,
			ConsoleKey.UpArrow or ConsoleKey.W => MenuCommand.MoveUp,
			ConsoleKey.Enter or ConsoleKey.Spacebar => MenuCommand.Select,
			ConsoleKey.Escape => MenuCommand.Escape,
			_ => MenuCommand.None
		};

	private enum MenuCommand
	{
		MoveUp,
		MoveDown,
		Select,
		Escape,
		None
	}
}
