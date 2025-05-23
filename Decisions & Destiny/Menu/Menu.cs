public class Menu
{
	public List<MenuItem> MenuItems { get; set; }
	private bool IsRunning { get; set; }

	public Menu(List<MenuItem> menuItems)
	{
		MenuItems = menuItems;
	}

	public MenuItem Run()
	{
		IsRunning = true;
		while (true)
		{
			Console.Clear();
			DrawMenu();
			var menuItem = MenuAction(Console.ReadKey());
			if (menuItem != null)
			{
				return menuItem;
			}
		}
	}

	private void DrawMenu()
	{
		for (int i = 0; i < MenuItems.Count; i++)
		{
			if (MenuItems[i].Selected)
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine($">{i + 1}. {MenuItems[i].Title}");
			}
			else
			{
				if (i == MenuItems.Count - 1) //Letztes Item ist Exit 
				{
					Console.ForegroundColor = ConsoleColor.DarkRed;
				}
				Console.WriteLine($"{i + 1}. {MenuItems[i].Title}");
			}
			Console.ResetColor();
		}
	}

	public MenuItem RunScene(string scenenText)
	{
		IsRunning = true;
		while (true)
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine(scenenText);
			Console.ResetColor();
			Console.WriteLine();
			DisplayChoices();
			var menuItem = MenuAction(Console.ReadKey());
			if (menuItem != null)
			{
				return menuItem;
			}
		}
	}

	private void DisplayChoices()
	{
		for (int i = 0; i < MenuItems.Count; i++)
		{
			if (MenuItems[i].Selected)
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine($">{i + 1}. {MenuItems[i].Title}");
			}
			else
			{
				Console.WriteLine($"{i + 1}. {MenuItems[i].Title}");
			}
			Console.ResetColor();
		}
	}

	private MenuItem MenuAction(ConsoleKeyInfo consoleKeyInfo)
	{
		int index;
		MenuItem menuItem;

		switch (consoleKeyInfo.Key)
		{
			case ConsoleKey.Tab:
			case ConsoleKey.DownArrow:
			case ConsoleKey.S:
				index = MenuItems.FindIndex(i => i.Selected);
				menuItem = MenuItems[index];
				menuItem.Selected = false;
				menuItem = MenuItems[(index + 1) % MenuItems.Count];
				menuItem.Selected = true;
				return null;

			case ConsoleKey.Enter:
			case ConsoleKey.Spacebar:
				return MenuItems.First(i => i.Selected);

			case ConsoleKey.UpArrow:
			case ConsoleKey.W:
				index = MenuItems.FindIndex(i => i.Selected);
				menuItem = MenuItems[index];
				menuItem.Selected = false;
				menuItem = MenuItems[(index + MenuItems.Count - 1) % MenuItems.Count];
				menuItem.Selected = true;
				return null;
			default:
				return null;
		}
	}
}